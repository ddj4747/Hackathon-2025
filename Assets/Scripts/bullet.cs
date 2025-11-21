using UnityEngine;
using UnityEngine.UIElements;

public class bullet : MonoBehaviour
{
    public float speed;
    public float damage;

    private Vector2 dir = new();
    private float lifeTimeTimer = 0;

    void Start()
    {
        dir = transform.rotation * Vector2.up;
    }

    void Update()
    {
        lifeTimeTimer += Time.deltaTime;
        transform.position += new Vector3(dir.x * speed * Time.deltaTime, dir.y * speed * Time.deltaTime);

        if (lifeTimeTimer > 5)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(damage);
        }
    }


}
