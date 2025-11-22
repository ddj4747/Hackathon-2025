using UnityEngine;

public class barrierSpawner : MonoBehaviour
{

    [SerializeField] private GameObject leftBarrierPrefab;
    [SerializeField] private GameObject rightBarrierPrefab;

    public bool isLeft;

    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 10)
        {
            spawnLeftBarrier();
            timer = 0;
        }
    }
    public void spawnLeftBarrier()
    {
        Debug.Log("spawn");
        if(isLeft)
            Instantiate(leftBarrierPrefab, transform.position, transform.rotation);
        else
            Instantiate(rightBarrierPrefab, transform.position, transform.rotation);
    }
}
