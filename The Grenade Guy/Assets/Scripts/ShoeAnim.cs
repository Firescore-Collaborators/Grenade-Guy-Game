using System.Collections;
using UnityEngine;

public class ShoeAnim : MonoBehaviour
{
    public Animator RightShoeAnim;
    public Animator LeftShoeAnim;
    public Animator shoeAnim;
    public Animator BoxAnim;
    public Animator CamAnim;
    int touchCount;
    public GameObject CloseBox;
    public ParticleSystem effectLeft;
    public ParticleSystem effectRight;

    public ParticleSystem[] confetti;

    private void Start()
    {
        Cursor.visible = false;
        touchCount = 0;
    }
    public void PlayAnim()
    {
        touchCount++;
        if (touchCount == 1)
        {
            RightShoeAnim.Play("shoeRight");
            effectRight.Play();
        }
        if (touchCount == 2)
        {
            LeftShoeAnim.Play("SHoeLeft");
            effectLeft.Play();
        }

        if (touchCount == 3)
        {
            CamAnim.Play("CameraSwith");
            RightShoeAnim.Play("shoeRightOnfoot");
            LeftShoeAnim.Play("shoeLeftOnfoot");
        }

        if (touchCount == 4)
        {
            Data.isPlaceBox = true;
            StartCoroutine(playBox());
        }

        if (PlaceBox.isClosed)
        {
            Data.isShoeSelection = false;
            StartCoroutine(playConfetti());
        }

    }


    IEnumerator playBox()
    {
        shoeAnim.Play("ShoeToPlaceAnim");
        CamAnim.Play("CameraToBox");
        RightShoeAnim.Play("ShoeRightOnBox");
        LeftShoeAnim.Play("ShoeLeftOnBox");
        yield return new WaitForSeconds(2f);
        BoxAnim.Play("boxMove");
        shoeAnim.enabled = false;
        LeftShoeAnim.enabled = false;
        RightShoeAnim.enabled = false;
        CloseBox.SetActive(true);
    }




    IEnumerator playConfetti()
    {
        CamAnim.Play("CameraToBag");
        BoxAnim.Play("BoxToBag");
        yield return new WaitForSeconds(1f);
       
        StartCoroutine(switchScwne());
    }

    IEnumerator switchScwne()
    {
        foreach (ParticleSystem ps in confetti)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
  
}
