using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public Enemy _parent;

    public void Attack()
    {
        Transform targetTransform = PlayerMovement.Instance.transform;
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
