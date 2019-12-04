using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomCharacterPicker : MonoBehaviour
{
    public MenuSelector menu;
    [Space]
    public GameObjects[] chars = new GameObjects[2];


    void Start()
    {
        int rando = UnityEngine.Random.Range(0, chars.Length);
        chars[rando].characters.SetActive(true);
        menu.cursor = menu.cursors[rando];
    }

    [Serializable]
    public struct GameObjects
    {
        public GameObject characters;
    }

}
