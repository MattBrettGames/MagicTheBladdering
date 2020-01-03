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
                target.poison += ticksTrue;
                if (exploding)
                {
                    target.TakeDamage(damageTrue, false);
                    target.Knockback(forceTrue, new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z));
                }
            }
            else
            {
                target.damageMult += 1;                
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
            }
        }
    }


}
