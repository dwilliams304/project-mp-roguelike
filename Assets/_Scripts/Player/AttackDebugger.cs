using UnityEngine;

public static class AttackDebugger
{
    public static void DrawDebugPoint(Vector3 pos, Color color, float duration = 1f, float size = 0.4f)
    {
        Debug.DrawRay(pos, Vector3.up * size, color, duration);
        Debug.DrawRay(pos, Vector3.right * size, color, duration);
        Debug.DrawRay(pos, Vector3.forward * size, color, duration);
    }

    public static void DrawDebugLine(Vector3 startPos, Vector3 endPos, Color color, float duration = 1f)
    {
        Debug.DrawLine(startPos, endPos, color, duration);
    }

    public static void DrawMeleeSwing(Vector3 origin, Vector3 forward, float radius, float swingAngleDegrees, Color color, float duration = 1f, int segments = 20)
    {
        float halfAngle = swingAngleDegrees / 2f;

        Quaternion leftRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfAngle, 0);

        Vector3 leftDir = leftRotation * forward;
        Vector3 rightDir = rightRotation * forward;

        // Draw left and right edges
        Debug.DrawLine(origin, origin + leftDir * radius, color, duration);
        Debug.DrawLine(origin, origin + rightDir * radius, color, duration);

        // Draw arc segments
        float angleStep = swingAngleDegrees / segments;
        Vector3 lastPoint = origin + leftDir * radius;
        for (int i = 1; i <= segments; i++)
        {
            Quaternion stepRotation = Quaternion.Euler(0, -halfAngle + angleStep * i, 0);
            Vector3 nextDir = stepRotation * forward;
            Vector3 nextPoint = origin + nextDir * radius;
            Debug.DrawLine(lastPoint, nextPoint, color, duration);
            lastPoint = nextPoint;
        }
    }

}