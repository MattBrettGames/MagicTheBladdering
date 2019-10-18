using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SongbirdVial : Throwables
{
    private ObjectPooler pooler;
    public string vialType;
    private GameObject smokeCloud;

    public void VialThrown() { Invoke("VialExplode", 5); }

    public void VialExplode()
    {
        pooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

        if (vialType == "Poison")
        {
            smokeCloud = pooler.poisonSmoke[0];
            pooler.poisonSmoke.Remove(smokeCloud);
        }
        if (vialType == "Adrenaline")
        {
            smokeCloud = pooler.adrenalineSmoke[0];
            pooler.adrenalineSmoke.Remove(smokeCloud);
        }
        if (vialType == "Boom")
        {
            smokeCloud = pooler.boomSmoke[0];
            pooler.boomSmoke.Remove(smokeCloud);
        }

        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.SetActive(true);
        for (int i = 0; i < 10; i++) { StartCoroutine(WaitForSmoke(smokeCloud, i)); }
    }

    private IEnumerator WaitForSmoke(GameObject smoke, int time)
    {
        yield return new WaitForSeconds(time/10);
        ScaleUp(smoke);
    }

    private void ScaleUp(GameObject smoke) { smoke.transform.localScale += Vector3.one; }

}
