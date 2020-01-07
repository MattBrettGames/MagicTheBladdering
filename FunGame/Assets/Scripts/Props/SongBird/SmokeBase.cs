using UnityEngine;

public class SmokeBase : BlankMono
{
    private bool exploding;
    private PlayerBase target;
    private int damageTrue;
    private float ticksTrue;
    private int forceTrue;

    virtual public void Begin(int damage, float ticks, int force, GameObject targetLooker, float size)
    {
        damageTrue = damage;
        ticksTrue = ticks;
        forceTrue = force;
        exploding = true;
        Invoke("EndForce", 0.2f);

        target = targetLooker.GetComponentInParent<PlayerBase>();
        InvokeRepeating("PoisonCheck", ticks, ticks);

        target = targetLooker.GetComponentInParent<PlayerBase>();

        GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>().CameraRumbleCall();

        if (Vector3.Distance(target.gameObject.transform.position, transform.position) <= size)
        {
            target.TakeDamage(damageTrue, true);
            target.Knockback(forceTrue, new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z));
        }
    }

    void EndForce() { exploding = false; }

    void PoisonCheck()
    {
        if (Vector3.Distance(target.gameObject.transform.position, transform.position) <= 1 * transform.localScale.y)
        {
            target.currentHealth -= 1;
            target.ControllerRumble(0.1f, 0.1f);
        }
    }
}