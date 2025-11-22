using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animation;             // Reference to the Animator
    [SerializeField] private InputActionReference aPress;     // Reference for the "A" key input
    [SerializeField] private InputActionReference dPress;     // Reference for the "D" key input
    private SpriteRenderer spriteRenderer;                    // Reference to the SpriteRenderer

    void Start()
    {
        // Get the SpriteRenderer component from the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Subscribe to the "started" and "canceled" events for both keys
        aPress.action.started += APRESS;
        aPress.action.canceled += AReleased;
        dPress.action.started += DPRESS;
        dPress.action.canceled += DReleased;
    }

    void APRESS(InputAction.CallbackContext context)
    {
        // Play the turning-left animation
        _animation.SetBool("turnLeft", true);
        _animation.SetBool("turnRight", false);
        transform.localScale = new Vector3( 1 , 1 , -1);
        // Flip the sprite to face left
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }
    }

    void DPRESS(InputAction.CallbackContext context)
    {
        // Play the turning-right animation
        _animation.SetBool("turnLeft", false);
        _animation.SetBool("turnRight", true);
        transform.localScale = new Vector3(1, 1, 1);
        // Flip the sprite to face right
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = false;
        }
    }

    void AReleased(InputAction.CallbackContext context)
    {
        // Stop the turning-left animation when the key is released
        spriteRenderer.flipX = true;
        _animation.SetBool("turnLeft", false);
        transform.localScale = new Vector3(1, 1, -1);
    }

    void DReleased(InputAction.CallbackContext context)
    {
        // Stop the turning-right animation when the key is released
        _animation.SetBool("turnRight", false);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void OnDestroy()
    {
        // Unsubscribe from input actions to prevent memory leaks
        aPress.action.started -= APRESS;
        aPress.action.canceled -= AReleased;
        dPress.action.started -= DPRESS;
        dPress.action.canceled -= DReleased;
    }
}
