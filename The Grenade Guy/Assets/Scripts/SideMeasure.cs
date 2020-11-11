using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMeasure : MonoBehaviour
{
    public Material[] measureColor;
    public static bool isGreenSide;

    private void Start()
    {
      isGreenSide = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("sidefoot"))
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = measureColor[1];
            isGreenSide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("sidefoot"))
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = measureColor[0];
            isGreenSide = false;
        }
    }
}
