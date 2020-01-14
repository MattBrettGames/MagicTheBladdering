using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : BlankMono
{
    private int damageFull;
    private int knockFull;
    private Vector3 knockDir;
    private Collider hitBox;
    private TrailRenderer trails;
    private bool pvpTrue;
    private float stunDurTrue;
    PlayerBase ownerTrue;
    bool stopAttackTrue;
    [HideInInspector] public Transform head;

    public void GainInfo(int damage, int knockback, Vector3 forward, bool pvp, float stunDur, PlayerBase owner, bool stopAttack)
    {
        damageFull = damage;
        knockFull = knockback;
        knockDir = forward;
        pvpTrue = pvp;
        stunDurTrue = stunDur;
        ownerTrue = owner;
        stopAttackTrue = stopAttack;
        head = trails.transform;
    }

    private void Start()
    {
        hitBox = gameObject.GetComponent<Collider>();
        hitBox.enabled = false;
        trails = gameObject.GetComponentInChildren<TrailRenderer>();
        trails.enabled = false;
    }

    public void StartAttack()
    {
        hitBox.enabled = true;
        trails.enabled = true;
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        trails.enabled = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        ThingThatCanDie player = other.gameObject.GetComponent<PlayerBase>();
        if (player != null)
        {
            ownerTrue.ControllerRumble(damageFull * 0.1f, 0.2f);
            player.TakeDamage(damageFull, knockDir, true, stopAttackTrue);
            player.Knockback(knockFull, knockDir);
        }

        if (player == null)
        {
            player = other.gameObject.GetComponent<FlamingWiosna>();
            player.TakeDamage(damageFull, knockDir, false, false);
        }

        if (player == null)
        {
            other.GetComponentInParent<PillarDamage>().TakeDamage(damageFull, knockDir, false, false);
        }
    }


}
