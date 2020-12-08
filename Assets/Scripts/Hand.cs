using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Vector3 handpos;
    void Update()
    {
        if(Data.isShoeSelection)
        {
            handpos = Input.mousePosition;
            handpos.y = handpos.y - 25f;
            transform.position = handpos;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
