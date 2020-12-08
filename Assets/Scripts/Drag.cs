using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;
    RaycastHit hit;
    Ray ray;
    public float height;

    private void Update()
    {
        if (Input.touchCount>0)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);

                if(hit.collider.gameObject.CompareTag("shoe"))
                {
                    GameObject player = hit.collider.gameObject;
                    Rigidbody rb = player.GetComponent<Rigidbody>();
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector3 tempos = player.transform.position;
                       // tempos.y = 0.5f;
                        player.transform.position = tempos;
                        rb.useGravity = false;
                        screenPoint = Camera.main.WorldToScreenPoint(player.transform.position);
                        offset = player.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector3 tempos = player.transform.position;
                        tempos.y = 0.5f;
                        player.transform.position = tempos;

                        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

                        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                        player.transform.position = curPosition;

                    }

                    if(touch.phase == TouchPhase.Ended)
                    {
                        rb.useGravity = true;
                    }
                }
            }
        }
    }


    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        gameObject.transform.position = curPosition;
    }
}
