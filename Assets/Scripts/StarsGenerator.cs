using System;
using UnityEngine;
using System.Collections.Generic;

public class StarsGenerator : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // seconds between spawns
    private float timer = 0f;
    [SerializeField] private float baseSpeed = 0.002f;
    [SerializeField] private float addedSpeed = 0.001f;
    [SerializeField] private int maxAmountOfStars = 15;
    [SerializeField] private int amountOfCurrentStars = 0;

    // Public property to access current amount of stars
    public int AmountOfCurrentStars
    {
        get { return amountOfCurrentStars; }
        set { amountOfCurrentStars = Mathf.Max(0, value); } // prevent negative values
    }

    public int MaxAmountOfStars
    {
        get { return maxAmountOfStars; }
    }

    [Serializable]
    public class Star
    {
        public string name;      // Name of the star
        public int weight;       // Weight for random selection
        public GameObject prefab; // Prefab of the star
    }

    public Star[] stars;

    // To track the positions of planets 8 and 9 only
    private List<Vector3> occupiedPositions8And9 = new List<Vector3>();

    void Start()
    {
        for (int i = 0; i < maxAmountOfStars; i++)
        {
            SpawnStarOnStart();
            amountOfCurrentStars++;
        }
    }

    void Update()
    {
        // Spawn new stars only if we haven't reached max
        if (amountOfCurrentStars < maxAmountOfStars)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                timer = 0f;
                SpawnStar();
                amountOfCurrentStars++;
            }
        }
    }

    void SpawnStarOnStart()
    {
        Star chosen = PickStar(out int starIndex);
        if (chosen.prefab != null)
        {
            float randomY = UnityEngine.Random.Range(-12f, 18f);
            float randomX = UnityEngine.Random.Range(-20f, 20f);

            // Only check for overlap if this is a planet 8 or 9
            if (starIndex == 8 || starIndex == 9)
            {
                // Check if the position is occupied by another 8 or 9
                if (IsPositionOccupiedFor8And9(new Vector3(randomX, randomY, 0)))
                {
                    return; // Don't spawn if position is occupied
                }
            }

            GameObject starObj = Instantiate(chosen.prefab, new Vector3(randomX, randomY, 0), Quaternion.identity);

            // Assign StarsGenerator reference to the spawned prefab
            StarBehaviour sb = starObj.GetComponent<StarBehaviour>();
            if (sb != null)
            {
                sb.starsGenerator = this;
                sb.ChosenSpeed = baseSpeed + addedSpeed * starIndex; // bigger index → faster
            }

            // Add the new position to the list of occupied positions for 8 or 9 planets
            if (starIndex == 8 || starIndex == 9)
            {
                occupiedPositions8And9.Add(new Vector3(randomX, randomY, 0));
            }
        }
    }

    void SpawnStar()
    {
        Star chosen = PickStar(out int starIndex);
        if (chosen.prefab != null)
        {
            float randomX = UnityEngine.Random.Range(-18f, 18f);
            
            float randomY = UnityEngine.Random.Range(-22f, 21f); // Top of the screen

            // Only check for overlap if this is a planet 8 or 9
            if (starIndex == 8 || starIndex == 9)
            {
                // Check if the position is occupied by another 8 or 9
                if (IsPositionOccupiedFor8And9(new Vector3(randomX, randomY, 0)))
                {
                    return; // Don't spawn if position is occupied
                }
            }

            GameObject starObj = Instantiate(chosen.prefab, new Vector3(randomX, randomY, 0), Quaternion.identity);

            // Assign StarsGenerator reference to the spawned prefab
            StarBehaviour sb = starObj.GetComponent<StarBehaviour>();
            if (sb != null)
            {
                sb.starsGenerator = this;
                sb.ChosenSpeed = baseSpeed + addedSpeed * starIndex; // bigger index → faster
            }

            // Add the new position to the list of occupied positions for 8 or 9 planets
            if (starIndex == 8 || starIndex == 9)
            {
                occupiedPositions8And9.Add(new Vector3(randomX, randomY, 0));
            }
        }
    }

    // Check if a position is occupied by another planet with index 8 or 9
    bool IsPositionOccupiedFor8And9(Vector3 position)
    {
        const float minDistance = 5f; // Minimum distance to other stars to avoid overlap
        foreach (Vector3 occupiedPos in occupiedPositions8And9)
        {
            if (Vector3.Distance(occupiedPos, position) < minDistance)
            {
                return true; // Position is too close to another planet 8 or 9
            }
        }
        return false;
    }

    // Modified PickStar to also return the index of the chosen star
    Star PickStar(out int starIndex)
    {
        int totalWeight = 0;
        for (int i = 0; i < stars.Length; i++)
            totalWeight += stars[i].weight;

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        for (int i = 0; i < stars.Length; i++)
        {
            cumulative += stars[i].weight;
            if (randomValue < cumulative)
            {
                starIndex = i;
                return stars[i];
            }
        }

        starIndex = stars.Length - 1;
        return stars[stars.Length - 1]; // fallback
    }
}
