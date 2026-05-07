using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    [Header("Ìàðêåðû")]
    public GameObject hoverMarkerPrefab;
    public GameObject confirmedMarkerPrefab;

    [Header("Ñëîé çåìëè/êðåïîñòè")]
    public LayerMask aimMask = ~0;

    // dfdsasapdsdaasd
    public Vector3? confirmedTarget { get; private set; }
    public bool hasTarget => confirmedTarget.HasValue;

    private GameObject hoverMarker;
    private GameObject confirmedMarker;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (hoverMarkerPrefab != null)
        {
            hoverMarker = Instantiate(hoverMarkerPrefab);
            hoverMarker.SetActive(false);
        }
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
        {
            if (hoverMarker != null)
            {
                hoverMarker.SetActive(true);
                hoverMarker.transform.position = hit.point + Vector3.up * 0.05f;
            }

            if (Input.GetMouseButtonDown(0))
            {
                ConfirmTarget(hit.point);
            }
        }
        else if (hoverMarker != null)
        {
            hoverMarker.SetActive(false);
        }
    }








    void ConfirmTarget(Vector3 point)
    {
        confirmedTarget = point;

        if (confirmedMarker != null)
            Destroy(confirmedMarker);

        if (confirmedMarkerPrefab != null)
        {
            confirmedMarker = Instantiate(
                confirmedMarkerPrefab,
                point + Vector3.up * 0.05f,
                Quaternion.identity
            );
        }
    }
}
