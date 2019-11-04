using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AreaGen : BlankMono
{
    [Header("Spawning Stats")]
    public int rowsToSpawn;
    public int columnsToSpawn;
    public int numOfObjectives;
    [Space]
    public areaInfo[] areaTypes = new areaInfo[1];

    [Header("Position Info")]
    public float xIncrease;
    public float zIncrease;
     
    public void CreateZone(int areaType)
    {
        //loadingImage.SetActive(true);
        Vector3 spawnPos = Vector3.zero;

        for (int i = 0; i < rowsToSpawn; i++)
        {
            for (int c = 0; c < columnsToSpawn; c++)
            {
                Instantiate<GameObject>(areaTypes[areaType].zones[UnityEngine.Random.Range(0, areaTypes[areaType].zones.Length)], spawnPos, Quaternion.identity, gameObject.transform);
                spawnPos.x += xIncrease;
            }
            spawnPos.z += zIncrease;
            spawnPos.x = 0;
        }

        Destroy(gameObject.transform.GetChild(0).gameObject);
        GameObject playerHome = Instantiate<GameObject>(areaTypes[areaType].playerHub, Vector3.zero, Quaternion.identity);

        Vector3 finalPos = gameObject.transform.GetChild(gameObject.transform.childCount - 1).transform.position;
        Destroy(gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject);
        GameObject bossHome = Instantiate<GameObject>(areaTypes[areaType].bossRoom, finalPos, Quaternion.identity);

        for (int i = 0; i < numOfObjectives; i++)
        {
            int z = UnityEngine.Random.Range(0, gameObject.transform.childCount - 1);
            Vector3 newPos = gameObject.transform.GetChild(z).transform.position;
            Destroy(gameObject.transform.GetChild(z).gameObject);
            Instantiate<GameObject>(areaTypes[areaType].objectiveRoom, newPos, Quaternion.identity);
        }
        
        NavMeshPath path = new NavMeshPath();

        //print(NavMesh.CalculatePath(playerHome.transform.position, bossHome.transform.position, 1, path));
        if (!NavMesh.CalculatePath(playerHome.transform.position, bossHome.transform.position, 1, path))
        {
            DestroyZones();
            CreateZone(areaType);
        }

    }

    public void DestroyZones()
    {
        GameObject[] zones = GameObject.FindGameObjectsWithTag("Zone");
        for (int i = 0; i < zones.Length; i++)
        {
            Destroy(zones[i]);
        }
    }


    [Serializable]
    public struct areaInfo
    {
        public GameObject[] zones;
        public GameObject playerHub;
        public GameObject bossRoom;
        public GameObject objectiveRoom;
    }

}
