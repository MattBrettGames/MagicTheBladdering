using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBase : ThingThatCanDie
{
    [SerializeField] int impactDamage;
    public TotemType thisTotemType;
    Vector3 trueTargetPos;
    bool isMoving;
    float lifeSpan;
    Skjegg owner;


    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, trueTargetPos, Time.deltaTime);
        }

        lifeSpan -= Time.deltaTime;
        if(lifeSpan <= 0)
        {
            Vanish();
        }
    }

    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack, PlayerBase attacker)
    {
        base.TakeDamage(damageInc, dirTemp, knockback, fromAttack, stopAttack, attacker);
        if(currentHealth <= 0)
        {
            Vanish();
        }
    }


    public void OnColliderEnter(Collision other)
    {
        if(other.transform.tag != tag)
        {
            Vanish();
            other.gameObject.GetComponent<ThingThatCanDie>().TakeDamage(impactDamage, Vector3.zero, 0, true, true, owner);
        }
    }


    public void SummonTotem(float tempLifeSpan, Skjegg skjegg)
    {
        tag = skjegg.tag;
        isMoving = false;
        lifeSpan = tempLifeSpan;
        owner = skjegg;
    }

    public void RecallTotem(Vector3 targetPos) 
    {
        trueTargetPos = targetPos;
        isMoving = true;
    }

    public void Vanish()
    {
        owner.EndTotemEffect(thisTotemType);
        gameObject.SetActive(false);
        isMoving = false;
    }

}
