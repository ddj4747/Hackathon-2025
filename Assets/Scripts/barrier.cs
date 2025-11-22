using UnityEngine;

public class barrier : MonoBehaviour
{
    public float speed = 2.0f;
    void Start()
    {
        
    }

    bool HasEnemySibling()
    {
        Transform parent = transform.parent;

        if (parent == null)
            return false;   // no parent = no siblings

        foreach (Transform child in parent)
        {
            if (child == transform)
                continue;   // skip yourself

            if (child.CompareTag("enemy"))
                return true; // found an enemy sibling
        }

        return false; // none found
    }

    void Update()
    {
        if(!HasEnemySibling())
        {
            Debug.Log("nie ma enemy siblinga");
            Destroy(gameObject);
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime); 
    }
}
