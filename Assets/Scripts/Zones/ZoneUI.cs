using UnityEngine;
using TMPro;

public class ZoneUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI timerText;

    [Header("Ссылка на пушку")]
    public Transform cannon;

    void Update()
    {
        if (ZoneManager.Instance == null || cannon == null)
            return;

        var z = ZoneManager.Instance.activeZone;
        if (z == null)
            return;

        // ?? внутри зоны?
        bool inside = ZoneManager.Instance.IsCannonInsideActiveZone(cannon.position);

        // ?? расстояние до зоны
        float dist = ZoneManager.Instance.DistanceToActiveZone(cannon.position);

        // ?? время до смены зоны
        float left = ZoneManager.Instance.timeLeft;

        // ?? погода
        string weather = WeatherPresets.GetLabel(z.weather);

        // ?? текст статуса
        if (inside)
        {
            statusText.text = $"<color=#5fff5f>В ЗОНЕ ({weather}) — стрельба разрешена</color>";
        }
        else
        {
            statusText.text = $"<color=#ffff5f>ВНЕ ЗОНЫ ({weather}) — {dist:F0} м</color>";
        }

        // ?? таймер
        timerText.text = $"Смена через: {left:F0} сек";
    }
}