using UnityEngine;

public class ZoneVisualOnTerrain : MonoBehaviour
{
    [Header("Terrain")]
    public Terrain terrain;

    [Header("ѕараметры")]
    public int gridResolution = 24;
    public float radius = 12f;
    public float pointSize = 1.2f;
    public float heightOffset = 0.1f;
    public Color color = new Color(0.3f, 1f, 0.3f, 0.5f);

    [Header("—сылки на материал")]
    public Material decalMaterial;

    void Start()
    {
        if (terrain == null) terrain = Terrain.activeTerrain;
        BuildGrid();
    }

    public void Configure(float r, Color c)
    {
        radius = r;
        color = c;
    }

    void BuildGrid()
    {
        float step = (radius * 2f) / gridResolution;
        Vector3 origin = transform.position - new Vector3(radius, 0, radius);

        for (int x = 0; x < gridResolution; x++)
        {
            for (int z = 0; z < gridResolution; z++)
            {
                Vector3 p = origin + new Vector3(x * step + step * 0.5f, 0, z * step + step * 0.5f);
                Vector2 dxz = new Vector2(p.x - transform.position.x, p.z - transform.position.z);
                if (dxz.magnitude > radius) continue;

                if (terrain != null)
                    p.y = terrain.SampleHeight(p) + terrain.transform.position.y + heightOffset;

                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Destroy(quad.GetComponent<Collider>());
                quad.transform.SetParent(transform);
                quad.transform.position = p;
                quad.transform.rotation = Quaternion.Euler(90, 0, 0);
                quad.transform.localScale = new Vector3(pointSize, pointSize, 1);

                var rend = quad.GetComponent<Renderer>();
                if (decalMaterial != null)
                    rend.material = new Material(decalMaterial);
                rend.material.color = color;
            }
        }
    }
}