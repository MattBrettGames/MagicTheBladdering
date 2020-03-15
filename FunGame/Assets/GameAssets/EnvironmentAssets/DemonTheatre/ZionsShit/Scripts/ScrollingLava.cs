using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingLava : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 0.5f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
