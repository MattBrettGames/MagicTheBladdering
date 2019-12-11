using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Player players;
    Player player2;
    [SerializeField] GameObject[] options;
    [Space]
    [SerializeField] string[] optionStrings;
    [Space]
    [SerializeField] GameObject selector;
    int currentDisplay = 0;
    [Space]
    [SerializeField] GameObject visuals;
    private PlayerBase playerCode1;
    private PlayerBase playerCode2;
    bool inputOnCooldown;

    void Start()
    {
        visuals.SetActive(false);
        players = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);

        playerCode1 = GameObject.Find("Player1Base").GetComponentInParent<PlayerBase>();
        playerCode2 = GameObject.Find("Player2Base").GetComponentInParent<PlayerBase>();
    }

    void Update()
    {
        if (players.GetButtonDown("Pause") | player2.GetButtonDown("Pause"))
        {
            visuals.SetActive(true);
            playerCode1.BeginActing();
            playerCode2.BeginActing();
            Time.timeScale = Mathf.Epsilon;
        }

        if (visuals.activeSelf)
        {
            if ((players.GetAxis("VertMove") >= 0.4f | player2.GetAxis("VertMove") >= 0.4f) && !inputOnCooldown)
            {
                if (currentDisplay < options.Length - 1) currentDisplay++;
                else currentDisplay = 0;
                StartCoroutine(EndCooldown());
                inputOnCooldown = true;
            }
            if ((players.GetAxis("VertMove") <= -0.4f | player2.GetAxis("VertMove") <= -0.4f) && !inputOnCooldown)
            {
                if (currentDisplay != 0) currentDisplay--;
                else currentDisplay = options.Length - 1;
                StartCoroutine(EndCooldown());
                inputOnCooldown = true;
            }

            selector.transform.position = options[currentDisplay].transform.position;

            if (players.GetButtonDown("AAction"))
            {
                Invoke(optionStrings[currentDisplay], 0);
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

    void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}
