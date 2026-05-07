using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CoordinateIndicator : MonoBehaviour
{
    [Header("Маркер под мышью")]
    public GameObject hoverMarkerPrefab;

    [Header("UI отображения координат")]
    public TextMeshProUGUI coordinatesText;

    [Header("Слой для рейкаста")]
    public LayerMask aimMask = ~0;

    private GameObject hoverMarker;
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
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (hoverMarker != null) hoverMarker.SetActive(false);
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
        {
            if (hoverMarker != null)
            {
                hoverMarker.SetActive(true);
                hoverMarker.transform.position = hit.point + Vector3.up * 0.05f;
            }
            if (coordinatesText != null)
            {
                Vector3 p = hit.point;
                coordinatesText.text = $"X: {p.x:F1}   Y: {p.y:F1}   Z: {p.z:F1}";
            }
        }
        else
        {
            if (hoverMarker != null) hoverMarker.SetActive(false);
        }
    }
}