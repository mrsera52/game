using UnityEngine;

public class StickToTerrain : MonoBehaviour
{
    [Header("Terrains (можно несколько)")]
    public Terrain[] terrains;

    [Header("Параметры")]
    public float heightOffset = 0f;
    public bool alignToSlope = true;
    public float slopeSmooth = 8f;

    [Header("Резервная высота (если не на терреине)")]
    public float fallbackHeight = 0f;

    void LateUpdate()
    {
        Terrain t = FindTerrainUnder(transform.position);

        Vector3 pos = transform.position;

        if (t != null)
        {
            float h = t.SampleHeight(pos) + t.transform.position.y;
            pos.y = h + heightOffset;
        }
        else
        {
            pos.y = fallbackHeight + heightOffset;
        }

        transform.position = pos;

        if (alignToSlope && t != null)
        {
            Vector3 normal = GetTerrainNormal(t, pos);
            Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, normal) *
                                   Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * slopeSmooth);
        }
    }

    Terrain FindTerrainUnder(Vector3 worldPos)
    {
        if (terrains == null) return null;

        foreach (var t in terrains)
        {
            if (t == null) continue;

            Vector3 origin = t.transform.position;
            Vector3 size = t.terrainData.size;

            if (worldPos.x >= origin.x &&
                worldPos.x <= origin.x + size.x &&
                worldPos.z >= origin.z &&
                worldPos.z <= origin.z + size.z)
            {
                return t;
            }
        }
        return null;
    }

    Vector3 GetTerrainNormal(Terrain t, Vector3 worldPos)
    {
        TerrainData td = t.terrainData;
        Vector3 terrainPos = t.transform.position;

        float u = (worldPos.x - terrainPos.x) / td.size.x;
        float v = (worldPos.z - terrainPos.z) / td.size.z;

        return td.GetInterpolatedNormal(
            Mathf.Clamp01(u),
            Mathf.Clamp01(v));
    }
}