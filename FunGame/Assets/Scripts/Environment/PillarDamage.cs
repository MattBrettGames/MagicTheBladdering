using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarDamage : ThingThatCanDie
{

    Rigidbody[] rb2ds = new Rigidbody[0];
    [SerializeField] GameObject column;
    [SerializeField] GameObject destructable;

    Vector3 dir;

    void Start()
    {
        rb2ds = destructable.GetComponentsInChildren<Rigidbody>();
    }

    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack)
    {
        dir = dirTemp;
        if (damageInc >= 15)
        {
            EngageDestruction();
            Time.timeScale = 0.2f;
            StartCoroutine(HitStopStop());
        }
    }

    IEnumerator HitStopStop()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
    }

    void EngageDestruction()
    {
        column.SetActive(false);

        for (int i = 0; i < rb2ds.Length; i++)
        {
            rb2ds[i].isKinematic = false;
            rb2ds[i].AddForce(dir * Random.Range(0.1f, 2), ForceMode.Impulse);
            StartCoroutine(RemoveRock(rb2ds[i].gameObject, 3));
        }
    }

    IEnumerator RemoveRock(GameObject rock, int time)
    {
        yield return new WaitForSeconds(time);
        rock.SetActive(false);

    }

}
