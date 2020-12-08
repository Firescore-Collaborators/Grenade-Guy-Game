using System.Collections;
using UnityEngine;

public class PlaceBox : MonoBehaviour
{
    public GameObject Shoe;
    RaycastHit hit;
    Ray ray;
    public Animator animBox;
    public static bool isClosed;

    private void Start()
    {
        isClosed = false;
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) && Data.isPlaceBox)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);

                if (hit.collider.gameObject.CompareTag("shoe"))
                {
                    hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    hit.collider.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }
            }
        }
    }

    public void CloseBox()
    {
        StartCoroutine(CloseBoxIE());
    }

    IEnumerator CloseBoxIE()
    {
        isClosed = true;
        animBox.Play("BoxClose");
        yield return new WaitForSeconds(1.5f);
        Shoe.SetActive(false);
    }
}
