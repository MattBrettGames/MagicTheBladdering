using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGen : BlankMono
{
    [Header("Spawning Stats")]
    public int rowsToSpawn;
    public int columnsToSpawn;
    public GameObject[] zones = new GameObject[2];
    public GameObject playerHub;
    public GameObject bossRoom;
    public GameObject objectiveRoom;
    public int numOfObjectives;

    [Header("Position Info")]
    public float xIncrease;
    public float zIncrease;

    [Header("Smoke & Mirrors")]
    public GameObject loadingImage;

    void Start()
    {
        CreateZone();
    }

    public void CreateZone()
    {
        loadingImage.SetActive(true);
        Vector3 spawnPos = Vector3.zero;

        for (int i = 0; i < rowsToSpawn; i++)
        {
            for (int c = 0; c < columnsToSpawn; c++)
            {
                Instantiate<GameObject>(zones[Random.Range(0, zones.Length)], spawnPos, Quaternion.identity, gameObject.transform);
                spawnPos.x += xIncrease;
            }
            spawnPos.z += zIncrease;
            spawnPos.x = 0;
        }

        Destroy(gameObject.transform.GetChild(0).gameObject);
        Instantiate<GameObject>(playerHub, Vector3.zero, Quaternion.identity);

        Vector3 finalPos = gameObject.transform.GetChild(gameObject.transform.childCount-1).transform.position; 
        Destroy(gameObject.transform.GetChild(gameObject.transform.childCount-1).gameObject);
        Instantiate<GameObject>(bossRoom, finalPos, Quaternion.identity);
                
        for (int i = 0; i< numOfObjectives; i++)
        {
            int z = Random.Range(0, gameObject.transform.childCount-1);
            Vector3 newPos = gameObject.transform.GetChild(z).transform.position;
            Destroy(gameObject.transform.GetChild(z).gameObject);
            Instantiate<GameObject>(objectiveRoom, newPos, Quaternion.identity);
        }


        loadingImage.SetActive(false);
    }

}
