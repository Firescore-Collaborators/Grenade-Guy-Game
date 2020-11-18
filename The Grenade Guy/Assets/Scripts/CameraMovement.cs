using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    //Serialized fields
    [Header("Camera")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 100f;

    [Header("Others")]
    [SerializeField] bool level2 = false;
    public Transform player;

    float zPos;
    float xPos;
    Quaternion initialRotation;
    bool move = false;
    bool finalmove = false;
    Vector3 finalPos;
    Quaternion finalRotation;

    bool changeX = false;
    bool moveToInitialX = true;
    Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        initialRotation = transform.rotation;
        zPos = player.position.z;
        xPos = player.position.x;
       // Debug.Log(zPos);

        //for level 1 final position and rotation for camera
        if (!level2)
        {
            finalPos = new Vector3(-9.78f, 2.14f, 8.39f);
            finalRotation = Quaternion.EulerAngles(0.6f, 3.1f, 0f);
        }

        //for level 2 final position and rotation for camera
        else
        {
            finalPos = new Vector3(3.07f, 2.43f, 27.67f);
            finalRotation = Quaternion.EulerAngles(0.2f, 3.1f, 0f);
        }
    }

    // Update is called once per frame

    private void Update()
    {
        //Debug.Log(transform.rotation.ToEulerAngles());

        //when player is running make the camera follow him aling the z axis
        if(move)
        {
            if(!changeX)
            {
                if(level2 && !moveToInitialX)
                {
                    var zcurr = player.position.z;
                    var zoff = zcurr - zPos;
                    transform.position += new Vector3(0, 0, zoff);
                    zPos = zcurr;

                }

                else if (level2 && moveToInitialX)
                {
                    var xcurr = player.position.x;
                    var zcurr = player.position.z;
                    
                    var xoff = xcurr - xPos;
                    var zoff = zcurr - zPos;

                    if(xcurr >= initialPos.x)
                    {
                        xoff = 0;
                    }
                    transform.position += new Vector3(xoff/3, 0, zoff);
                    zPos = zcurr;
                    xPos = xcurr;
                }
            }
            else
            {
                if(level2 && !moveToInitialX)
                {
                    var xcurr = player.position.x;
                    var zcurr = player.position.z;

                    var xoff = xcurr - xPos;
                    var zoff = zcurr - zPos;
                    transform.position += new Vector3(xoff/3, 0, zoff);
                    zPos = zcurr;
                    xPos = xcurr;

                }


            }
           

        }

        //lerping to the final position and rotation when player has reached the final location
        if(finalmove)
        {
            Vector3 dir;

           // var targetPosition = waypoints[start].transform.position;
            dir = finalPos - transform.position;
            Quaternion rotation = Quaternion.LookRotation(dir);

            transform.position = Vector3.MoveTowards
                    (transform.position, finalPos, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, rotateSpeed * Time.deltaTime);
        }
    }

    
    public void SetCamerMove()
    {
       
        move = !move;
    }

    
    public void SetFinalCameraMove()
    {
        finalmove = true;
    }

    public void SetChangeX(bool val)
    {
        changeX = val;
        moveToInitialX = !moveToInitialX;
    }

}

