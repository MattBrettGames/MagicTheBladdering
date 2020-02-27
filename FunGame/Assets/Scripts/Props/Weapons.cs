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
    AttackType currentAttack;

    public void GainInfo(int damage, int knockback, Vector3 forward, bool pvp, float stunDur, PlayerBase owner, bool stopAttack, AttackType attackType)
    {
        damageFull = damage;
        knockFull = knockback;
        knockDir = forward;
        pvpTrue = pvp;
        stunDurTrue = stunDur;
        ownerTrue = owner;
        stopAttackTrue = stopAttack;
        head = trails.transform;
        tag = owner.tag;
        currentAttack = attackType;
    }

    private void Start()
    {
        hitBox = gameObject.GetComponent<Collider>();
        hitBox.enabled = false;
        trails = gameObject.GetComponentInChildren<TrailRenderer>();
        trails.gameObject.SetActive(false);
    }

    public void StartAttack()
    {
        hitBox.enabled = true;
        trails.enabled = true;
        trails.gameObject.SetActive(true);
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        trails.enabled = false;
        trails.gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        ThingThatCanDie player = other.gameObject.GetComponent<ThingThatCanDie>();
        if (other.tag != tag)
        {
            player.TakeDamage(damageFull, knockDir, knockFull, true, stopAttackTrue, ownerTrue);
            ownerTrue.ControllerRumble(damageFull * 0.1f, 0.2f, true, player as PlayerBase);
            ownerTrue.OnHit(player as PlayerBase, currentAttack);
        }
    }
}