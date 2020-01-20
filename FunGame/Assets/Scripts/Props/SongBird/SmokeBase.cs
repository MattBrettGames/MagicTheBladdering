using UnityEngine;
using System.Collections;

public class SmokeBase : MonoBehaviour
{
    private PlayerBase owner;
    bool isBurst;
    int damageTrue;
    int forceTrue;


    virtual public void Begin(int damage, int force, float size, float time, PlayerBase ownerTemp, string tagtemp)
    {
        CancelInvoke();
        tag = tagtemp;
        isBurst = true;
        gameObject.transform.localScale = Vector3.zero;
        owner = ownerTemp;

        GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>().CameraRumbleCall();

        StartCoroutine(StopBurst());
        for (int i = 0; i < size; i++)
        {
            StartCoroutine(Shrink(time + (i * 0.05f)));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != tag)
        {
            PlayerBase otherCode = other.GetComponent<PlayerBase>();
            otherCode.poison = true;
            
            if (isBurst)
            {
                otherCode.TakeDamage(damageTrue, transform.position - other.transform.position, forceTrue, true, true, owner);
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
        yield return new WaitForSecondsRealtime(time);
        transform.localScale -= Vector3.one;
        if (transform.localScale.y <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator StopBurst()
    {
        yield return new WaitForSeconds(0.2f);
        isBurst = false;
    }

}