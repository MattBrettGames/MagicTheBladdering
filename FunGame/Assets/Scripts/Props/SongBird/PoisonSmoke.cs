using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : BlankMono
{
    [Header("Editables")]
    public float poisonPerTick;
    private List<GameObject> afflicted = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(TIMETICK());
    }

    private void OnTriggerEnter(Collider other)
    {
        afflicted.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        afflicted.Remove(other.gameObject);
    }

    private IEnumerator TIMETICK()
    {
        yield return new WaitForSeconds(poisonPerTick);
        for (int i = 0; i < afflicted.Count; i++)
        {
            afflicted[i].GetComponent<PlayerBase>().poison++;
        }
    }
}