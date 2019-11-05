using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase
{



    int objsLeft = 3;

    public List<GameObject> entourage = new List<GameObject>();


    public void BeginActive()
    {
        gameObject.SetActive(true);
        aggroRange = 100;

        PlayerBase[] players = GameObject.FindObjectsOfType<PlayerBase>();
        targetPlayer = players[Random.Range(0, 2)].gameObject.transform;


        if (entourage != null)
        {
            for (int i = 0; i < entourage.Count; i++)
            {
                entourage[i].SetActive(true);
                entourage[i].GetComponent<EnemyBase>().aggro = true;
            }
        }
    }

    public override void TakeDamage(int damage, string player)
    {
        base.TakeDamage(damage, player);
        UniverseController universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.Invoke("BossDeath()", 5);
    }

    public void ObjectiveDestroyed()
    {
        objsLeft--;
        if (objsLeft <= 0)
        {
            BeginActive();
        }

    }
}