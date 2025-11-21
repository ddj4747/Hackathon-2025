using DG.Tweening;
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

    private void MoveToWaipoint()
    {
        Vector2 destination = new Vector2(
            _path._pathWaypoints[_waypointIndex]._x,
            _path._pathWaypoints[_waypointIndex]._y
        );


        float distance = Vector2.Distance(transform.position, destination);
        float duration = distance / _speed;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOPath(_path._pathSegments[_waypointIndex].ToArray(), duration, PathType.Linear))
            .OnComplete(() =>
            {
                _waypointIndex++;
                if (_waypointIndex == _path._pathWaypoints.Length)
                {
                    _waypointIndex = 0;
                }
                MoveToWaipoint();
            });
    }

    private void OnEnable()
    {
        if (_path == null) return;

        transform.position = new Vector2(_path._pathWaypoints[0]._x, _path._pathWaypoints[0]._y);
        _waypointIndex++;
        if (_waypointIndex == _path._pathWaypoints.Length)
        {
            _waypointIndex = 0;
        }

        MoveToWaipoint();
    }
}
