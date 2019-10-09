using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : BlankMono
{

    public GameObject healthBar;
    public GameObject barBorder;
    private PlayerBase targetPlayer;
    public string thisPlayer;
    public List<Sprite> images = new List<Sprite>();
    public Image image;

    public void SetStats(int imageInt)
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player" + thisPlayer).GetComponent<PlayerBase>();
        barBorder.transform.localScale = transform.localScale;
        image.sprite = images[imageInt];
    }

    public void Update()
    {
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50, 0.2f, 1);
        print(targetPlayer.currentHealth);
    }


}