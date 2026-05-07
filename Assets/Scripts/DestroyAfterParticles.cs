using UnityEngine;

public class DestroyAfterParticles : MonoBehaviour
{
    public float extraSeconds = 0.35f;

    void Start()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null && ps.main.loop == false)
        {
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax + extraSeconds);
        }
        else
            Destroy(gameObject, 2f);
    }
}