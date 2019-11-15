using UnityEngine;

public class SmokeBase : BlankMono
{
    private PlayerBase target;
    private int damageTrue;
    private int ticksTrue;

    virtual public void Begin(int damage, int ticks)
    {
        damageTrue = damage;
        ticksTrue = ticks;
    }

    void OnCollisionEnter(Collision other)
    {
        target = other.gameObject.GetComponent<PlayerBase>();

        if (target != null)
        {
            if (other.transform.tag != tag)
            {
                target.TakeDamage(damageTrue);
                target.poison += ticksTrue;
            }
            else
            {
                target.damageMult += 1;
                target.dodgeDur += 0.5f;
                target.dodgeSpeed += 5;
            }
        }
    }

    void OnCollsiionExit(Collider other)
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
