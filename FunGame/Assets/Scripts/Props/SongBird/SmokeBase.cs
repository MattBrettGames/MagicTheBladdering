using UnityEngine;

public class SmokeBase : BlankMono
{

    private PlayerBase target;


    virtual public void Begin(int damage, int force, GameObject targetLooker, float size, float time)
    {

        target = targetLooker.GetComponentInParent<PlayerBase>();

        GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>().CameraRumbleCall();

        if (Vector3.Distance(target.gameObject.transform.position, transform.position) <= size)
        {
            target.TakeDamage(damage, true);
            target.Knockback(force, new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z));
        }
        CancelInvoke();

        for (int i = 0; i < size; i++)
        {
            Invoke("Shrink", time + (i * 0.01f));
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

    void Shrink()
    {
        transform.localScale -= Vector3.one;
        if (transform.localScale.y <= 0)
        {
            gameObject.SetActive(false);
        }
    }


}