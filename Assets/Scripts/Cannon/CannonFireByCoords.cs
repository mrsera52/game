using System.Collections;
using UnityEngine;

public class CannonFireByCoords : MonoBehaviour
{
    [Header("Базовые параметры (резерв)")]
    public GameObject defaultProjectilePrefab;
    public float defaultLaunchSpeed = 60f;

    [Header("Пушка")]
    public Transform muzzle;
    public float reloadTime = 1.5f;

    [Header("Стрельба")]
    public bool useHighArc = false;

    private CannonController cannon;
    private float lastFireTime = -999f;

    void Start()
    {
        cannon = GetComponent<CannonController>();
    }

    public void AimAndFire(Vector3 target, System.Action<string, bool> onResult)
    {
        if (Time.time - lastFireTime < reloadTime)
        {
            float left = reloadTime - (Time.time - lastFireTime);
            onResult?.Invoke($"Перезарядка: {left:F1} сек", false);
            return;
        }

        if (ZoneManager.Instance != null && !ZoneManager.Instance.IsCannonInsideActiveZone(transform.position))
        {
            float dist = ZoneManager.Instance.DistanceToActiveZone(transform.position);
            onResult?.Invoke($"Пушка вне зоны! До зоны {dist:F0} м", false);
            return;
        }

        ProjectileData data = ProjectileSelector.Instance != null ? ProjectileSelector.Instance.Current : null;
        GameObject prefab = data != null && data.prefab != null ? data.prefab : defaultProjectilePrefab;
        float speed = data != null ? data.muzzleSpeed : defaultLaunchSpeed;
        float overshoot = data != null ? data.overshootMultiplier : 1f;

        Vector3 origin = muzzle.position;

        Vector3 effectiveTarget = ApplyOvershoot(origin, target, overshoot);

        if (!BallisticCalculator.TrySolveAngle(origin, effectiveTarget, speed,
                useHighArc, out float yaw, out float pitch))
        {
            onResult?.Invoke("Цель вне зоны досягаемости", false);
            return;
        }

        cannon.SetAimTarget(yaw, pitch);
        StartCoroutine(WaitForAimAndFire(yaw, pitch, target, prefab, speed, data, onResult));
    }

    Vector3 ApplyOvershoot(Vector3 origin, Vector3 target, float multiplier)
    {
        if (Mathf.Approximately(multiplier, 1f)) return target;

        Vector3 flat = target - origin;
        flat.y = 0f;
        Vector3 extended = origin + flat * multiplier;
        extended.y = target.y;
        return extended;
    }

    IEnumerator WaitForAimAndFire(float yaw, float pitch, Vector3 target,
                                  GameObject prefab, float speed, ProjectileData data,
                                  System.Action<string, bool> onResult)
    {
        onResult?.Invoke("Наведение на цель...", true);

        float timeout = 5f, t = 0f;
        while (!cannon.IsAimedAt(yaw, pitch) && t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (t >= timeout)
        {
            onResult?.Invoke("Не удалось навестись (тайм-аут)", false);
            yield break;
        }

        Fire(prefab, speed, data);
        lastFireTime = Time.time;
        onResult?.Invoke($"Огонь {data?.displayName} по ({target.x:F0}, {target.y:F0}, {target.z:F0})!", true);
    }

    void Fire(GameObject prefab, float speed, ProjectileData data)
    {
        GameObject p = Instantiate(prefab, muzzle.position, muzzle.rotation);
        Rigidbody rb = p.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = muzzle.forward * speed;

        var sp = p.GetComponent<SimpleProjectile>();
        if (sp != null && data != null)
            sp.damage *= data.damageMultiplier;
    }
}