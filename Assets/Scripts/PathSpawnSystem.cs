using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PathManager : MonoBehaviour
{
    [Header("Available Paths")]
    // Create an array or list of paths (Path objects are references to your Path Scriptable Objects or prefabs)
    public Path[] availablePaths;
    [SerializeField] private float minRange = 10;
    [SerializeField] private float maxRange = 20;
    private float timeSinceLastSpawn = 100f;
    private float randomDelay = 0f;
    public InfoBoxScript InfoBoxScript;

    public Enemy _finalBoss;
    public Transform _start;
    public Transform _end;

    public static int WaveCounter = 1;

    public static int EnemyLeft = 0;

    void Start()
    {
        // Initialize with a random delay to wait before spawning the first path
        SetRandomDelay();
        WaveCounter = 1;
        
    }

    private IEnumerator BossEntrance()
    {
        Enemy enemy = Instantiate(_finalBoss, _start);
        yield return enemy.transform
            .DOMove(_end.position, 2f)
            .SetEase(Ease.InOutSine)
            .WaitForCompletion();

    }

    void Update()
    {
        if (InfoBoxScript.spawningEnemies)
        {

            // Accumulate time
            timeSinceLastSpawn += Time.deltaTime;

            // If enough time has passed, instantiate a new path
            if (timeSinceLastSpawn >= randomDelay)
            {
                if (EnemyLeft != 0)
                {
                    return;
                }

                // Pick a random path and instantiate it
                Path chosenPath = GetRandomPath();
                if (chosenPath != null)
                {
                    if (WaveCounter % 6 == 0)
                    {
                        StartCoroutine(BossEntrance());
                        EnemyLeft++;
                        randomDelay = 30;
                        WaveCounter++;
                    }
                    else
                    {
                        Instantiate(chosenPath.gameObject, Vector3.zero, Quaternion.identity);
                        WaveCounter++;
                    }
                }

                // Reset the time and set a new random delay
                
                timeSinceLastSpawn = 0f;
                SetRandomDelay();
            }
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
        randomDelay = Random.Range(minRange, maxRange); // Set random delay between 2 and 10 seconds
    }
}
