using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBackgroundController : MonoBehaviour
{

    [SerializeField] private GameObject[] victoryBackgrounds = new GameObject[5];


    public void SetBackground(string objectName)
    {
        for (int i = 0; i < victoryBackgrounds.Length; i++)
        {
            if (victoryBackgrounds[i].name == objectName)
                victoryBackgrounds[i].SetActive(true);
            else
                victoryBackgrounds[i].SetActive(false);
        }
    }


}
