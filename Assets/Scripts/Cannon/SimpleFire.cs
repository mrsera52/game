using UnityEngine;

public class SimpleFire : MonoBehaviour
{
    [Header("Снаряд")]
    public GameObject projectilePrefab;
    public Transform muzzle;
    public float launchSpeed = 60f;
    public float reloadTime = 1.5f;

    [Header("Стрельба")]
    public bool useHighArc = false;
    public bool waitForAimBeforeFire = true;

    private CannonController cannon;
    private TargetSelector selector;
    private float lastFireTime = -999f;
    private bool firePending;

    void Start()
    {
        cannon = GetComponent<CannonController>();
        selector = GetComponent<TargetSelector>();
    }

    void Update()
    {
        if (selector != null && selector.hasTarget)
        {
            UpdateAim();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selector == null || !selector.hasTarget)
            {
                Debug.Log("Сначала выберите цель ЛКМ");
                return;
            }

            if (Time.time - lastFireTime < reloadTime)
            {
                Debug.Log("Перезарядка...");
                return;
            }

            firePending = true;
        }

        if (firePending && selector != null && selector.hasTarget)
        {
            TrySolveAndFire();
        }
    }

    void UpdateAim()
    {
        Vector3 origin = muzzle != null ? muzzle.position : transform.position;

        if (selector.confirmedTarget.HasValue &&
            BallisticCalculator.TrySolveAngle(
                origin,
                selector.confirmedTarget.Value,
                launchSpeed,
                useHighArc,
                out float yaw,
                out float pitch))
        {
            cannon.SetAimTarget(yaw, pitch);
        }
    }

    void TrySolveAndFire()
    {
        if (!selector.confirmedTarget.HasValue)
            return;

        Vector3 origin = muzzle.position;

        if (!BallisticCalculator.TrySolveAngle(
                origin,
                selector.confirmedTarget.Value,
                launchSpeed,
                useHighArc,
                out float yaw,
                out float pitch))
        {
            Debug.Log("Цель вне зоны досягаемости");
            firePending = false;
            return;
        }

        if (waitForAimBeforeFire && !cannon.IsAimedAt(yaw, pitch))
            return;

        Fire();
        firePending = false;
    }

    void Fire()
    {
        if (projectilePrefab == null || muzzle == null)
        {
            Debug.LogWarning("Не задан projectilePrefab или muzzle");
            return;
        }

        GameObject p = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        Rigidbody rb = p.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = muzzle.forward * launchSpeed; // ?? исправлено
        }

        lastFireTime = Time.time;
    }
}