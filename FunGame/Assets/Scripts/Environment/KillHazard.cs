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

    void Start()
    {
    }
    void OnCollisionEnter(Collision other)
    {
        print("Yup;oiuhoiugh");
        DealDamage(other.gameObject);
        Physics.IgnoreLayerCollision(other.gameObject.layer, gameObject.layer, false);
    }

    void OnTriggerEnter(Collider other)
    {
        print("Yup;oiuhoiugh, but this time it's a trigger");
        DealDamage(other.gameObject);
        Physics.IgnoreLayerCollision(other.gameObject.layer, gameObject.layer, false);
    }

    void DealDamage(GameObject other)
    {
        if (other.transform.tag != "Zone")
        {
            print(other.name + " is what has been hit");

            PlayerBase code = other.gameObject.GetComponent<PlayerBase>();
            if (code != null)
            {
                print("got the script is what has been hit");
                code.TakeDamage(damageToPlayer, dir, force, false, false, null, knockbackDuration);

                if (hasAnimation)
                {
                    anims.SetTrigger(animName);
                }
                if (code.trueIFrames)
                {
                    print("it had trueFrames :(");
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
        Physics.IgnoreLayerCollision(otherLayer, gameObject.layer, false);
    }

}
