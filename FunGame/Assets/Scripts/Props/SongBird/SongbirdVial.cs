using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SongbirdVial : Throwables
{
    public float curseDuration;
    private ObjectPooler pooler;
    public string vialType;
    private GameObject smokeCloud;

    public override void CollisionEvent(GameObject collision)
    {
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();

        if (collision.tag != transform.tag)
        {
            if (vialType == "Poison")
            {
                smokeCloud = pooler.poisonSmoke[0];
            }
            if (vialType == "Adrenaline")
            {
                smokeCloud = pooler.adrenalineSmoke[0];
            }
            if (vialType == "Boom")
            {
                smokeCloud = pooler.boomSmoke[0];
            }

            smokeCloud.transform.position = transform.position;
            smokeCloud.transform.localScale = Vector3.zero;
            smokeCloud.SetActive(true);
            for (int i = 0; i < 10; i++) { StartCoroutine(WaitForSmoke(smokeCloud, i)); }
        }
    }

    private IEnumerator WaitForSmoke(GameObject smoke, int time)
    {
        yield return new WaitForSeconds(time/10);
        ScaleUp(smoke);
    }

    private void ScaleUp(GameObject smoke) { smoke.transform.localScale += Vector3.one; }

}
