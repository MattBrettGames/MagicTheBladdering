using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaExplosions : MonoBehaviour
{

    PlayerBase ownerTrue;
    int damageFull;
    int knockFull;

    Vector3 dirTrue;
    float spaceTrue;

    //Collider hitBox;
    ParticleSystem parts;

    public void Setup()
    {
        //hitBox = GetComponent<Collider>();
        parts = GetComponentInChildren<ParticleSystem>();
    }

    public void StartChain(PlayerBase owner, int damage, int knockback, WiosnaExplosions nextBlast, Vector3 lastPos, Vector3 dir, float spacing, float timeBetweenBlasts, int remaining, UniverseController uni, string ySound)
    {
        print("Chain Started + " + (dir * spacing));

        tag = owner.tag;

        ownerTrue = owner;
        damageFull = damage;
        knockFull = knockback;
        dirTrue = dir;
        spaceTrue = spacing;

        gameObject.SetActive(true);
        gameObject.transform.position += dir * spacing;
        remaining--;
        damageFull--;

        if (remaining > 0)
        {
            StartCoroutine(NextBlast(timeBetweenBlasts, nextBlast, remaining, uni, ySound));
            uni.PlaySound(ySound);
        }

        parts.Clear();
        parts.Play();

        StartCoroutine(Fade(timeBetweenBlasts * 2f));

    }


    IEnumerator NextBlast(float time, WiosnaExplosions next, int remaining, UniverseController uni, string ySound)
    {
        yield return new WaitForSeconds(time);

        next.StartChain(ownerTrue, damageFull, knockFull, this, transform.position, dirTrue, spaceTrue, time, remaining, uni, ySound);
    }

    IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);

        parts.Stop();
        parts.Clear();
        gameObject.SetActive(false);
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerBase player = other.gameObject.GetComponent<PlayerBase>();
        if (player != null && player.tag != tag)
        {
            ownerTrue.ControllerRumble(damageFull * 0.1f, 0.2f);
            player.TakeDamage(damageFull, true, true);
            player.Knockback(knockFull, transform.position - player.transform.position);
        }
        if (player == null)
        {
            FlamingWiosna clone = other.gameObject.GetComponent<FlamingWiosna>();
            clone.TakeDamage(damageFull);
        }
    }
}