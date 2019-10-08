using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritDagger : Weapons
{
    public void CallGhost()
    {
        ObjectPooler pooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        GameObject ghost = pooler.ghostList[0];
        ghost.transform.position = transform.position;
        ghost.SetActive(true);        
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.GetComponent<PlayerBase>() != null)
        {
            CallGhost();
        }
    }
}
