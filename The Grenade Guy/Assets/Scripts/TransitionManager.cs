using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Animator TransitionAnim;
    public Animator BagAnim;
    public Animator PickUpAnim;
    public GameObject bag;
    public GameObject refrence;
    public static bool playanim;

    private void Start()
    {
        TransitionAnim.Play("Fadeout");
        playanim = false;
        if(PlaceBox.isClosed)
        {
            refrence.SetActive(false);
            bag.SetActive(true);
            BagAnim.Play("BagPickUp");
            PickUpAnim.Play("Picking Up Object");
        }
    }

    public void Update()
    {
        if (playanim)
        {
            LoadNextLevel();
            playanim = false;
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(Loadasync());

    }

    IEnumerator Loadasync()
    {
        TransitionAnim.Play("FadeIn");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       AsyncOperation oper = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while(oper.isDone)
        {
            TransitionAnim.Play("FadeOut");
        }
    }

    public void startTransition()
    {
        playanim = true;
    }
}
