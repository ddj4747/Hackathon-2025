using DG.Tweening;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(HealthComponent), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public HealthComponent HealthComponent { get; private set; }

    [Header("Stats")]
    public float _damage;
    public float _attackSpeed;
    public float _speed;
    public Bullet _bullet;
    public Vector3 _attackOffset;
    [SerializeField] private bool _passive;

    public Material _deathMaterial;
    public Material _normalMaterial;

    private Material _deathMaterialCopy;
    private Material _normalMaterialCopy;

    private Rigidbody2D rb;
    private SpriteRenderer sp;

    [Header("Info")]
    public Path _path;

    private Sequence _moveSeq;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        HealthComponent = GetComponent<HealthComponent>();
        sp = GetComponent<SpriteRenderer>();

        _deathMaterialCopy = new Material(_deathMaterial);
        _normalMaterialCopy = new Material(_normalMaterial);
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            Instantiate(_bullet, _attackOffset + transform.position, transform.rotation);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    private Vector3 _lastPosition;
    

    private void MoveToWaipoint()
    {
        if (_path == null || _path._pathSegments == null || _path._pathSegments.Count == 0) return;

        float pathLength = 0f;
        for (int i = 1; i < _path._pathSegments.Count; i++)
            pathLength += Vector3.Distance(_path._pathSegments[i - 1], _path._pathSegments[i]);

        if (Mathf.Approximately(pathLength, 0f)) return;

        float duration = pathLength / _speed;

        // 1. Reset last position before starting
        _lastPosition = transform.position;

        _moveSeq?.Kill();
        _moveSeq = DOTween.Sequence();

        // 2. Change PathMode to Sidescroller2D or Ignore (Standard 2D)
        _moveSeq.Append(transform.DOPath(_path._pathSegments.ToArray(), duration, PathType.Linear, PathMode.Sidescroller2D)
            .SetEase(Ease.Linear)
            .SetId(this))
            .OnComplete(() =>
            {
                if (_path.loop) MoveToWaipoint();
                else OnDeath();
            });
    }


    private Vector3 last = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 v = (transform.position - last) / Time.deltaTime;
        last = transform.position;

        if (v.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90;
            float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, 120 * Time.fixedDeltaTime);
            rb.rotation = newAngle;

        }
    }

    private void Start()
    {
        if (_path == null || _path._pathWaypoints == null || _path._pathWaypoints.Length == 0) return;

        transform.position = new Vector2(_path._pathWaypoints[0]._x, _path._pathWaypoints[0]._y);

        _moveSeq = DOTween.Sequence();

        PathManager.EnemyLeft++;

        MoveToWaipoint();
        if (!_passive) 
        { 
            StartCoroutine(AttackLoop());
        }
    }


    private bool isDead = false;
    public void OnDeath()
    {
        enemyParticleSystem.playDeathParticles(transform.position);

        _moveSeq?.OnComplete(null);
        _moveSeq?.Kill();
        StopAllCoroutines();
        Destroy(gameObject);
        isDead = true;
        PathManager.EnemyLeft--;
    }

    Coroutine damageCoruntine = null;
    private IEnumerator DamageShader()
    {
        List<Material> mt = new List<Material>();
        mt.Add(_deathMaterialCopy);
        sp.SetMaterials(mt);

        yield return new WaitForSeconds(0.2f);

        mt.Clear();
        mt.Add(_normalMaterialCopy);
        sp.SetMaterials(mt);
    }

    public void OnHealthChange(float ss)
    {
        if (isDead)
        {
            return;
        }

        if (damageCoruntine != null)
        {
            StopCoroutine(damageCoruntine);
        }

        damageCoruntine = StartCoroutine(DamageShader());

        enemyParticleSystem.playDamageParticles(transform.position, Quaternion.Euler(transform.up));
    }
}
