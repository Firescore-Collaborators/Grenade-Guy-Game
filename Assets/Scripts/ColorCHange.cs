using UnityEngine;

public class ColorCHange : MonoBehaviour
{
    public Material[] measureColor;
    public static bool isGreenmain;

    private void Start()
    {
        isGreenmain = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("foot"))
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = measureColor[1];
            isGreenmain = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("foot"))
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = measureColor[0];
            isGreenmain = false;
        }
    }
}
