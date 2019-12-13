using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameToTag : MonoBehaviour
{
    void Start() 
    {
        gameObject.tag = gameObject.name; 
    }
}
