using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Animator _animation;
    [SerializeField] private InputActionReference aPress;
    [SerializeField] private InputActionReference dPress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void APRESS(InputAction.CallbackContext context)
    {
        _animation.SetBool("turnLeft", true);
        _animation.SetBool("turnRight", false);
    }
    void DPRESS(InputAction.CallbackContext context)
    {
        _animation.SetBool("turnLeft", false);
        _animation.SetBool("turnRight", true);
    }



    void Start()
    {
        aPress.action.performed += APRESS;
        dPress.action.performed += DPRESS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
