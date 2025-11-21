using System;
using UnityEngine;

public class StarsGenerator : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // seconds between spawns
    private float timer = 0f;
    private float baseSpeed = 0.001f;
    private float addedSpeed = 0.0005f;
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
            float randomY = UnityEngine.Random.Range(-7f, 7f);
            float randomX = UnityEngine.Random.Range(-10f, 10f);

            GameObject starObj = Instantiate(chosen.prefab, new Vector3(randomX, randomY, 0), Quaternion.identity);

            // Assign StarsGenerator reference to the spawned prefab
            StarBehaviour sb = starObj.GetComponent<StarBehaviour>();
            if (sb != null)
            {
                sb.starsGenerator = this;
                sb.ChosenSpeed = baseSpeed + addedSpeed * starIndex; // bigger index → faster
            }
        }
    }

    void SpawnStar()
    {
        Star chosen = PickStar(out int starIndex);
        if (chosen.prefab != null)
        {
            float randomX = UnityEngine.Random.Range(-10f, 10f);

            GameObject starObj = Instantiate(chosen.prefab, new Vector3(randomX, 6f, 0), Quaternion.identity);

            // Assign StarsGenerator reference to the spawned prefab
            StarBehaviour sb = starObj.GetComponent<StarBehaviour>();
            if (sb != null)
            {
                sb.starsGenerator = this;
                sb.ChosenSpeed = baseSpeed + addedSpeed * starIndex; // bigger index → faster
            }
        }
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
