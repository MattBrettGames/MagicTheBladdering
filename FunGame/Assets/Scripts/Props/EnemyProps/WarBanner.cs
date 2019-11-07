using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBanner : BlankMono
{
    public float buffPercent;
    public float buffRadius;
    public GameObject buffRing;

    private SphereCollider sphere;
    private Animator anim;
    
    public void BeginBuff()
    {
        anim = buffRing.GetComponent<Animator>();
        sphere.radius = buffRadius;
    }

    private IEnumerator BuffLoop()
    {
        yield return new WaitForSeconds(2);
        anim.SetTrigger("BuffRing");
        StartCoroutine(BuffLoop());
    }

    void OnCollisionEnter(Collision other)
    {
        EnemyBase buffed = other.gameObject.GetComponent<EnemyBase>();

        if (buffed != null)
        {
            buffed.atkMult += buffPercent;
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        EnemyBase buffed = other.gameObject.GetComponent<EnemyBase>();

        if (buffed != null)
        {
            buffed.atkMult -= buffPercent;
        }
    }

}