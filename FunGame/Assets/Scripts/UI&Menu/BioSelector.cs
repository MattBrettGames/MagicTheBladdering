using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class BioSelector : BlankMono
{

    public List<GameObject> displays = new List<GameObject>();
    public List<GameObject> movesetDisplays = new List<GameObject>();
    private int currentDisplay;
    private bool inputCooldown;
    private bool onMovesets;

    Player player;
    Player player2;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);

        displays[currentDisplay].SetActive(true);
        onMovesets = false;
    }

    void LateUpdate()
    {
        if (player.GetButtonDown("AAction") || player2.GetButtonDown("AAction"))
        {
            onMovesets = !onMovesets;
        }

        if ((player.GetAxis("VertMove") <= -0.4f && !inputCooldown) || (player2.GetAxis("VertMove") <= -0.4f && !inputCooldown))
        {
            onMovesets = false;
            displays[currentDisplay].SetActive(false);

            if (currentDisplay < displays.Count - 1) currentDisplay++;
            else currentDisplay = 0;

            displays[currentDisplay].SetActive(true);
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
        if ((player.GetAxis("VertMove") >= 0.4f && !inputCooldown)||(player2.GetAxis("VertMove") >= 0.4f && !inputCooldown))
        {
            onMovesets = false;
            displays[currentDisplay].SetActive(false);

            if (currentDisplay != 0) currentDisplay--;
            else currentDisplay = displays.Count - 1;

            displays[currentDisplay].SetActive(true);
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
    }

    private void Update()
    {
        for(int i = 0; i < movesetDisplays.Count; i++)
        {
            movesetDisplays[i].SetActive(false);
        }
        movesetDisplays[currentDisplay].SetActive(onMovesets);
    }

    void EndCooldown()
    {
        inputCooldown = false;
    }
}
