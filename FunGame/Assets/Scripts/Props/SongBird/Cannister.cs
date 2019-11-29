using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : BlankMono
{
    private GameObject smokeTrue;

    public void TriggerBurst(GameObject smoke, int damage, int tick, int size)
    {
        smokeTrue = smoke;
        smoke.SetActive(true);
        smoke.transform.position = transform.position;
        smoke.transform.rotation = new Quaternion(0, 0, 180, 0);

        SmokeBase smoke1 = smoke.GetComponent<SmokeBase>();
        smoke1.Begin(damage, tick);

        smoke.GetComponent<SmokeBase>().Begin(damage, tick);
        for (int i = 0; i < size; i++)
        {
            StartCoroutine(smokeGrowth(i * 0.01f, smokeTrue));
        }
        Invoke("Vanish", 0.5f);
    }

    void Vanish() { gameObject.SetActive(false); }

    private IEnumerator smokeGrowth(float time, GameObject smokecloud)
    {
        yield return new WaitForSeconds(time);
        smokecloud.transform.localScale += Vector3.one;
    }
}
