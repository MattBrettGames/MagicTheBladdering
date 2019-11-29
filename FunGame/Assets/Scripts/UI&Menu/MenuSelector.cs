using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuSelector : MonoBehaviour
{
    [Header("Options")]
    public GameObject[] options = new GameObject[2];
    public GameObject[] cursors = new GameObject[0];
   [HideInInspector] public GameObject cursor;

    [Header("Components")]
    public UniverseController universe;
    private Player player1;
    private Player player2;

    private bool inputOnCooldown;
    private int currentSel;

    void Start()
    {
        cursor.transform.position = options[currentSel].transform.position;
        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);      
        
        for (int i = 0; i < cursors.Length; i++)
        {
            cursors[i].SetActive(false);
        }
        cursor.SetActive(true);
    }

    void Update()
    {
        if (player1.GetAxis("VertMove") <= -0.4 && !inputOnCooldown || player2.GetAxis("VertMove") <= -0.4 && !inputOnCooldown)
        {
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
            if (currentSel >= options.Length - 1) { currentSel = 0; }
            else { currentSel++; }
            cursor.transform.position = options[currentSel].transform.position;
        }
        if (player1.GetAxis("VertMove") >= 0.4 && !inputOnCooldown || player2.GetAxis("VertMove") >= 0.4 && !inputOnCooldown)
        {
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
            if (currentSel <= 0) { currentSel = options.Length - 1; }
            else { currentSel--; }
            cursor.transform.position = options[currentSel].transform.position;
        }
        if (player1.GetButtonDown("AAction") || player2.GetButtonDown("AAction"))
        {
            Invoke(options[currentSel].name, 0);
        }
    }

    void Quit() { Application.Quit(); }
    void Duel() { universe.SelectedPlay(); }
    void Bios() { universe.SelectedBios(); }
    void Adventure() { universe.SelectedAdventure(); }
    void EndCooldown() { inputOnCooldown = false; }
}