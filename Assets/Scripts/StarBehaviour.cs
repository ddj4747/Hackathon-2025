using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
    public StarsGenerator starsGenerator; // reference your main generator
    private Vector3 currentPos;
    private float moveDelay = 0.02f;
    private float currentDelay = 0f;
    public float ChosenSpeed = 0f;
    private void Start()
    {
        currentPos = transform.position;
        

    }

    void Update()
    {
        if (starsGenerator != null)
        {
            Debug.Log("Current stars: " + starsGenerator.AmountOfCurrentStars);

            // Increment the timer
            currentDelay += Time.deltaTime;

            if (currentDelay >= moveDelay)
            {
                // Reduce Y by 0.02
                currentPos.y -= ChosenSpeed;

                // Apply updated position
                transform.position = currentPos;

                // Reset timer
                currentDelay = 0f;
            }

            // Destroy when below threshold
            if (currentPos.y < -15f) // changed to negative so they fall off-screen
            {
                starsGenerator.AmountOfCurrentStars--;
                Destroy(gameObject);
            }
        }
    }
}
