using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : BlankMono
{
    public void TriggerBurst(GameObject smoke, int damage, int size, int knockback, GameObject target, float time)
    {
        smoke.SetActive(true);
        smoke.transform.position = transform.position;
        smoke.transform.rotation = new Quaternion(0, 0, 180, 0);

        SmokeBase smoke1 = smoke.GetComponent<SmokeBase>();
        smoke1.Begin(damage, knockback, target, size, time);

        for (int i = 0; i < size; i++)
        {
            StartCoroutine(smokeGrowth(i * 0.01f, smoke));
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
