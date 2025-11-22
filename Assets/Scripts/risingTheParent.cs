using UnityEngine;

public class risingTheParent : MonoBehaviour
{
    public float riseSpeed = 2f;

    void Update()
    {
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }
}

