using UnityEngine;
using System.Collections;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] objectsToSpawn;  // Array of prefabs to spawn
    public Transform spawnPoint;         // Optional specific spawn location
    public float minSpawnInterval = 2f;  // Minimum time between spawns
    public float maxSpawnInterval = 8f;  // Maximum time between spawns

    [Header("Spawn Options")]
    public bool randomizePosition = false;  // Random position within range?
    public Vector3 spawnArea = new Vector3(3f, 0f, 3f); // XZ area around spawner

    private void Start()
    {
        // Start the random spawn loop
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Wait a random amount of time between 2–8 seconds
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject()
    {
        if (objectsToSpawn.Length == 0)
        {
            return;
        }

        // Choose a random prefab
        GameObject prefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // Determine spawn position
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

        if (randomizePosition)
        {
            spawnPos += new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                Random.Range(-spawnArea.z, spawnArea.z)
            );
        }

        // Spawn the object
        Instantiate(prefab, spawnPos, Quaternion.identity);
        Debug.Log($"[RandomObjectSpawner] Spawned: {prefab.name} at {spawnPos}");
    }

    private void OnDrawGizmosSelected()
    {
        if (randomizePosition)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, spawnArea * 2);
        }
    }
}