using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject spawnParent;
    public GameObject panel;
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;
    public int maxTotalObjectsToSpawn;

    private int currentTotalObjects = 0;
    public int remainingObjects;

    void Start()
    {
        SpawnObjects();
        remainingObjects = currentTotalObjects;
    }

    void SpawnObjects()
    {
        while (currentTotalObjects < maxTotalObjectsToSpawn)
        {
            int randomIndex = Random.Range(0, objectsToSpawn.Length);

            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
                Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2),
                Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
            );

            GameObject spawnedObject = Instantiate(objectsToSpawn[randomIndex], randomPosition, Quaternion.identity);
            spawnedObject.transform.parent = spawnParent.transform;
            currentTotalObjects++;
        }
    }

    public void ObjectDestroyed()
    {
        remainingObjects--;

        if (remainingObjects <= 0)
        {
            // All objects are destroyed, trigger win condition
            panel.SetActive(true);
            Debug.Log("You Win!");
        }
    }
}
