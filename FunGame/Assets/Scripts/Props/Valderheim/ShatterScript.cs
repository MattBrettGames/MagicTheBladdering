using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterScript : MonoBehaviour
{

    int trueDamage;
    PlayerBase owner;
    [SerializeField] GameObject damageEffect;

    public void SetInfo(int newDamage, PlayerBase ownerTemp)
    {
        trueDamage = newDamage;
        owner = ownerTemp;
        damageEffect.SetActive(false);
    }

    public IEnumerator DealDamage()
    {
        damageEffect.SetActive(false);
        yield return new WaitForEndOfFrame();
        damageEffect.SetActive(true);

        print("Have successfully called DealDamage");

        Collider[] hit = Physics.OverlapSphere(transform.position, transform.localScale.y * 0.5f);

        print("Have gotten " + hit.Length + " overlaps");
        for (int i = 0; i < hit.Length; i++)
        {
            print(hit[i].name);
            if (hit[i].tag.Contains("Player") && hit[i].tag != tag)
            {
                hit[i].GetComponent<PlayerBase>().TakeDamage(trueDamage, Vector3.zero, 0, true, true, owner, 0);
            }
        }

    }
}
