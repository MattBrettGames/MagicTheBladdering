using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SongbirdVial : Throwables
{
    private ObjectPooler pooler;
    public string vialType;
    private GameObject smokeCloud;

    public void VialThrown() { Invoke("VialExplode", 1);  }

    public void VialExplode()
    {
        print("Vial hath exploded");
        pooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        print(vialType + " is the current vial type");

        if (vialType == "Poison")
        {
            smokeCloud = pooler.poisonSmoke[pooler.poisonSmoke.Count-1];
            pooler.poisonSmoke.Remove(smokeCloud);
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<PoisonSmoke>().Begin();
        }
        if (vialType == "Adrenaline")
        {
            smokeCloud = pooler.adrenalineSmoke[pooler.adrenalineSmoke.Count-1];
            pooler.adrenalineSmoke.Remove(smokeCloud);
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<AdrenalineSmoke>().Begin();
        }
        if (vialType == "Boom")
        {
            smokeCloud = pooler.boomSmoke[pooler.boomSmoke.Count-1];
            pooler.boomSmoke.Remove(smokeCloud);
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<BoomSmoke>().Begin();
        }


        smokeCloud.SetActive(true);
        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = new Vector3(10, 10, 10);
        pooler.ReturnToVialPool(gameObject);
    }

}
