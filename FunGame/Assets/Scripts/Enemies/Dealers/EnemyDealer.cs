using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDealer : MonoBehaviour
{

    public GameObject[] enemies = new GameObject[5];
    public GameObject[] playerArray = new GameObject[2];
    public List<SpawnPosPerArena> spawnAreas = new List<SpawnPosPerArena>();
    private List<EnemyBase> wave = new List<EnemyBase>();
    private int currentLevel;

    public EnemyBase GenerateEnemy()
    {
        GameObject enemy = Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)],spawnAreas[currentLevel - 7].spawnPosUnique[UnityEngine.Random.Range(0, 3)], Quaternion.identity);
        EnemyBase code = enemy.GetComponent<EnemyBase>();
        code.SetStats(playerArray[UnityEngine.Random.Range(0, 2)].transform, this);
        enemy.SetActive(false);
        return code;
    }

    public void BeginWave(int diff, int level)
    {
        playerArray[0] = GameObject.FindGameObjectWithTag("Player1");
        playerArray[1] = GameObject.FindGameObjectWithTag("Player2");

        for (int i = 0; i < diff; i++)
        {
            currentLevel = level;
            wave.Add(GenerateEnemy());
        }

        Invoke("BeginWave", 10);
    }


    private void BeginWave()
    {
        for (int i = 0; i < wave.Count; i++)
        {
            //print("Beginning wave - " + i);
            wave[i].gameObject.transform.position = spawnAreas[currentLevel - 7].spawnPosUnique[UnityEngine.Random.Range(0, 3)];
            wave[i].Beginwave();
            wave[i].gameObject.SetActive(true);
        }
        wave.Clear();
    }


    public void EnemyDeath(GameObject enemy)
    {
        enemy.SetActive(false);
        EnemyBase enemyCode = enemy.GetComponent<EnemyBase>();
        wave.Add(enemyCode);
        enemyCode.health = enemyCode.maxHealth;
    }


    [Serializable]
    public struct SpawnPosPerArena
    {
        public List<Vector3> spawnPosUnique;
    }


}
