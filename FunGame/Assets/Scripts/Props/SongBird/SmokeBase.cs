using UnityEngine;

public class SmokeBase : BlankMono
{
    private PlayerBase target;
    private int damageTrue;
    private int ticksTrue;
    private int forceTrue;

    virtual public void Begin(int damage, int ticks, int force)
    {
        damageTrue = damage;
        ticksTrue = ticks;
        forceTrue = force;
    }

    void OnTriggerEnter(Collider other)
    {
        target = other.gameObject.GetComponent<PlayerBase>();
        if (target != null)
        {
            if (other.transform.tag != tag)
            {
                target.TakeDamage(damageTrue);
                target.poison += ticksTrue;
                target.Knockback(-forceTrue, transform.position - other.transform.position);
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
