using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongbirdVial : Throwables
{
    public float curseDuration;
    private ObjectPooler pooler;
    public string vialType;

    public override void CollisionEvent(GameObject collision)
    {
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();

        if(collision.tag != transform.tag)
        {
            if (vialType == "Poison") { pooler.poisonSmoke[0].transform.position = transform.position; }// pooler.transform.sc }
            if(vialType == "Adrenaline") { }
            if(vialType == "Boom") { }
        }


    }
}
