using UnityEngine;

public class AttackGizmoDrawer : MonoBehaviour
{
    public static AttackGizmoDrawer Instance;

    private Vector3 position;
    private Vector3 forward;
    private float radius;
    private float angle;
    private Color color;
    private float expireTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void DrawMeleeSwing(Vector3 _position, Vector3 _forward, float _radius, float _angle, Color _color, float _duration)
    {
        position = _position;
        forward = _forward;
        radius = _radius;
        angle = _angle;
        color = _color;
        expireTime = Time.time + _duration;
    }

    private void OnDrawGizmos()
    {
        if (Time.time > expireTime) return;
        
        if (radius <= 0f || angle <= 0f) return;

        Gizmos.color = color;

        int segments = 30;
        float step = angle / segments;
        Quaternion startRotation = Quaternion.Euler(0, -angle / 2, 0);
        Vector3 lastPoint = position + (startRotation * forward) * radius;

        for (int i = 1; i <= segments; i++)
        {
            Quaternion nextRotation = Quaternion.Euler(0, -angle / 2 + step * i, 0);
            Vector3 nextPoint = position + (nextRotation * forward) * radius;

            Gizmos.DrawLine(position, lastPoint);
            Gizmos.DrawLine(lastPoint, nextPoint);
            Gizmos.DrawLine(nextPoint, position);

            lastPoint = nextPoint;
        }
    }
}
