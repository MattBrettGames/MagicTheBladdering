using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacterPicker : MonoBehaviour
{

    public GameObject[] chars = new GameObject[2];
       
    void Start()
    {
        chars[Random.Range(0, chars.Length)].SetActive(true);
    }


}
