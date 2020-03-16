using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDeathController : MonoBehaviour
{
    [SerializeField] GameObject environmentalDeathEffect;

    void Start()
    {
        if (environmentalDeathEffect != null)
            StartCoroutine(NewStart());
    }

    IEnumerator NewStart()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        PlayerBase[] players = GetComponentsInChildren<PlayerBase>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].enviroDeathEffect = Instantiate(environmentalDeathEffect);
            players[i].enviroDeathEffect.SetActive(false);
        }



    }

}
