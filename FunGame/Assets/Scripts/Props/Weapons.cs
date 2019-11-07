using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : BlankMono
{
    private int damageFull;
    private int knockFull;
    private Vector3 knockDir;
    private BoxCollider hitBox;
    private TrailRenderer trails;
    private bool pvpTrue;


    public void GainInfo(int damage, int knockback, Vector3 forward, bool pvp)
    {
        damageFull = damage;
        knockFull = knockback;
        knockDir = forward;
        pvpTrue = pvp;
    }

    private void Start()
    {
        hitBox = gameObject.GetComponent<BoxCollider>();
        //trails = gameObject.GetComponentInChildren<TrailRenderer>();
        //trails.enabled = false;
        hitBox.enabled = false;
    }

    public void StartAttack()
    {
        hitBox.enabled = true;
        //trails.enabled = true;
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        //trails.enabled = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (pvpTrue)
        {
            if(other.transform.tag != tag)
            {
                other.GetComponent<PlayerBase>().TakeDamage(damageFull);
            }
        }
        else
        {
            if (other.transform.tag == "Enemy")
            {
                EnemyBase target = other.transform.GetComponent<EnemyBase>();
                target.TakeDamage(damageFull, tag);
            }
            else if (other.transform.tag == "Objective")
            {
                ObjectiveController target = other.GetComponent<ObjectiveController>();
                target.TakeDamage(damageFull);
            }
        }
    }


}
