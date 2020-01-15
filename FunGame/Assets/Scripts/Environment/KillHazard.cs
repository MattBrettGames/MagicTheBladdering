using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHazard : BlankMono
{
    [Header("Damage")]
    public int damageToPlayer;

    [Header("Knockback")]
    public Vector3 dir;
    public int force;

    [Header("Animation?")]
    [SerializeField] bool hasAnimation;
    [SerializeField] Animator anims;
    [SerializeField] string animName;
       
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag != "Zone")
        {
            if (other.gameObject.GetComponent<PlayerBase>() != null)
            {
                PlayerBase code = other.gameObject.GetComponent<PlayerBase>();
                code.currentHealth -= damageToPlayer;
                code.TakeDamage(1, dir, force, false, false);

                if (hasAnimation)
                {
                    anims.SetTrigger(animName);
                }
            }
        }
    }
}