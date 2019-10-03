using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseTrap : BlankMono
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag != transform.tag)
        {
            other.gameObject.GetComponent<PlayerBase>().GainCurse(15);
        }
    }
}
