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
    AudioSource thisSource;

    void Start()
    {
        int rando = UnityEngine.Random.Range(0, chars.Length);
        chars[rando].characters.SetActive(true);
        menu.cursor = menu.cursors[rando];
        thisSource = gameObject.AddComponent<AudioSource>();

        if (thisSource.clip != themes[rando] || thisSource.clip == null)
        {
            thisSource.Stop();
            thisSource.clip = themes[rando];
            thisSource.Play();
        }

        //audioManager.volume = OptionMenuController.masterVolume * OptionMenuController.musicVolume;
        //audioManager.PlayOneShot(themes[rando]);
    }

    [Serializable]
    public struct GameObjects
    {
        public GameObject characters;
    }

}