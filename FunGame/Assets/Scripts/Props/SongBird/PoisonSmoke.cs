using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : SmokeBase
{
    [Header("Editables")]
    public float secsBetweenTicks;
    public float duration;
    private List<PlayerBase> afflicted = new List<PlayerBase>();

    public override void Begin(string tagToGet)
    {
        StartCoroutine(TIMETICK());
        Invoke("Return", duration);
        tag = tagToGet;
    }

    private void OnTriggerEnter(Collider other)
    {
        afflicted.Add(other.gameObject.GetComponent<PlayerBase>()) ;
    }

    private void OnTriggerExit(Collider other)
    {
        afflicted.Remove(other.gameObject.GetComponent<PlayerBase>());
    }

    private IEnumerator TIMETICK()
    {
        yield return new WaitForSeconds(secsBetweenTicks);
        for (int i = 0; i < afflicted.Count; i++)
        {
            afflicted[i].poison++;
        }
        TIMETICK();
    }

    private void Return()
    {
        GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>().ReturnToPoisonSmokePool(gameObject);
    }

}