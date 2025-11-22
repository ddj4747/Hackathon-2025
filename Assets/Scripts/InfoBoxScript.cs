using TMPro;
using UnityEngine;

public class InfoBoxScript : MonoBehaviour
{
    [SerializeField] private bool printText = false;
    [SerializeField] private string text = "";
    [SerializeField] private float delayBetweenLetters = 0.02f;
    private float currentDelayCounter = 0f;
    private int currentTextLength = 0;
    private TextMeshProUGUI tmpText;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>(); // Assign TextMeshProUGUI component correctly
    }

    void Update()
    {
        if (printText && tmpText.text.Length < text.Length)
        {
            currentDelayCounter += Time.deltaTime;
            if (currentDelayCounter > delayBetweenLetters)
            {
                tmpText.text += text[currentTextLength]; // Append one letter at a time
                currentTextLength++;
                currentDelayCounter = 0f; // Reset the delay counter
            }
        }
    }
}
