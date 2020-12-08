using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static bool isShoeSelection;
    public static bool isPlaceBox;

    private void Awake()
    {
        isShoeSelection = true;
        isPlaceBox = false;
    }
}
