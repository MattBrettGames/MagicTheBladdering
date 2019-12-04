using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterRotater : MonoBehaviour
{
    Player player;
    [SerializeField] int thisPInt;

    [SerializeField] float speed;
    void Start()
    {
        player = ReInput.players.GetPlayer(thisPInt);
    }

    void Update()
    {
        transform.Rotate(0, 1 * speed, 0);
    }
}
