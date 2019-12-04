using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Player players;
    Player player2;

    [SerializeField] GameObject[] options;
    [Space]
    [SerializeField] string[] optionStrings;
    [Space]
    [SerializeField] GameObject selector;
    int currentDisplay;
    [Space]
    [SerializeField] GameObject visuals;

    void Start()
    {
        visuals.SetActive(false);
        players = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);
    }

    void Update()
    {
        print("I exist");
        if (players.GetButtonDown("Pause") || player2.GetButtonDown("Pause"))
        {
            print("Pause button has been received");
            visuals.SetActive(true);
            Time.timeScale = Mathf.Epsilon;
        }

        if (visuals.activeSelf)
        {
            if (players.GetAxis("VertMove") >= 0.4f|| player2.GetAxis("VertMove") >= 0.4f)
            {
                if (currentDisplay < options.Length - 1) currentDisplay++;
                else currentDisplay = 0;
                selector.transform.localPosition = options[currentDisplay].transform.position;
            }
            if (players.GetAxis("VertMove") <= 0.4f || player2.GetAxis("VertMove") <= 0.4f)
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
        visuals.SetActive(false);
        Time.timeScale = 1;
    }

    void Quit()
    {
        UniverseController universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.ReturnToMenu();
    }

}
