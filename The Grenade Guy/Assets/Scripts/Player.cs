using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Serializing fields
    [Header("Player")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] Animator animator;
    [SerializeField] List <Transform> waypoints;   //storing the waypoints where the player will go after killing
                                                   //current enemy
    [SerializeField] float mouseSensitivity = 150f;

    [Header("Others")]
    [SerializeField] GameObject grenade;
    [SerializeField] GameObject diamondConfettiePrefab;
    [SerializeField] Transform diamond;
    [SerializeField] bool level2 = false;       
   
    
    
    private bool isGrenadeActive = false;
    float xRotation = 0f;
    bool move = false;
    int start = 0;
    int end;
    Vector3 playerInititalPos;
    bool cameraFollow = false;
    GameObject g;
    // GameObject firstPoint;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;     //making the cursor invisible in the game screen
        end = waypoints.Count - 1;  
        playerInititalPos = transform.position;     //initial player position
    }

    // Update is called once per frame
    void Update()
    {
        //setting the "Grenade Throw" bool parameter in animator to true so that 
        // the player starts performing grenade throw animation
        if(isGrenadeActive)
        {
            animator.SetBool("Grenade Throw", isGrenadeActive);
            
        }

        //As soon as the animator completes the grnade toss clip, send him to standing still position
        if(isGrenadeActive && this.animator.GetCurrentAnimatorStateInfo(0).IsName("Toss Grenade"))
        {
            isGrenadeActive = false;
            animator.SetBool("Grenade Throw", isGrenadeActive);   //setting grenade throw parameter to false
           
        }

        //if player isn't running between waypoints, he can rotate on y axis based on mouse movement(basically looking sideways)
        if (!move)
            RotatePlayer();

        
        else
            Move();                       //initiate player running
       
    }

    //spawn new grenade after the wait time is over
    private IEnumerator SpawnGrenade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 pos;

        //if grenade is on level 2, spawn it at a certain positin with respect to player, otherwise at another
        //this could be done in a better way
        if (!level2)
            pos = new Vector3(-9.115f, 1.374f, -16.794f);
        else
            pos = new Vector3(6.21f, 1.39f, -0.66f);
        var offset = pos - playerInititalPos;
        //Debug.Log(offset);
         g = Instantiate(grenade, transform.position + offset, Quaternion.identity);
       // transform.parent = g.transform;
    }

    private void Move()
    {
        if(!cameraFollow)
        {
            //make camera follow the player along z axis
            FindObjectOfType<Camera>().GetComponent<CameraMovement>().SetCamerMove();
            cameraFollow = true;
        }

        //making the player run from start point to end point
        if(start <= end)
        {
            
            Vector3 dir;

            var targetPosition = waypoints[start].transform.position;
            dir = targetPosition - transform.parent.position;
            Quaternion rotation = Quaternion.LookRotation(dir);

            //moving towards the next waypoint
            transform.parent.position = Vector3.MoveTowards
                      (transform.parent.position, targetPosition, moveSpeed * Time.deltaTime);

            //rotating toward in the direction of running
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rotation, rotateSpeed * Time.deltaTime);

            if (transform.parent.position == targetPosition)
            {
                start++;
                // iterating start pos when player has reached the current target waypoint
            }
        }

        else
        {
            move = false;
            FindObjectOfType<Camera>().GetComponent<CameraMovement>().SetCamerMove();
            cameraFollow = false;
            animator.SetBool("Run", false);              //stopping the run animation
            StartCoroutine(SpawnGrenade(0f));           //spawn new grenade when player has reached at the new location

            //at level 1 on reching the last location instantiate the diamond confettie  and start
            //dance animation
            if (end == 9 && !level2)
            {
                move = false;
                FindObjectOfType<Camera>().GetComponent<CameraMovement>().SetFinalCameraMove();
                StartCoroutine(InstantiateDiamond());
                animator.SetBool("Dance", true);
            }

            //at level 2 on reching the last location instantiate the diamond confettie  and start
            //dance animation
            else if (end == 13 && level2)
            {
                move = false;
                FindObjectOfType<Camera>().GetComponent<CameraMovement>().SetFinalCameraMove();
                StartCoroutine(InstantiateDiamond());          //instantiate diamond aftter certain time
                animator.SetBool("Dance", true);
            }
        }
        
    }

    public void FreezeAnimation()
    {
        animator.SetFloat("Speed", 0f);
    }

    public void ContinueAnimation()
    {
        animator.SetFloat("Speed", 1.5f);
       // transform.parent = null;
    }

    private IEnumerator InstantiateDiamond()
    {
        yield return new WaitForSeconds(1.2f);
        GameObject diamondConfettie = Instantiate(diamondConfettiePrefab, diamond.position, Quaternion.identity);
        Destroy(diamondConfettie, 1f);
    }

    public void InstantiateGrenade(float waitTime)
    {
        StartCoroutine(SpawnGrenade(waitTime));
    }

    public void SetMove(int start, int end)
    {
        StartCoroutine(PlayerMove(start , end));
    }

    IEnumerator PlayerMove(int start, int end)
    {
        yield return new WaitForSeconds(0.9f);
        move = true;
        this.start = start;
        this.end = end;
        animator.SetBool("Run", true);
    }


    private void RotatePlayer()
    {
         float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
         float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

         xRotation -= mouseY;
         xRotation = Mathf.Clamp(xRotation, -90f, 90f);

         //transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
         transform.parent.Rotate(Vector3.up * -mouseX);
       
        // transform.rotation = Quaternion.LookRotation(new Vector3(grenade.transform.position.x, transform.position.y, grenade.transform.position.z));
       
    }

    public void SetGrenadeActive(bool val)
    {
        isGrenadeActive = val;
    }
}
