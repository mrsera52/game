using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [Header("ѕараметры")]
    public float damage = 100f;
    public float lifetime = 10f;

    [Header("Ёффекты")]
    public GameObject hitExplosionPrefab;
    public bool explodeOnAnything;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = transform.position;
        Vector3 normal = collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector3.up;
        Quaternion rot = Quaternion.LookRotation(normal);

        if (collision.contactCount > 0)
        {
            ContactPoint cp = collision.GetContact(0);
            pos = cp.point;
            normal = cp.normal;
            rot = Quaternion.LookRotation(normal);
        }

        FortressHealth fh = FindFortressHealth(collision);

        if (fh != null)
        {
            fh.TakeDamage(damage);
#if UNITY_EDITOR
            Debug.Log("”рон по крепости");
#endif
        }

        if (hitExplosionPrefab != null && (explodeOnAnything || fh != null))
            Instantiate(hitExplosionPrefab, pos, rot);

        Destroy(gameObject);
    }

    static FortressHealth FindFortressHealth(Collision collision)
    {
        Transform t = collision.collider.transform;
        while (t != null)
        {
            if (t.CompareTag("Fortress"))
            {
                var fh = t.GetComponent<FortressHealth>();
                if (fh != null) return fh;
            }
            t = t.parent;
        }

        var onCollider = collision.collider.GetComponent<FortressHealth>();
        return onCollider != null ? onCollider : collision.collider.GetComponentInParent<FortressHealth>();
    }
}