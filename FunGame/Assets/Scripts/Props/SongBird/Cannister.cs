using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : BlankMono
{
    [SerializeField] GameObject parts;

    public void TriggerBurst(GameObject smoke, int damage, int size, int knockback, float time, PlayerBase owner, float impactDur, bool interrupt)
    {
        smoke.SetActive(true);
        parts.SetActive(false);

        smoke.transform.position = transform.position;
        smoke.transform.localScale = Vector3.zero;
        smoke.transform.rotation = new Quaternion(0, 0, 180, 0);

        SmokeBase smoke1 = smoke.GetComponent<SmokeBase>();
        smoke1.Begin(damage, knockback, size, time, owner, tag, impactDur, interrupt);

        for (int i = 0; i < size; i++)
        {
            StartCoroutine(smokeGrowth(i * 0.01f, smoke));
        }


        Collider[] overlaps = Physics.OverlapSphere(transform.position, size + 1);
        if (overlaps.Length != 0)
        {
            print(overlaps.Length + " is the number of things I hit, it was " + overlaps[0].name);
            for (int i = 0; i < overlaps.Length; i++)
            {
                ThingThatCanDie thing = overlaps[i].GetComponent<ThingThatCanDie>();
                if (thing != null && thing.tag != tag)
                {
                    thing.TakeDamage(damage, Vector3.zero, 0, true, interrupt, owner);
                }
            }

        }
        parts.SetActive(true);
        Invoke("Vanish", 0.5f);
    }

    void Vanish() 
    {
        gameObject.SetActive(false); 
    }

    private IEnumerator smokeGrowth(float time, GameObject smokecloud)
    {
        yield return new WaitForSecondsRealtime(time);
        smokecloud.transform.localScale += Vector3.one;
    }
}
