using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : BlankMono
{
    [Header("Editables")]
    public float secsBetweenTicks;
    private List<PlayerBase> afflicted = new List<PlayerBase>();

    private void Start()
    {
        StartCoroutine(TIMETICK());
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
}