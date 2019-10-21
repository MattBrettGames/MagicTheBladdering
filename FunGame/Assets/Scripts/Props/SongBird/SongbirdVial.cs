using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SongbirdVial : Throwables
{
    private ObjectPooler pooler;
    public string vialType;
    private GameObject smokeCloud;
    private Color vialColour;
    private bool moving;
    private Vector3 targetPos;
    private int playerID;

    public void VialThrown(Color colour, Vector3 target, int id, string tag)
    {
        Invoke("VialExplode", 1);
        vialColour = colour;
        GetComponent<MeshRenderer>().material.color = colour;
        targetPos = target;
        playerID = id;
        gameObject.tag = tag;
        moving = true;
    }

    void Update()
    {
        print("Target pos = " + targetPos);
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 1);
        }
    }

    public void VialExplode()
    {
        //moving = false;

        pooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

        if (vialType == "Poison")
        {
            smokeCloud = pooler.poisonSmoke[playerID];
            //pooler.poisonSmoke.Remove(smokeCloud);
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<PoisonSmoke>().Begin();
        }
        if (vialType == "Adrenaline")
        {
            smokeCloud = pooler.adrenalineSmoke[playerID];
            //pooler.adrenalineSmoke.Remove(smokeCloud);
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<AdrenalineSmoke>().Begin();
        }
        if (vialType == "Boom")
        {
            smokeCloud = pooler.boomSmoke[playerID];
            //pooler.boomSmoke.Remove(smokeCloud);
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<BoomSmoke>().Begin();
        }

        smokeCloud.tag = gameObject.tag; 
        smokeCloud.GetComponent<MeshRenderer>().material.color = vialColour;
        smokeCloud.SetActive(true);
        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = new Vector3(15, 15, 15);
        pooler.ReturnToVialPool(gameObject);
    }
}