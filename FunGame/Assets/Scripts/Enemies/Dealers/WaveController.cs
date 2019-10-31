using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{

    public EnemyDealer dealer;
    public int waveGap;
    public int diffScaling;
    public int difficulty;
    public bool isSpawning;
    private int currentLevel;

    public void BeginWaves(int level)
    {
        currentLevel = level;
        dealer.BeginWave(difficulty,level);
        StartCoroutine(TriggerWave());
    }

    IEnumerator TriggerWave()
    {
        if (!isSpawning)
        {
            yield return new WaitForSeconds(waveGap);
            dealer.BeginWave(difficulty, currentLevel);
            difficulty += diffScaling;

            StartCoroutine(TriggerWave());
        }
    }
}