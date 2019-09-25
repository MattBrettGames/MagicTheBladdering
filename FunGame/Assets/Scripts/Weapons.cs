using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : BlankMono
{
    private int damageFull;

    public void GainInfo(int damage)
    {
        damageFull = damage;
    }

    public void StartAttack()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void EndAttack()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag != transform.tag)
        {
            PlayerBase target = other.transform.GetComponent<PlayerBase>();
            target.TakeDamage(damageFull);
            target.KnockedDown(damageFull/10);
        }
    }


}
