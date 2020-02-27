using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmokeBase : MonoBehaviour
{
    private PlayerBase owner;
    bool isBurst;
    int damageTrue;
    int forceTrue;
    bool stopAttack;

    virtual public void Begin(int damage, int force, float size, float time, PlayerBase ownerTemp, string tagtemp, float impactDur, bool stopAttackTemp, Color playerColour)
    {
        CancelInvoke();
        StopAllCoroutines();
        tag = tagtemp;
        isBurst = true;
        gameObject.transform.localScale = Vector3.zero;
        owner = ownerTemp;
        stopAttack = stopAttackTemp;

        GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>().CameraRumbleCall(0.1f);

        StartCoroutine(StopBurst(impactDur));
        for (int i = 0; i < size; i++)
        {
            StartCoroutine(Shrink(time + (i * 0.05f)));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != tag)
        {
            ThingThatCanDie otherCode = other.GetComponent<ThingThatCanDie>();
            if (otherCode.gameObject.name.Contains("Vald") || otherCode.gameObject.name.Contains("Song") || otherCode.gameObject.name.Contains("Carm") || otherCode.gameObject.name.Contains("Wios") || otherCode.gameObject.name.Contains("Skjegg"))
            {
                PlayerBase otterCode = other.GetComponent<PlayerBase>();
                otterCode.poison = true;
            }

            if (isBurst)
            {
                otherCode.TakeDamage(damageTrue, transform.position - other.transform.position, forceTrue, true, stopAttack, owner);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != tag)
        {
            PlayerBase otherCode = other.GetComponent<PlayerBase>();
            otherCode.poison = false;
        }
    }

    IEnumerator Shrink(float time)
    {
        yield return new WaitForSeconds(time);
        transform.localScale -= Vector3.one;
        transform.position -= new Vector3(0, 0.1f, 0);
        if (transform.localScale.y <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator StopBurst(float impactDur)
    {
        yield return new WaitForSeconds(impactDur);
        isBurst = false;
    }
}