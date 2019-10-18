using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : BlankMono
{
    public GameObject healthBar;
    public GameObject barBorder;
    public PlayerBase targetPlayer;
    public string thisPlayer;
    public List<Sprite> images = new List<Sprite>();
    public List<Vector3> imagePos = new List<Vector3>();
    public Image image;
    
    public void Start()
    {
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
    }

    public void SetStats(int imageInt)
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player" + thisPlayer).GetComponent<PlayerBase>();
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        image.sprite = images[imageInt];
        image.transform.position = imagePos[imageInt];
    }

    public void Update()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);
        if(targetPlayer.currentHealth < 0) { targetPlayer.currentHealth = 0; }
    }

}