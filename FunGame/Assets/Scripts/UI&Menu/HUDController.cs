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

    public void SetStats(int imageInt)
    {
        targetPlayer = playerBase.GetComponentInParent<PlayerBase>();
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);

        image.SetActive(false);
        image = images[imageInt];
        image.SetActive(true);

        image.transform.position = imagePos[imageInt];
    }

    public void Update()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);
        if (targetPlayer.currentHealth < 0) { targetPlayer.currentHealth = 0; }
    }
}