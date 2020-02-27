using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomCharacterPicker : MonoBehaviour
{
    public MenuSelector menu;
    AudioSource audioManager;
    [Space]
    public GameObjects[] chars = new GameObjects[2];
    public AudioClip[] themes = new AudioClip[0];

    void Start()
    {
        int rando = UnityEngine.Random.Range(0, chars.Length);
        chars[rando].characters.SetActive(true);
        menu.cursor = menu.cursors[rando];

        audioManager.volume = OptionMenuController.masterVolume * OptionMenuController.musicVolume;
        audioManager.PlayOneShot(themes[rando]);
    }

    [Serializable]
    public struct GameObjects
    {
        public GameObject characters;
    }

}