using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHazard : BlankMono
{
    [Header("Damage")]
    public int damageToEnemy;
    public int damageToPlayer;

    [Header("Knockback")]
    public Vector3 dir;
    public int force;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag != "Zone")
        {
            if (other.gameObject.GetComponent<PlayerBase>() != null)
            {
                PlayerBase code = other.gameObject.GetComponent<PlayerBase>();
                code.currentHealth -= damageToPlayer;
                code.TakeDamage(1, false);
                code.Knockback(force, dir);

            }
        }
    }
}