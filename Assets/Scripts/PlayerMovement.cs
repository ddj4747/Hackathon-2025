using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HealthComponent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private float _movementSpeed = 45f;
    [SerializeField] private ShootingComponent shootingComponent1;
    [SerializeField] private ShootingComponent shootingComponent2;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody2D;
    private bool wasLastShotLeft = false;


    public HealthComponent HealthComponent { get; private set; }
    public static PlayerMovement Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        HealthComponent = GetComponent<HealthComponent>();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        bool currentlyMoving = _moveInput.magnitude > 0.1f;

    }

    private void onShoot()
    {
            shootingComponent2.Shoot(transform.up);
            shootingComponent1.Shoot(transform.up);

    }
   
    void Start()
    {
        moveAction.action.Enable();
        moveAction.action.started += OnMove;
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootAction.action.ReadValue<float>() > 0f)
        {
            onShoot();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearDamping = 5f; // większe tłumienie dla płynnego zatrzymania

        Vector2 movement = _moveInput * _movementSpeed;
        _rigidbody2D.AddForce(movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            HealthComponent.TakeDamage(1);
        }
    }
}
