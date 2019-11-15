using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : BlankMono
{
    private GameObject smokeTrue;

    public void TriggerBurst(GameObject smoke, int damage, int tick)
    {
        smokeTrue = smoke;
        smoke.SetActive(true);
        smoke.transform.position = transform.position;

        smoke.GetComponent<SmokeBase>().Begin(damage, tick);
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(smokeGrowth(i * 0.01f, smokeTrue));
            print(i * 0.01f);
        }

        gameObject.SetActive(false);
    }
    
    private IEnumerator smokeGrowth(float time, GameObject smokecloud)
    {
        print("SmokeGrowth");
        yield return new WaitForSeconds(time);
        smokecloud.transform.localScale += Vector3.one;
    }
}
