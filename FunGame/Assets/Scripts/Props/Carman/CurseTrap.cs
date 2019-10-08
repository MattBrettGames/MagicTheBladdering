using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseTrap : BlankMono
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag != "Player")
        {
            other.gameObject.GetComponent<PlayerBase>().GainCurse(15);
            GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>().ReturnToCurseTrapList(gameObject);
        }
    }
}
