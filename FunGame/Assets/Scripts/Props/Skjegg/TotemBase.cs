using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBase : ThingThatCanDie
{
    public TotemType thisTotemType;
    float lifeSpan;
    Skjegg owner;

    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0 && gameObject.activeSelf)
        {
            Vanish();
        }
    }

    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack, PlayerBase attacker)
    {
        base.TakeDamage(damageInc, dirTemp, knockback, fromAttack, stopAttack, attacker);
        if (currentHealth <= 0)
        {
            Vanish();
        }
    }

    public void SummonTotem(float tempLifeSpan, Skjegg skjegg)
    {
        enabled = true;
        tag = skjegg.tag;
        lifeSpan = tempLifeSpan;
        owner = skjegg;
    }

    public void Vanish()
    {
        owner.EndTotemEffect(thisTotemType);
        gameObject.SetActive(false);
        enabled = false;
    }

}
