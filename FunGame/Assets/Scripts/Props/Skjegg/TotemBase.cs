using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBase : ThingThatCanDie
{
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
    
    public void SummonTotem(float tempLifeSpan, Skjegg skjegg)
    {
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
