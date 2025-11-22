using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Enemy _parent;

    [SerializeField] private Transform _firePoint;

    private void Start()
    {
        if (_firePoint == null)
        {
            _firePoint = transform;
        }
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(_parent._attackSpeed);
        }
    }

    private void Attack()
    {
        Instantiate(_parent._bullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, 90));
        Instantiate(_parent._bullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
    }
}