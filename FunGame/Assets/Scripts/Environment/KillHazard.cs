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
    [SerializeField] float knockbackDuration = 0.1f;

    [Header("Animation?")]
    [SerializeField] bool hasAnimation;
    [SerializeField] Animator anims;
    [SerializeField] string animName;

    void OnCollisionEnter(Collision other)
    {
        DealDamage(other.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        DealDamage(other.gameObject);
    }

    void DealDamage(GameObject other)
    {
        if (other.transform.tag != "Zone")
        {
            if (other.gameObject.GetComponent<PlayerBase>() != null)
            {
                PlayerBase code = other.gameObject.GetComponent<PlayerBase>();
                code.TakeDamage(damageToPlayer, dir, force, false, false, null, knockbackDuration);

                if (hasAnimation)
                {
                    anims.SetTrigger(animName);
                }
                if (code.trueIFrames)
                {
                    int otherLayer = other.gameObject.layer;
                    Physics.IgnoreLayerCollision(otherLayer, gameObject.layer, true);
                    StartCoroutine(EndPass(otherLayer));
                }
            }
        }

    }


    IEnumerator EndPass(int otherLayer)
    {
        yield return new WaitForSecondsRealtime(2);
        Physics.IgnoreLayerCollision(otherLayer, gameObject.layer, false); ;
    }

}