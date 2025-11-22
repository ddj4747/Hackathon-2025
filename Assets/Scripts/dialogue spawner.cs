using UnityEngine;
using TMPro;

public class DialogueSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePrefab; 
    [SerializeField] private Transform spawnPosition;   
    
    private GameObject currentDialogue; 

    void Start()
    {
        SpawnDialogue();


    }

    public void SpawnDialogue()
    {
        if (currentDialogue != null)
        {
            
            Destroy(currentDialogue);
        }        
        currentDialogue = Instantiate(dialoguePrefab, spawnPosition.position, Quaternion.identity);

        TextMeshProUGUI dialogueText = currentDialogue.GetComponentInChildren<TextMeshProUGUI>();

                
    }
}
