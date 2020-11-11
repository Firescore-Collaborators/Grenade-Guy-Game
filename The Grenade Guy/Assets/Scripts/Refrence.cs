using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrence : MonoBehaviour
{
   public GameObject startRefrence;

    private void OnTriggerEnter(Collider other)
    {
        if(!PlaceBox.isClosed)
        {
            StartCoroutine(showrefrence());
        }
    }

    IEnumerator showrefrence()
    {
        yield return new WaitForSeconds(1f);
        startRefrence.SetActive(true);
    }
}
