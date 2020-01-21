using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : BlankMono
{
    public void TriggerBurst(GameObject smoke, int damage, int size, int knockback, float time, PlayerBase owner, float impactDur, bool interrupt)
    {
        smoke.SetActive(true);
        smoke.transform.position = transform.position;
        smoke.transform.localScale = Vector3.zero;
        smoke.transform.rotation = new Quaternion(0, 0, 180, 0);

        SmokeBase smoke1 = smoke.GetComponent<SmokeBase>();
        smoke1.Begin(damage, knockback, size, time, owner, tag, impactDur, interrupt);

        for (int i = 0; i < size; i++)
        {
            StartCoroutine(smokeGrowth(i * 0.01f, smoke));
        }
        Invoke("Vanish", 0.5f);
    }

    void Vanish() { gameObject.SetActive(false); }

    private IEnumerator smokeGrowth(float time, GameObject smokecloud)
    {
        yield return new WaitForSecondsRealtime(time);
        smokecloud.transform.localScale += Vector3.one;
    }
}
