using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuSelector : MonoBehaviour
{
    [Header("Options")]
    public GameObject[] options = new GameObject[4];
    public GameObject[] cursors = new GameObject[0];
    [HideInInspector] public GameObject cursor;
    RectTransform cursorTransform;
    RectTransform[] optionTransforms = new RectTransform[4];

    [Header("Components")]
    public UniverseController universe;
    private Player player1;
    private Player player2;

    private bool inputOnCooldown;
    private int currentSel;

    void Start()
    {
        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);

        for (int i = 0; i < cursors.Length; i++)
        {
            cursors[i].SetActive(false);
        }
        cursor.SetActive(true);
        cursorTransform = cursor.GetComponent<RectTransform>();

        for (int i = 0; i < options.Length; i++)
        {
            optionTransforms[i] = options[i].GetComponent<RectTransform>();
        }

        cursorTransform.anchoredPosition = optionTransforms[currentSel].anchoredPosition;
    }

    void Update()
    {
        if (player1.GetAxis("VertMove") <= -0.4 && !inputOnCooldown || player2.GetAxis("VertMove") <= -0.4 && !inputOnCooldown)
        {
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
            if (currentSel >= options.Length - 1) { currentSel = 0; }
            else { currentSel++; }
            cursorTransform.anchoredPosition = optionTransforms[currentSel].anchoredPosition;
        }
        if (player1.GetAxis("VertMove") >= 0.4 && !inputOnCooldown || player2.GetAxis("VertMove") >= 0.4 && !inputOnCooldown)
        {
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
            if (currentSel <= 0) { currentSel = options.Length - 1; }
            else { currentSel--; }
            cursorTransform.anchoredPosition = optionTransforms[currentSel].anchoredPosition;
        }
        if (player1.GetButtonDown("AAction") || player2.GetButtonDown("AAction"))
        {
            Invoke(options[currentSel].name, 0);
            cursorTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    void Quit() { Application.Quit(); }
    void Duel() { universe.SelectedPlay(); }
    void Bios() { universe.SelectedBios(); }
    void BugFix() { universe.Restart(); }
    void Options() { universe.SelectedOptions(); }
    void Adventure() { } //universe.SelectedAdventure(); }
    void EndCooldown() { inputOnCooldown = false; }
}