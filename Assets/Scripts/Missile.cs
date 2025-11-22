using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float damage;

    private Vector2 dir = new();
    private float lifeTimeTimer = 0;
    private float damageRange = 2;

    private Rigidbody2D rb;

    private void BlowUp()
    {
        if (Vector3.Distance(PlayerMovement.Instance.transform.position, transform.position) < damageRange)
        {
            PlayerMovement.Instance.HealthComponent.TakeDamage(damage);
        }

        Destroy(gameObject);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = transform.rotation * Vector2.up;
    }

    void FixedUpdate()
    {
        lifeTimeTimer += Time.fixedDeltaTime;

        Vector3 targetDir = PlayerMovement.Instance.transform.position - transform.position;
        Vector2 moveDir = targetDir.normalized;
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);

        if (lifeTimeTimer > 20)
        {
            BlowUp();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == tag)
        {
            BlowUp();
        }
    }
}
