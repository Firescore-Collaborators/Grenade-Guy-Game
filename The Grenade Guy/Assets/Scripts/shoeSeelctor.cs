using UnityEngine;

public class shoeSeelctor : MonoBehaviour
{
    public GameObject Right;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("shoe"))
        {
            // other.GetComponent<Rigidbody>().MovePosition(Right.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("shoe"))
        {
            other.transform.position = Vector3.Lerp(transform.position, Right.transform.position, 0.00001f);
        }
    }
}