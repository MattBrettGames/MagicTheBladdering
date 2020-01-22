using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaExplosions : MonoBehaviour
{

    PlayerBase ownerTrue;
    int damageFull;
    int knockFull;
    Vector3 knockDir;
    float spaceTrue;
    ParticleSystem parts;

    public void Setup()
    {
        parts = GetComponentInChildren<ParticleSystem>();
    }

    public void StartChain(PlayerBase owner, int damage, int knockback, WiosnaExplosions nextBlast, Vector3 lastPos, Vector3 dir, float spacing, float timeBetweenBlasts, int remaining, UniverseController uni, string ySound)
    {
        gameObject.tag = owner.tag;

        ownerTrue = owner;
        damageFull = damage;
        knockFull = knockback;
        knockDir = dir;
        spaceTrue = spacing;

        parts.Clear();
        gameObject.SetActive(true);

        gameObject.transform.position += (dir * spacing);
        nextBlast.transform.position += (dir * spacing);
        remaining--;
        damageFull -= 2;

        if (remaining > 0)
        {
            StartCoroutine(NextBlast(timeBetweenBlasts, nextBlast, remaining, uni, ySound));
            uni.PlaySound(ySound);
        }

        parts.Play();

        StartCoroutine(Fade(timeBetweenBlasts * 2f));

    }


    IEnumerator NextBlast(float time, WiosnaExplosions next, int remaining, UniverseController uni, string ySound)
    {
        yield return new WaitForSeconds(time);

        next.StartChain(ownerTrue, damageFull, knockFull, this, transform.position, knockDir, spaceTrue, time, remaining, uni, ySound);
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
        if (other.tag != tag || other.tag == "Untagged")
        {
            print(other.name + " is the thing I hit");
            ThingThatCanDie player = other.gameObject.GetComponent<ThingThatCanDie>();
            player.TakeDamage(damageFull, knockDir, knockFull, true, true, ownerTrue);
            ownerTrue.ControllerRumble(damageFull * 0.1f, 0.2f);
        }
    }
}