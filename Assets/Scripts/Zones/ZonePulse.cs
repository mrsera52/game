using UnityEngine;

public class ZonePulse : MonoBehaviour
{
    public Color normalColor = new Color(0.2f, 1f, 0.2f, 0.4f);
    public Color warningColor = new Color(1f, 0.2f, 0.2f, 0.6f);
    public float warningThreshold = 10f;
    public float pulseSpeed = 4f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float left = ZoneManager.Instance != null ? ZoneManager.Instance.timeLeft : 100f;
        Color baseColor = (left < warningThreshold) ? warningColor : normalColor;

        if (left < warningThreshold)
        {
            float a = Mathf.Lerp(0.2f, 0.7f, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            baseColor.a = a;
        }

        mat.color = baseColor;
    }
}