using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : BlankMono
{
    public GameObject healthBar;
    public GameObject barBorder;

    private PlayerBase targetPlayer;

    public GameObject playerBase;
    public string thisPlayerInt;
    public GameObject[] images = new GameObject[2];
    public List<Vector3> imagePos = new List<Vector3>();
    public GameObject image;


    [Header("Character Specific")]
    public GameObject commonBar;
    [Space]
    public GameObject songCounter;


    public void Start()
    {
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
    }

    public void SetStats(int imageInt)
    {
        //targetPlayer = GameObject.FindGameObjectWithTag("Player" + thisPlayerInt).GetComponent<PlayerBase>();
        targetPlayer = playerBase.GetComponentInParent<PlayerBase>();
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);

        commonBar.transform.localScale = new Vector3(targetPlayer.AccessUniqueFeature(1) * 20, 0.2f, 1);

        if (targetPlayer.name.Contains("ongbir"))
        {
            commonBar.SetActive(true);
            songCounter.SetActive(true);
            songCounter.transform.localScale = new Vector3(targetPlayer.AccessUniqueFeature(1)*0.5f, 0.2f, 1);
        }

        image.SetActive(false);
        image = images[imageInt];
        image.SetActive(true);

        image.transform.position = imagePos[imageInt];
    }

    public void Update()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);
        if (targetPlayer.currentHealth < 0) { targetPlayer.currentHealth = 0; }

        commonBar.transform.localScale = Vector3.Lerp(commonBar.transform.localScale, new Vector3(targetPlayer.AccessUniqueFeature(1)*0.4f, 0.2f, 1), 0.3f);
    }
}