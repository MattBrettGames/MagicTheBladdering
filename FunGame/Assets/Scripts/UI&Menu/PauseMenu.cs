using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] Color activeColour;
    [SerializeField] Color selectedColour;
    [SerializeField] Vector3 sizeChange;
    Player players;
    Player player2;
    [SerializeField] GameObject[] options;
    [Space]
    [SerializeField] string[] optionStrings;
    [Space]
    int currentDisplay = 0;
    [Space]
    [SerializeField] GameObject visuals;
    private PlayerBase playerCode1;
    private PlayerBase playerCode2;
    bool inputOnCooldown;
    List<Text> texts = new List<Text>();
    bool ready;

    void Start()
    {
        StartCoroutine(TrueStart());
        visuals.SetActive(false);
    }

    IEnumerator TrueStart()
    {
        yield return new WaitForSecondsRealtime(4.3f);
        ready = true;
        print("Pause is ready");
        players = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);

        GameObject everything = GameObject.Find("EverythingYouNeed");

        playerCode1 = everything.GetComponentsInChildren<PlayerBase>()[0];
        playerCode2 = everything.GetComponentsInChildren<PlayerBase>()[1];

        for (int i = 0; i < options.Length; i++) { texts.Add(options[i].GetComponent<Text>()); }
        options[currentDisplay].transform.localScale += sizeChange;
        texts[currentDisplay].color = activeColour;
    }


    void Update()
    {
        if (players.GetButtonDown("Pause") || player2.GetButtonDown("Pause"))
        {
            if (ready)
            {
                visuals.SetActive(true);
                Time.timeScale = 0;
                playerCode1.BeginActing();
                playerCode2.BeginActing();
            }
        }

        if (visuals.activeSelf)
        {
            if ((players.GetAxis("VertMove") <= -0.4f || player2.GetAxis("VertMove") <= -0.4f) && !inputOnCooldown)
            {
                texts[currentDisplay].color = Color.white;
                options[currentDisplay].transform.localScale = Vector3.one - sizeChange;

                if (currentDisplay < options.Length - 1) currentDisplay++;
                else currentDisplay = 0;

                options[currentDisplay].transform.localScale = Vector3.one + sizeChange;
                texts[currentDisplay].color = activeColour;

                StartCoroutine(EndCooldown());
                inputOnCooldown = true;
            }
            if ((players.GetAxis("VertMove") >= 0.4f || player2.GetAxis("VertMove") >= 0.4f) && !inputOnCooldown)
            {
                texts[currentDisplay].color = Color.white;
                options[currentDisplay].transform.localScale = Vector3.one - sizeChange;

                if (currentDisplay != 0) currentDisplay--;
                else currentDisplay = options.Length - 1;

                options[currentDisplay].transform.localScale = Vector3.one + sizeChange;
                texts[currentDisplay].color = activeColour;

                StartCoroutine(EndCooldown());
                inputOnCooldown = true;
            }
            if (players.GetButtonDown("AAction") || player2.GetButtonDown("AAction"))
            {
                texts[currentDisplay].color = selectedColour;
                options[currentDisplay].transform.localScale = Vector3.one - (sizeChange * 2);
                Invoke(optionStrings[currentDisplay], 0);
                currentDisplay = 0;
            }
        }
    }

    IEnumerator EndCooldown()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        inputOnCooldown = false;
    }

    void Resume()
    {
        visuals.SetActive(false);
        Time.timeScale = 1;
        playerCode1.EndActing();
        playerCode2.EndActing();
    }

    void Fix()
    {
        Resume();
        playerCode1.gameObject.transform.position = new Vector3(-15, 0, 0);
        playerCode2.gameObject.transform.position = new Vector3(15, 0, 0);
    }

    void Quit()
    {
        Time.timeScale = 1;
        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu");
    }
}