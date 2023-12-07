using System.Collections;
using UnityEngine;

public class SolarSpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float initialSpawnRate = 2f; // Initial time between each obstacle spawn
    public float minSpawnRate = 0.5f; // Minimum time between each obstacle spawn
    public float spawnRateDecrease = 0.1f; // Rate at which spawn rate decreases over time
    public float obstacleSpeed = 5f;
    public Transform startPoint;
    public Transform endPoint;
    public int initialMaxSpawnCount = 5; // Initial maximum number of obstacles to spawn at once
    public int maxSpawnCountIncrease = 1; // Amount to increase max spawn count over time

    private float currentSpawnRate; // Current time between each obstacle spawn
    private int currentMaxSpawnCount; // Current maximum number of obstacles to spawn at once
    private bool isSpawning = true; // Flag to control spawning

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        currentMaxSpawnCount = initialMaxSpawnCount;
        StartCoroutine(SpawnLoop());
    }

   IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            int spawnCount = Random.Range(1, currentMaxSpawnCount + 1); // Random number of obstacles to spawn
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(currentSpawnRate); // Wait for a short duration between spawns
            }

            // Decrease spawn rate
            currentSpawnRate = Mathf.Max(currentSpawnRate - spawnRateDecrease, minSpawnRate);

            // Increase max spawn count
            currentMaxSpawnCount += maxSpawnCountIncrease;

            // Log spawn rate and max spawn count
            Debug.Log("Spawn Rate: " + currentSpawnRate + ", Max Spawn Count: " + currentMaxSpawnCount);

            yield return new WaitForSeconds(currentSpawnRate); // Wait for the next wave of spawns
        }
    }

    void SpawnObstacle()
    {
        Vector2 randomPosition = Vector2.Lerp(startPoint.position, endPoint.position, Random.value);
        GameObject obstacle = Instantiate(obstaclePrefab, randomPosition, Quaternion.identity);
        StartCoroutine(MoveObstacle(obstacle));
    }

    IEnumerator MoveObstacle(GameObject obstacle)
    {
        float distance = Vector2.Distance(startPoint.position, endPoint.position);
        float elapsedTime = 0f;

        while (elapsedTime < distance / obstacleSpeed)
        {
            if (obstacle != null)
            {
                elapsedTime += Time.deltaTime;
                float journey = elapsedTime / (distance / obstacleSpeed);
                obstacle.transform.position = Vector2.Lerp(startPoint.position, endPoint.position, journey);
                yield return null;
            }
            else
            {
                Debug.Log("Obstacle is already destroyed");
                yield break;
            }
        }

        if (obstacle != null)
        {
            Destroy(obstacle);
        }
    }

    // Method to stop spawning obstacles
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // Method to resume spawning obstacles
    public void ContinueSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
}
