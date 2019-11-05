using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomCharacterPicker : MonoBehaviour
{

    public GameObjects[] chars = new GameObjects[2];

    void Start()
    {
        int rando = UnityEngine.Random.Range(0, chars.Length);
        chars[rando].characters.SetActive(true);
        chars[rando].background.SetActive(true);
    }

    [Serializable]
    public struct GameObjects
    {
        public GameObject characters;
        public GameObject background;
    }

}
