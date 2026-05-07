using UnityEngine;

public static class WeatherPresets
{
    public static Color GetColor(WeatherType w)
    {
        switch (w)
        {
            case WeatherType.Clear: return new Color(0.4f, 1f, 0.4f, 0.4f);
            case WeatherType.Rain: return new Color(0.4f, 0.6f, 1f, 0.5f);
            case WeatherType.Wind: return new Color(1f, 0.9f, 0.4f, 0.5f);
            case WeatherType.Fog: return new Color(0.85f, 0.85f, 0.85f, 0.5f);
            case WeatherType.Snow: return new Color(0.9f, 0.95f, 1f, 0.5f);
        }
        return Color.white;
    }

    public static string GetLabel(WeatherType w)
    {
        switch (w)
        {
            case WeatherType.Clear: return "ясно";
            case WeatherType.Rain: return "ƒождь";
            case WeatherType.Wind: return "ясно";
            case WeatherType.Fog: return "“уман";
            case WeatherType.Snow: return "—нег";
        }
        return "?";
    }
}