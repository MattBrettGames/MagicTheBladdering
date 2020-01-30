using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJetHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> JetObj = new List<GameObject>(); //create class for these

    [SerializeField]
    private float maxTime = 15f;
    [SerializeField]  //time between outbursts
    private float minTime = 8f;
    [SerializeField]
    private float maxDuration = 5f;
    [SerializeField]     //random between these two values. length of outburst
    private float minDuration = 3f;

    [SerializeField]
    private int maxActive = 3;   //max no active at once.

    public int currentActive = 0;

    void Start()
    {
        StartCoroutine(outburstTimer()); //get going.
    }


    public void SetInactive()
    {
        currentActive -= 1;
    }

    IEnumerator outburstTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
           
            foreach(GameObject go in JetObj)
            {
                FireJet fj = go.GetComponent<FireJet>();
                if (!fj.isActive && currentActive < maxActive)
                {
                    fj.setActive(Random.Range(minDuration, maxDuration));
                    currentActive += 1;
                    break;
                }
                yield return null;
            }
        }
    }


}
