using UnityEngine;

public class LightFlash : MonoBehaviour
{
    public float peak = 2f;

    Light L;
    float t;

    void Start()
    {
        L = GetComponent<Light>();
        if (L != null) L.intensity = peak;
        t = 0.15f;
    }

    void Update()
    {
        if (L == null) return;
        t -= Time.deltaTime;
        if (t > 0) L.intensity = peak * (t / 0.15f);
        else { L.intensity = 0f; Destroy(L); }
    }
}