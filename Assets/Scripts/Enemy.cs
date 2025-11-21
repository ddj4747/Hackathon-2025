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
    public Path _path;

    private void MoveToWaipoint()
    {
        if (_path == null || _path._pathSegments == null || _path._pathSegments.Count == 0) return;

        float pathLength = 0f;
        for (int i = 1; i < _path._pathSegments.Count; i++)
            pathLength += Vector3.Distance(_path._pathSegments[i - 1], _path._pathSegments[i]);

        if (Mathf.Approximately(pathLength, 0f)) return;

        float duration = pathLength / _speed;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOPath(_path._pathSegments.ToArray(), duration, PathType.Linear, PathMode.TopDown2D)
            .SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                MoveToWaipoint();
            });
    }


    private void Start()
    {
        if (_path == null || _path._pathWaypoints == null || _path._pathWaypoints.Length == 0) return;

        transform.position = new Vector2(_path._pathWaypoints[0]._x, _path._pathWaypoints[0]._y);

        MoveToWaipoint();
    }


}
