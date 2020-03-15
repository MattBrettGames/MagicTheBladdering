using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRiseAndRotate : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed = 3f;
    [SerializeField]
    private float riseSpeed = 2f;
    [SerializeField]
    private float maxHeight;
    [SerializeField]
    private float minHeight;

    private bool up;
    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, minHeight, transform.localPosition.z);
        up = true;
    }

  
    void Update()
    {
        if((maxHeight - transform.localPosition.y) <= 4f)
        {
            up = false;
        }
        if ((transform.localPosition.y - minHeight) <= 4f)
        {
            up = true;
        }
        if (up)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(transform.localPosition.x, maxHeight, transform.localPosition.z), riseSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(transform.localPosition.x, minHeight, transform.localPosition.z), riseSpeed * Time.deltaTime);
        }

        transform.eulerAngles += new Vector3(0, rotSpeed * Time.deltaTime, 0);
       // Debug.Log(maxHeight - transform.localPosition.y);
    }
}
