using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObstacleManager : MonoBehaviour
{
   public GameObject obstaclePrefab;
    public GameObject obstaclePrefab2;
    public int poolSize = 10;
    public float spawnRate = 2f; // Time between each obstacle spawn
    public float obstacleSpeed = 5f;

    private List<GameObject> pooledObstacles = new List<GameObject>();
    private Camera mainCamera;
    private bool gameActive = true;
    public bool isSpawning = true; // Flag to control obstacle spawning

    public GameObject StartPoint;
    public GameObject EndPoint;

    private float initialSpawnRate; // Store the initial spawn rate


    void Start()
    {
        mainCamera = Camera.main;
        initialSpawnRate = spawnRate; // Store the initial spawn rate

        // Creating and initializing the pool of obstacles
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.SetActive(false);
            pooledObstacles.Add(obstacle);
        }

        // Start spawning obstacles
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnRate);
        StartCoroutine(IncreaseSpawnRate());
    }


    IEnumerator IncreaseSpawnRate()
    {
        bool increasing = true; // Flag to track if the spawn rate is increasing or decreasing

        while (true)
        {
            yield return new WaitForSeconds(10f); // Wait for 10 seconds

            if (increasing)
            {
                spawnRate -= 1.0f; // Increase spawn rate by 1
            }

            if (spawnRate <= 2)
            {
                spawnRate = 5;
            }

            Debug.Log("Spawn rate: " + spawnRate);

            // Restart the spawn loop with the new spawn rate
            if (isSpawning)
            {
                CancelInvoke(nameof(SpawnObstacle)); // Cancel the existing InvokeRepeating
                InvokeRepeating(nameof(SpawnObstacle), 0f, spawnRate);
            }
        }
    }

    void Update()
    {
        if (!gameActive)
        {
            // Logic for when an obstacle is hit (e.g., stop camera movement)
            mainCamera.transform.Translate(Vector3.zero); // Stop camera movement or perform other game over actions.
            StopObstacleSpawning(); // Stop spawning new obstacles when game is not active
        }
    }
 void SpawnObstacle()
    {
        if (!isSpawning) return; // Check if spawning is allowed

        GameObject obstacle = GetPooledObstacle();
        if (obstacle != null)
        {
            // Randomly select between obstaclePrefab and obstaclePrefab2
            GameObject selectedPrefab = Random.Range(0, 2) == 0 ? obstaclePrefab : obstaclePrefab2;

            obstacle = Instantiate(selectedPrefab); // Instantiate the selected prefab
            obstacle.transform.position = StartPoint.transform.position; // Set spawn position

            obstacle.SetActive(true);

            // Move the obstacle from start to end
            StartCoroutine(MoveObstacle(obstacle));
        }
    }


    IEnumerator MoveObstacle(GameObject obstacle)
    {
        float elapsedTime = 0f;
        Vector2 startPos = StartPoint.transform.position;
        Vector2 endPos = EndPoint.transform.position;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * (1 / obstacleSpeed);
            obstacle.transform.position = Vector2.Lerp(startPos, endPos, elapsedTime);
            yield return null;
        }

        // Deactivate the obstacle when it reaches the end
        obstacle.SetActive(false);
    }

    GameObject GetPooledObstacle()
    {
        // Retrieve an inactive obstacle from the pool
        for (int i = 0; i < pooledObstacles.Count; i++)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }
        }

        // If no inactive obstacles are available, expand the pool by creating a new obstacle
        GameObject obstacle = Instantiate(obstaclePrefab);
        obstacle.SetActive(false);
        pooledObstacles.Add(obstacle);
        return obstacle;
    }

    // Method to stop spawning obstacles
    public void StopObstacleSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnObstacle)); // Stop spawning new obstacles
    }

    // Method to continue spawning obstacles
    public void ContinueObstacleSpawning()
    {
        isSpawning = true;
        // Resume spawning obstacles with the current spawn rate
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnRate);
    }

    // Method to call when an obstacle hits the player (you should trigger this from your player collision logic)
    public void ObstacleHit()
    {
        gameActive = false;
        Debug.Log("Obstacle hit!");
    }
}
