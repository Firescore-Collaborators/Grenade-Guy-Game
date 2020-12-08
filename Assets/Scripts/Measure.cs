using UnityEngine;

public class Measure : MonoBehaviour
{
    int indexMeasure;
    public Animator horiAnim;
    public Animator SideAnim;
    private Vector3 horizontalPos, sidepos;
    public GameObject horizontal;
    public GameObject side;
    public Animator TextGoodJob;
    public Animator TextTryAgain;


    private void Awake()
    {
        indexMeasure = 1;
        SideAnim.enabled = false;
    }


    public void clickCounter()
    {

        indexMeasure++;
        if (indexMeasure == 1)
        {
            horiAnim.Play("knobmeasuring");
        }


        if (indexMeasure == 2)
        {
            horiAnim.enabled = false;
            SideAnim.enabled = true;
            if(ColorCHange.isGreenmain)
            {
                TextGoodJob.Play("GoodJobText");
            }
            else
            {
                TextTryAgain.Play("GoodJobText");
            }
        }


         if (indexMeasure == 3)
        {
            SideAnim.enabled = false;
            if (SideMeasure.isGreenSide)
            {
                TextGoodJob.Play("GoodJobText");
            }
            else
            {
                TextTryAgain.Play("GoodJobText");
            }
        }


         if(indexMeasure == 4)
         {
            TransitionManager.playanim = true;
         }

    }
}
