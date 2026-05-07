using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;

    [Header("Привязка")]
    public Transform fortress;

    [Header("Размещение")]
    public int zoneCount = 5;
    public float minDistanceFromFortress = 25f;
    public float maxDistanceFromFortress = 80f;
    public float minDistanceBetweenZones = 18f;
    public float groundY = 0f;

    [Header("Параметры зоны")]
    public float zoneRadius = 12f;
    public float zoneDuration = 60f;

    [Header("Префабы")]
    public GameObject zoneVisualPrefab;
    public GameObject rainFxPrefab;
    public GameObject snowFxPrefab;
    public GameObject fogFxPrefab;

    public List<WeatherZone> zones { get; private set; } = new();
    public WeatherZone activeZone { get; private set; }
    public float timeLeft { get; private set; }

    void Awake() => Instance = this;

    void Start()
    {
        BuildZones();
        ActivateZone(0);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            int next = (zones.IndexOf(activeZone) + 1) % zones.Count;
            ActivateZone(next);
        }
    }

    void BuildZones()
    {
        WeatherType[] all = { WeatherType.Clear, WeatherType.Rain, WeatherType.Wind, WeatherType.Fog, WeatherType.Snow };

        for (int i = 0; i < zoneCount; i++)
        {
            Vector3 pos = FindFreePosition();
            WeatherZone z = new WeatherZone
            {
                center = pos,
                radius = zoneRadius,
                weather = all[i % all.Length],
                windDirection = Random.onUnitSphere,
                windSpeed = Random.Range(0f, 15f)
            };
            z.windDirection.y = 0;
            z.windDirection.Normalize();

            CreateVisual(z);
            zones.Add(z);
        }
    }

    Vector3 FindFreePosition()
    {
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Vector2 dir2D = Random.insideUnitCircle.normalized;
            float dist = Random.Range(minDistanceFromFortress, maxDistanceFromFortress);
            Vector3 candidate = fortress.position + new Vector3(dir2D.x * dist, 0, dir2D.y * dist);
            candidate.y = groundY;

            bool ok = true;
            foreach (var existing in zones)
            {
                if (Vector3.Distance(existing.center, candidate) < minDistanceBetweenZones)
                {
                    ok = false;
                    break;
                }
            }
            if (ok) return candidate;
        }

        Vector2 fallback = Random.insideUnitCircle.normalized * maxDistanceFromFortress;
        return fortress.position + new Vector3(fallback.x, 0, fallback.y);
    }

    void CreateVisual(WeatherZone z)
    {
        if (zoneVisualPrefab == null) return;

        z.visual = Instantiate(zoneVisualPrefab, z.center, Quaternion.identity);

        var visual = z.visual.GetComponent<ZoneVisualOnTerrain>();
        if (visual != null)
        {
            visual.Configure(z.radius, WeatherPresets.GetColor(z.weather));
        }
        else
        {
            z.visual.transform.localScale = new Vector3(z.radius * 2, 0.1f, z.radius * 2);
            Renderer r = z.visual.GetComponentInChildren<Renderer>();
            if (r != null) r.material.color = WeatherPresets.GetColor(z.weather);
        }

        z.visual.SetActive(false);
    }

    void ActivateZone(int index)
    {
        if (zones.Count == 0) return;
        index = Mathf.Clamp(index, 0, zones.Count - 1);

        if (activeZone != null)
        {
            if (activeZone.visual != null) activeZone.visual.SetActive(false);
            if (activeZone.weatherFx != null) Destroy(activeZone.weatherFx);
        }

        activeZone = zones[index];
        if (activeZone.visual != null) activeZone.visual.SetActive(true);

        SpawnWeatherFx(activeZone);
        timeLeft = zoneDuration;

        Debug.Log($"Активна зона {index}: {WeatherPresets.GetLabel(activeZone.weather)} в {activeZone.center}");
    }

    void SpawnWeatherFx(WeatherZone z)
    {
        GameObject prefab = null;
        switch (z.weather)
        {
            case WeatherType.Rain: prefab = rainFxPrefab; break;
            case WeatherType.Snow: prefab = snowFxPrefab; break;
            case WeatherType.Fog: prefab = fogFxPrefab; break;
        }
        if (prefab != null)
            z.weatherFx = Instantiate(prefab, z.center + Vector3.up * 10f, prefab.transform.rotation);
    }

    public bool IsCannonInsideActiveZone(Vector3 cannonPos)
    {
        return activeZone != null && activeZone.Contains(cannonPos);
    }

    public float DistanceToActiveZone(Vector3 cannonPos)
    {
        if (activeZone == null) return float.MaxValue;
        Vector2 a = new Vector2(activeZone.center.x, activeZone.center.z);
        Vector2 b = new Vector2(cannonPos.x, cannonPos.z);
        return Vector2.Distance(a, b) - activeZone.radius;
    }
}