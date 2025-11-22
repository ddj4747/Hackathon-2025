using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public Enemy _parent;

    public void Attack()
    {
        Transform targetTransform = PlayerMovement.Instance.transform;

        Vector2 dir = (Vector2)targetTransform.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(_parent._bullet, transform.position - new Vector3(0.2f, 0.7f), Quaternion.Euler(0f, 0f, angle-90));
        Instantiate(_parent._bullet, transform.position - new Vector3(-0.2f, 0.7f), Quaternion.Euler(0f, 0f, angle - 90));

    }

    private void Start()
    {
        StartCoroutine(AtttackLoop());
    }

    private IEnumerator AtttackLoop()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(_parent._attackSpeed);
        }
    }
}
