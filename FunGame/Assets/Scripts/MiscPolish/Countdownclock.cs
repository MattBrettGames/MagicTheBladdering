using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdownclock : MonoBehaviour
{
    [SerializeField] GameObject firstNum;
    [SerializeField] GameObject secondNum;
    [SerializeField] GameObject thirdNum;
    [SerializeField] GameObject fightSign;


    void Start()
    {
        firstNum.SetActive(false);
        secondNum.SetActive(false);
        thirdNum.SetActive(false);
        fightSign.SetActive(false);

        Invoke("CallFirst", 0.3f);
        Invoke("CallSecond", 1.3f);
        Invoke("CallThird", 2.3f);
        Invoke("CallFight", 3.3f);
        Invoke("EndAll", 4.3f);
    }

    void CallFirst()
    {
        firstNum.SetActive(true);
    }

    void CallSecond()
    {
        firstNum.SetActive(false);
        secondNum.SetActive(true);
    }

    void CallThird()
    {
        secondNum.SetActive(false);
        thirdNum.SetActive(true);
    }

    void CallFight()
    {
        thirdNum.SetActive(false);
        fightSign.SetActive(true);
    }

    void EndAll()
    {
        fightSign.SetActive(false);
    }


}
