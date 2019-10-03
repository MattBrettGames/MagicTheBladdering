using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BlankMono
{
    public Transform player1;
    public Transform player2;
    
    private Vector3 offset;

    private void Update()
    {
        float up = Vector3.Distance(player1.position, player2.position);

        Vector3 pos = player2.position - player1.position;

        transform.LookAt(pos);

        transform.position = new Vector3 (0, up ,-15) + pos;
    }
}