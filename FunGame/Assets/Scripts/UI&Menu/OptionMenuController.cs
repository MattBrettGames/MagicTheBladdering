using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    Player player1;
    Player player2;
    int currentDisplay;

    [SerializeField] GameObject cursor;
    [SerializeField] Slider[] sliders = new Slider[0];
    [SerializeField] Text[] displays = new Text[0];

    [Header("Values")]
    [SerializeField] public static float masterVolume;
    [SerializeField] public static float musicVolume;
    [SerializeField] public static float sfxVolume;

    bool inputOnCooldown;
    UniverseController uni;
//    AudioManager aud;

    void Start()
    {

        uni = GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>();
     /*   aud = uni.gameObject.GetComponentInChildren<AudioManager>();

        sliders[0].value = aud.masterVolume;
        sliders[1].value = aud.musicVolume;
        sliders[2].value = aud.sfxVolume;
        */
        for (int i = 0; i < sliders.Length; i++)
        {
            displays[i].text = Mathf.RoundToInt(sliders[i].value * 100) + "%";
        }

        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);
    }

    void Update()
    {
        cursor.transform.position = new Vector3(500, sliders[currentDisplay].transform.position.y, 0);

        if (player1.GetAxis("VertMove") <= -0.4f && !inputOnCooldown || player2.GetAxis("VertMove") <= -0.4f && !inputOnCooldown)
        {
            if (currentDisplay < sliders.Length - 1) currentDisplay++;
            else currentDisplay = 0;
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
        }
        if (player1.GetAxis("VertMove") >= 0.4f && !inputOnCooldown || player2.GetAxis("VertMove") >= 0.4f && !inputOnCooldown)
        {
            if (currentDisplay != 0) currentDisplay--;
            else currentDisplay = sliders.Length - 1;
            Invoke("EndCooldown", 0.2f);
            inputOnCooldown = true;
        }

        if (currentDisplay == 0 || currentDisplay == 1 || currentDisplay == 2)
        {
            if (Mathf.Abs(player1.GetAxis("HoriMove") + player2.GetAxis("HoriMove")) > 0.3f)
                sliders[currentDisplay].value += (player1.GetAxis("HoriMove") + player2.GetAxis("HoriMove")) * Time.deltaTime * 0.3f;

            if (sliders[currentDisplay].value < 0) sliders[currentDisplay].value = 0;
            if (sliders[currentDisplay].value > 1) sliders[currentDisplay].value = 1;

            displays[currentDisplay].text = Mathf.RoundToInt(sliders[currentDisplay].value * 100) + "%";

        }

        if (Input.GetButtonDown("AllAButton")) { Save(); }
    }

    void Save()
    {
        masterVolume = sliders[0].value;
        musicVolume = sliders[1].value;
        sfxVolume = sliders[2].value;

        /*
        aud.UpdateSettings();

        aud.masterVolume = masterVolume;
        aud.musicVolume = musicVolume;
        aud.sfxVolume = sfxVolume;
        */
    }

    void EndCooldown() { inputOnCooldown = false; }

}