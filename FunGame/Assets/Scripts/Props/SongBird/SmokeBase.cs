using UnityEngine;

public class SmokeBase : BlankMono
{
    private bool exploding;
    private PlayerBase target;
    private int damageTrue;
    private int ticksTrue;
    private int forceTrue;

    virtual public void Begin(int damage, int ticks, int force)
    {
        damageTrue = damage;
        ticksTrue = ticks;
        forceTrue = force;
        exploding = true;
        Invoke("EndForce", 0.2f);
    }

    void EndForce() { exploding = false; }

    void OnTriggerEnter(Collider other)
    {
        target = other.gameObject.GetComponent<PlayerBase>();
        if (target != null)
        {
            if (other.transform.tag != tag)
            {
                target.TakeDamage(damageTrue);
                target.poison += ticksTrue;
                if (exploding)
                {
                    target.Knockback(forceTrue, other.transform.position - transform.position);
                }
            }
            else
            {
                target.damageMult += 1;
                target.dodgeDur += 0.5f;
                target.dodgeSpeed += 5;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        target = other.GetComponent<PlayerBase>();

        if (target != null)
        {
            if (other.tag == tag)
            {
                target.damageMult -= 1;
                target.dodgeDur -= 0.5f;
                target.dodgeSpeed -= 5;
            }
        }
    }


}
