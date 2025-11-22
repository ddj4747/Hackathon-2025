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
        // 1. Get simple direction
        Vector3 direction = PlayerMovement.Instance.transform.position - _parent.transform.position;

        // 2. Calculate angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 3. Apply rotation
        // NOTE: If your sprite faces UP, add -90 to the angle: (angle - 90)
        transform.rotation = Quaternion.LookRotation(_parent.transform.position, direction);
    }

    private void Start()
    {
        StartCoroutine(AttackLoop());
    }
}
