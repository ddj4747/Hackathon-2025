using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 10f;
    public string _tag;

    // Maximum total rotation from initial launch direction
    public float maxRotationFromStart = 45f;

    // Rotation speed in degrees per second
    public float rotationSpeed = 120f;

    private Vector2 dir;
    private Vector2 startDir;
    private float lifeTimeTimer = 0f;
    private float damageRange = 5f;
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
        dir = transform.up;
        startDir = dir;
    }

    void FixedUpdate()
    {
        lifeTimeTimer += Time.fixedDeltaTime;

        // Desired direction toward player
        Vector2 targetDir = ((Vector2)PlayerMovement.Instance.transform.position - (Vector2)transform.position).normalized;

        // Clamp total rotation from start
        float angleFromStart = Vector2.SignedAngle(startDir, targetDir);
        if (Mathf.Abs(angleFromStart) > maxRotationFromStart)
        {
            float clampedAngle = Mathf.Sign(angleFromStart) * maxRotationFromStart;
            float startAngleRad = Mathf.Atan2(startDir.y, startDir.x);
            float newAngleRad = startAngleRad + clampedAngle * Mathf.Deg2Rad;
            targetDir = new Vector2(Mathf.Cos(newAngleRad), Mathf.Sin(newAngleRad)).normalized;
        }

        // Rotate dir gradually toward clamped targetDir
        float angleBetween = Vector2.SignedAngle(dir, targetDir);
        float maxTurnThisFrame = rotationSpeed * Time.fixedDeltaTime;
        float clampedTurn = Mathf.Clamp(angleBetween, -maxTurnThisFrame, maxTurnThisFrame);
        float newDirAngle = Mathf.Atan2(dir.y, dir.x) + clampedTurn * Mathf.Deg2Rad;
        dir = new Vector2(Mathf.Cos(newDirAngle), Mathf.Sin(newDirAngle)).normalized;

        // Move
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        // Rotate sprite
        float spriteAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.MoveRotation(spriteAngle);

        if (lifeTimeTimer > 20f)
            BlowUp();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_tag))
            BlowUp();
    }
}
