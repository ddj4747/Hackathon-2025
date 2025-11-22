using UnityEngine;

public class barrier : MonoBehaviour
{
    public float speed = 2.0f;
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime); 
    }
}
