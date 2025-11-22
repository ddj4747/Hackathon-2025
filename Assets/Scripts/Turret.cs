using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Enemy _parent;

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
        
    }

    private void FixedUpdate()
    {
        //Vector2 direction = PlayerMovement.Instance.transform.position - _parent.transform.position - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, _parent.transform.rotation.z + angle);

        transform.rotation = Quaternion.FromToRotation(transform.position, PlayerMovement.Instance.transform.position);
    }

    private void Start()
    {
        StartCoroutine(AttackLoop());
    }
}
