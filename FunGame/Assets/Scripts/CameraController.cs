using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BlankMono
{
    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Transform player4;
    private Vector3 offset = new Vector3(0, 25, 0);

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;
        //player3 = GameObject.FindGameObjectWithTag("Player3").transform;
        //player4 = GameObject.FindGameObjectWithTag("Player4").transform;
    }

    void Update()
    {
        Vector3 targetPos = player1.position - player2.position;
        transform.position = targetPos + offset;
    }
}
