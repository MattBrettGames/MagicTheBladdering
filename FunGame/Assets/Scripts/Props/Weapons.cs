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

    public void GainInfo(int damage, int knockback, Vector3 forward)
    {
        damageFull = damage;
        knockFull = knockback;
        knockDir = forward;
    }

    private void Start()
    {
        hitBox = gameObject.GetComponent<BoxCollider>();
        //trails = gameObject.GetComponent<TrailRenderer>();
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
//        print("Collided with " + other);
        if (other.transform.tag != transform.tag)
        {
            PlayerBase target = other.transform.GetComponent<PlayerBase>();
            target.KnockedDown(damageFull / 10);
            target.Knockback(knockFull, knockDir);
            target.TakeDamage(damageFull);
        }
    }


}
