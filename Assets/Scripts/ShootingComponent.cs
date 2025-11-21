using UnityEngine;

public class ShootingComponent : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    public float cooldown;

    private float timerToNextShot;

    void Update()
    {
        timerToNextShot += Time.deltaTime;
    }

    public void Shoot(Vector2 direction)
    {
        if(timerToNextShot > cooldown)
        {
            timerToNextShot = 0;
            Instantiate(bulletPrefab, transform.position , transform.rotation);
        }
    }
}
