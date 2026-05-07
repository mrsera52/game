using UnityEngine;

public class ZoneArrow : MonoBehaviour
{
    public Transform cannon;
    public GameObject arrowPrefab;
    public float heightAboveCannon = 4f;

    private GameObject arrow;

    void Start()
    {
        if (arrowPrefab != null)
        {
            arrow = Instantiate(arrowPrefab);
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        if (arrow == null || cannon == null || ZoneManager.Instance == null) return;

        var z = ZoneManager.Instance.activeZone;
        if (z == null)
        {
            arrow.SetActive(false);
            return;
        }

        bool inside = ZoneManager.Instance.IsCannonInsideActiveZone(cannon.position);
        if (inside)
        {
            arrow.SetActive(false);
            return;
        }

        arrow.SetActive(true);

        Vector3 dir = z.center - cannon.position;
        dir.y = 0f;

        arrow.transform.position = cannon.position + Vector3.up * heightAboveCannon;
        if (dir.sqrMagnitude > 0.01f)
            arrow.transform.rotation = Quaternion.LookRotation(dir);
    }
}