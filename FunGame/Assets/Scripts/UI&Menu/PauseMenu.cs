using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Player players;

    [SerializeField] GameObject[] options;
    [SerializeField] GameObject selector;
    int currentDisplay;

    [SerializeField] string[] optionStrings;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (players.GetButtonDown("Pause"))
        {
            gameObject.SetActive(true);
            Time.timeScale = Mathf.Epsilon;
        }

        if (gameObject.activeSelf)
        {
            if (players.GetAxis("HoriMove") >= 0.4f)
            {
                if (currentDisplay < options.Length - 1) currentDisplay++;
                else currentDisplay = 0;
                selector.transform.localPosition = options[currentDisplay].transform.position;
            }
            if (players.GetAxis("HoriMove") <= 0.4f)
            {
                if (currentDisplay != 0) currentDisplay--;
                else currentDisplay = options.Length - 1;
                selector.transform.localPosition = options[currentDisplay].transform.position;
            }
        }
        if (players.GetButtonDown("AAction"))
        {
            Invoke(optionStrings[currentDisplay], 0);
        }
    }

    void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void Quit()
    {

    }

}
