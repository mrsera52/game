using UnityEngine;

public static class BallisticCalculator
{
    public static bool TrySolveAngle(
        Vector3 origin, Vector3 target, float speed, bool highArc,
        out float yawDegrees, out float pitchDegrees)
    {
        Vector3 diff = target - origin;
        Vector3 diffXZ = new Vector3(diff.x, 0f, diff.z);
        float distance = diffXZ.magnitude;
        float heightDiff = diff.y;
        float g = -Physics.gravity.y;

        yawDegrees = Mathf.Atan2(diffXZ.x, diffXZ.z) * Mathf.Rad2Deg;

        float v2 = speed * speed;
        float v4 = v2 * v2;
        float root = v4 - g * (g * distance * distance + 2f * heightDiff * v2);

        if (root < 0f)
        {
            pitchDegrees = 45f;
            return false;
        }

        root = Mathf.Sqrt(root);
        float lowAngle = Mathf.Atan2(v2 - root, g * distance);
        float highAngle = Mathf.Atan2(v2 + root, g * distance);

        pitchDegrees = (highArc ? highAngle : lowAngle) * Mathf.Rad2Deg;
        return true;
    }
}