using UnityEngine;
using System.Collections;

public class SmokeBase : MonoBehaviour
{
    private PlayerBase target;

    virtual public void Begin(int damage, int force, GameObject targetLooker, float size, float time)
    {
        //CancelInvoke();

        target = targetLooker.GetComponentInParent<PlayerBase>();

        GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>().CameraRumbleCall();

        if (Vector3.Distance(target.gameObject.transform.position, transform.position) <= size)
        {
            target.TakeDamage(damage, new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z), force, true, false);
        }

        Collider[] obj = Physics.OverlapSphere(transform.position, size);
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i].name.Contains("FlamingClone"))
            {
                obj[i].GetComponent<FlamingWiosna>().TakeDamage(damage, Vector3.zero, 0, false, false);
            }
        }


        for (int i = 0; i < size; i++)
        {
            StartCoroutine(Shrink(time + (i * 0.01f)));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == target.tag)
        {
            target.poison = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == target.tag)
        {
            target.poison = false;
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
}