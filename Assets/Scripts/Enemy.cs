using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(HealthComponent), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public HealthComponent HealthComponent { get; private set; }

    [Header("Stats")]
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _speed;

    [Header("Info")]
    [SerializeField] private Path _path;
    [SerializeField] private int _waypointIndex;

    // inside Enemy.cs

    private void MoveToWaipoint()
    {
        if (_path == null || _path._pathSegments == null || _path._pathSegments.Count == 0) return;

        // Clamp index
        _waypointIndex = Mathf.Clamp(_waypointIndex, 0, _path._pathSegments.Count - 1);

        List<Vector3> segment = _path._pathSegments[_waypointIndex];
        if (segment == null || segment.Count < 2) return;

        // Calculate accurate path length (sum of distances between consecutive sample points)
        float pathLength = 0f;
        for (int i = 1; i < segment.Count; i++)
            pathLength += Vector3.Distance(segment[i - 1], segment[i]);

        // Avoid zero-length
        if (Mathf.Approximately(pathLength, 0f)) return;

        float duration = pathLength / _speed;

        // Use DOPath with the sampled points. PathType.Linear is fine because the points already sample the curve.
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOPath(segment.ToArray(), duration, PathType.Linear))
           .OnComplete(() =>
           {
               // advance to next segment
               _waypointIndex++;
               if (_waypointIndex >= _path._pathSegments.Count)
                   _waypointIndex = 0;

               MoveToWaipoint();
           });
    }

    private void Start()
    {
        if (_path == null || _path._pathWaypoints == null || _path._pathWaypoints.Length == 0) return;

        // start at first waypoint
        transform.position = new Vector2(_path._pathWaypoints[0]._x, _path._pathWaypoints[0]._y);

        // start on segment 0 (which is waypoint 0 -> waypoint 1)
        _waypointIndex = 0;

        MoveToWaipoint();
    }


}
