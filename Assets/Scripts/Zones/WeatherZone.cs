using UnityEngine;

public enum WeatherType { Clear, Rain, Wind, Fog, Snow }

[System.Serializable]
public class WeatherZone
{
    public Vector3 center;
    public float radius = 12f;
    public WeatherType weather;

    public Vector3 windDirection = Vector3.right;
    public float windSpeed;

    [HideInInspector] public GameObject visual;
    [HideInInspector] public GameObject weatherFx;

    public bool Contains(Vector3 worldPos)
    {
        Vector2 a = new Vector2(center.x, center.z);
        Vector2 b = new Vector2(worldPos.x, worldPos.z);
        return Vector2.Distance(a, b) <= radius;
    }
}