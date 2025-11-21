using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Path : MonoBehaviour
{
    [System.Serializable]
    struct Waypoint
    {
        public float _x;
        public float _y;
        public bool _curve;     // use curve or not for this segment
        public float _curveFactor; // "how curvy" the segment is
    }

    [SerializeField] Waypoint[] _pathWaypoints;

    [Header("Path Settings")]
    [SerializeField] bool loop = false;

    [Header("Visual Settings")]
    [SerializeField] Color waypointColor = Color.yellow;
    [SerializeField] Color lineColor = Color.white;
    [SerializeField] Color curveColor = Color.cyan;
    [SerializeField] float waypointSize = 0.05f;
    [SerializeField] int curveSegments = 20;

    void OnDrawGizmos() { DrawPath(); }
    void OnDrawGizmosSelected() { DrawPath(); }

    void DrawPath()
    {
        if (_pathWaypoints == null || _pathWaypoints.Length < 2)
            return;

        int count = _pathWaypoints.Length;

        // Get world-space positions
        Vector3[] pos = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            pos[i] = transform.TransformPoint(
                new Vector3(_pathWaypoints[i]._x, _pathWaypoints[i]._y, 0));
        }

        // Draw waypoints
        Gizmos.color = waypointColor;
        foreach (var p in pos)
            Gizmos.DrawSphere(p, waypointSize);

        // Draw segments
        int end = loop ? count : count - 1;

        for (int i = 0; i < end; i++)
        {
            int next = (i + 1) % count; // works for both loop & non-loop

            Vector3 a = pos[i];
            Vector3 b = pos[next];
            Waypoint w = _pathWaypoints[i];

            // Straight segment
            if (!w._curve || Mathf.Approximately(w._curveFactor, 0f))
            {
                Gizmos.color = lineColor;
                Gizmos.DrawLine(a, b);
                continue;
            }

            // Build curved segment using midpoint offset
            Vector3 midpoint = (a + b) * 0.5f;

            // perpendicular to segment
            Vector3 dir = (b - a).normalized;
            Vector3 perp = new Vector3(-dir.y, dir.x, 0);

            // how strongly it bends
            Vector3 control = midpoint + perp * w._curveFactor;

            Gizmos.color = curveColor;

            Vector3 prev = a;
            for (int s = 1; s <= curveSegments; s++)
            {
                float t = s / (float)curveSegments;
                Vector3 q = QuadraticBezier(a, control, b, t);
                Gizmos.DrawLine(prev, q);
                prev = q;
            }
        }

#if UNITY_EDITOR
        // Optional labels
        for (int i = 0; i < pos.Length; i++)
            Handles.Label(pos[i] + Vector3.up * 0.1f, $"#{i}");
#endif
    }

    static Vector3 QuadraticBezier(Vector3 a, Vector3 c, Vector3 b, float t)
    {
        float u = 1 - t;
        return (u * u * a) + (2 * u * t * c) + (t * t * b);
    }
}
