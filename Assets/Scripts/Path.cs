using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Path : MonoBehaviour
{
    [System.Serializable]
    public struct Waypoint
    {
        public float _x;
        public float _y;
        public bool _curve;
        public float _curveFactor;
    }

    public Waypoint[] _pathWaypoints;

    [HideInInspector] public List<List<Vector3>> _pathSegments;

    public void OnEnable()
    {
        _pathSegments = new List<List<Vector3>>();
        for (int i = 0; i < _pathWaypoints.Length-1; i++) {
            _pathSegments.Add(GetSegmentPoints(
                _pathWaypoints[i],
                new Vector3(_pathWaypoints[i]._x, _pathWaypoints[i]._y),
                new Vector3(_pathWaypoints[i + 1]._x, _pathWaypoints[i+1]._y)
            ));
        }

        _pathSegments.Add(GetSegmentPoints(
            _pathWaypoints[_pathWaypoints.Length - 1],
            new Vector3(_pathWaypoints[_pathWaypoints.Length - 1]._x, _pathWaypoints[_pathWaypoints.Length - 1]._y),
            new Vector3(_pathWaypoints[0]._x, _pathWaypoints[0]._y)
        ));
    }

    List<Vector3> GetSegmentPoints(Waypoint w, Vector3 a, Vector3 b, int segments = 20)
    {
        List<Vector3> pts = new List<Vector3>();

        if (!w._curve || Mathf.Approximately(w._curveFactor, 0f))
        {
            pts.Add(a);
            pts.Add(b);
            return pts;
        }

        // Same midpoint offset logic as your Gizmo
        Vector3 midpoint = (a + b) * 0.5f;
        Vector3 dir = (b - a).normalized;
        Vector3 perp = new Vector3(-dir.y, dir.x, 0);
        Vector3 control = midpoint + perp * w._curveFactor;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            pts.Add(QuadraticBezier(a, control, b, t));
        }

        return pts;
    }


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
