using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private InputActionReference pressSpace;

    public int SceneIndex = 1;
    private void pressedSpace(InputAction.CallbackContext context)
    {
        
        SceneManager.LoadScene(SceneIndex , LoadSceneMode.Single);
    }

   
    void Start()
    {
        
        pressSpace.action.started += pressedSpace;
    }

    
    void Update()
    {
        
    }

    
    private void OnDestroy()
    {
        pressSpace.action.started -= pressedSpace;
    }
}
