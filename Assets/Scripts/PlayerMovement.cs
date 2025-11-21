using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float _movementSpeed = 45f;
    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody2D;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        bool currentlyMoving = _moveInput.magnitude > 0.1f;

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
        
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearDamping = 5f; // większe tłumienie dla płynnego zatrzymania

        Vector2 movement = _moveInput * _movementSpeed;
        _rigidbody2D.AddForce(movement);
    }
}
