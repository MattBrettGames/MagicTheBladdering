using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacterPicker : MonoBehaviour
{

    public GameObjects[] chars = new GameObjects[2];
       
    void Start()
    {
        int rando = Random.Range(0, chars.Length);
        chars[rando].background.SetActive(true);
        chars[rando].characters.SetActive(true);
    }
    
    public struct GameObjects
    {
        public GameObject characters;
        public GameObject background;
    }

}
