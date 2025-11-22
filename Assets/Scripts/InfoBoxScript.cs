using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoBoxScript : MonoBehaviour
{
    [SerializeField] private bool printText = true;
    [SerializeField] private float delayBetweenLetters = 0.02f;
    [SerializeField] private InputActionReference spaceBar;
    [SerializeField] private List<string> textList = new List<string>();  // List of strings to print
    public bool spawningEnemies;  // This is now a public variable, accessible by other scripts

    private float currentDelayCounter = 0f;
    private int currentTextLength = 0;
    private int currentStringIndex = 0;  // Keeps track of which string in the list is being printed
    private TextMeshProUGUI tmpText;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>(); // Assign TextMeshProUGUI component correctly
        spaceBar.action.started += onSpace;  // Subscribe to spacebar press event
    }

    void onSpace(InputAction.CallbackContext context)
    {
        // When spacebar is pressed, move to the next string in the list
        if (currentStringIndex < textList.Count - 1)  // Check if it's not the last string
        {
            currentStringIndex++;  // Go to the next string
            currentTextLength = 0;  // Reset the character counter for the next string
            tmpText.text = "";  // Clear the previous text
        }
        else
        {
            // On the last string, destroy the parent and the Canvas
            spaceBar.action.started -= onSpace;
            spawningEnemies = true;
            Destroy(tmpText.gameObject.transform.parent.parent.gameObject);  // Destroys the entire Canvas + Text
        }
    }

    void Update()
    {
        if (printText && currentStringIndex < textList.Count && tmpText.text.Length < textList[currentStringIndex].Length)
        {
            currentDelayCounter += Time.deltaTime;
            if (currentDelayCounter > delayBetweenLetters)
            {
                tmpText.text += textList[currentStringIndex][currentTextLength]; // Append one letter at a time
                currentTextLength++;

                // Ensure the last character gets added even if the loop finishes
                if (currentTextLength >= textList[currentStringIndex].Length)
                {
                    tmpText.text = textList[currentStringIndex]; // Forcefully complete the text on the last string
                }

                currentDelayCounter = 0f; // Reset the delay counter
            }
        }
    }
}
