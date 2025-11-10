using UnityEngine;
using System.Collections;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] objectsToSpawn;
    public Transform spawnPoint;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 8f;

    [Header("Spawn Options")]
    public bool randomizePosition = false;  //random position within range
    public Vector3 spawnArea = new Vector3(3f, 0f, 3f); //XZ area around spawner

    [Header("Hierarchy Organization")]
    public string parentName = "ObjSpawned";  //name of the parent object
    private Transform parentContainer;

    private void Start()
    {
        //find or create the parent container
        GameObject parentObj = GameObject.Find(parentName);
        if (parentObj == null)
        {
            parentObj = new GameObject(parentName);
        }
        parentContainer = parentObj.transform;

        //start the random spawn loop
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            //wait a random amount of time between 2–8 seconds
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

        //choose a random prefab
        GameObject prefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        //determine spawn position
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

        if (randomizePosition)
        {
            spawnPos += new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                Random.Range(-spawnArea.z, spawnArea.z)
            );
        }

        //spawn the object
        //Instantiate(prefab, spawnPos, Quaternion.identity);

        //spawn the object and parent it under the container
        GameObject spawnedObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObj.transform.SetParent(parentContainer);
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