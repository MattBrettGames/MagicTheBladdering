using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    Player player1;
    Player player2;
    GameObject currentElement;

    [SerializeField] Slider[] sliders = new Slider[0];

    //-- Settings --\\
    bool isPost;
    float masterVolume;
    float musicVolume;
    float sfxVolume;

    void Start()
    {
        player1 = ReInput.players.GetPlayer(1);
        player2 = ReInput.players.GetPlayer(2);
    }

    void Update()
    {

        if (player1.GetAxis("VertMove") >= 0.4f || player2.GetAxis("VertMove") >= 0.4f)
        {
            





        }
        if (player1.GetAxis("VertMove") <= -0.4f || player2.GetAxis("VertMove") <= -0.4f)
        {






        }





    }

    void Save()
    {

        if (sliders[0].value == 1) { isPost = true; }
        else { isPost = false; }



        UniverseController uni = GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>();

        uni.isPostProcessing = isPost;

        AudioManager aud = uni.gameObject.GetComponentInChildren<AudioManager>();
        aud.mastervolume = masterVolume;
        aud.musicvolume = musicVolume;
        aud.sfxvolume = sfxVolume;


    }


}