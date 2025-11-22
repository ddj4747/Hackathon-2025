using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("Available Paths")]
    // Create an array or list of paths (Path objects are references to your Path Scriptable Objects or prefabs)
    public Path[] availablePaths;

    private float timeSinceLastSpawn = 0f;
    private float randomDelay = 0f;
  
    void Start()
    {
        // Initialize with a random delay to wait before spawning the first path
        SetRandomDelay();
    }

    void Update()
    {
        // Accumulate time
        timeSinceLastSpawn += Time.deltaTime;
        
        // If enough time has passed, instantiate a new path
        if (timeSinceLastSpawn >= randomDelay)
        {
            // Pick a random path and instantiate it
            Path chosenPath = GetRandomPath();
            if (chosenPath != null)
            {
                // Instantiate the Path prefab at the origin (or modify as necessary)
                Instantiate(chosenPath.gameObject, Vector3.zero, Quaternion.identity); 
            }

            // Reset the time and set a new random delay
            timeSinceLastSpawn = 0f;
            SetRandomDelay();
        }
        
    }

    // Method to randomly pick a path
    public Path GetRandomPath()
    {
        if (availablePaths.Length == 0)
        {
            Debug.LogError("No paths available to choose from!");
            return null;
        }

        // Pick a random index in the array
        int randomIndex = Random.Range(0, availablePaths.Length);
        return availablePaths[randomIndex];
    }

    // Method to set a random delay between path spawns
    private void SetRandomDelay()
    {
        randomDelay = Random.Range(2f, 10f); // Set random delay between 2 and 10 seconds
    }
}
