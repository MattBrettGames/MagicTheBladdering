using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : BlankMono
{
    private int damageFull;
    private BoxCollider hitBox;
    private TrailRenderer trails;

    public void GainInfo(int damage)
    {
        damageFull = damage;
    }

    private void Start()
    {
        hitBox = gameObject.GetComponentInChildren<BoxCollider>();
        trails = gameObject.GetComponentInChildren<TrailRenderer>();
        trails.enabled = false;
        hitBox.enabled = false;
    }

    public void StartAttack()
    {
        hitBox.enabled = true;
        trails.enabled = true;
    }

    public void EndAttack()
    {
        trails.enabled = false;
        hitBox.enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag != transform.tag)
        {
            PlayerBase target = other.transform.GetComponent<PlayerBase>();
            target.TakeDamage(damageFull);
            target.KnockedDown(damageFull / 10);
        }
    }


}
