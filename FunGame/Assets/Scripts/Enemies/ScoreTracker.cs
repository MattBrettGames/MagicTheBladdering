using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreTracker : MonoBehaviour
{
    public int[] enemyValues = new int[1];
    private int p1Score;
    private int p2Score;

    public void EnemyDeath(string player, int enemyID)
    {
        if (player == "Player1")
        {
            p1Score += enemyValues[enemyID];
        }
        if (player == "Player2")
        {
            p2Score += enemyValues[enemyID];
        }
        if (player == "both")
        {
            p1Score += enemyValues[enemyID];
            p2Score += enemyValues[enemyID];
        }
    }

    public int[] ReturnScores()
    {
        int[] vs = { p1Score, p2Score };
        return vs;
    }
}