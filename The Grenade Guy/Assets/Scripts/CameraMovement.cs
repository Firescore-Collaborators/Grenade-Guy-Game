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
    Quaternion initialRotation;
    bool move = false;
    bool finalmove = false;
    Vector3 finalPos;
   [SerializeField]Quaternion finalRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
        zPos = player.position.z;
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
            finalPos = new Vector3(3.24f, 2.07f, 25.7f);
            finalRotation = Quaternion.EulerAngles(0.4f, 3.1f, 0f);
        }
    }

    // Update is called once per frame

    private void Update()
    {
       // Debug.Log(transform.rotation.ToEulerAngles());

        //when player is running make the camera follow him aling the z axis
        if(move)
        {
            var zcurr = player.position.z;
            var zoff = zcurr - zPos;
            transform.position += new Vector3(0, 0, zoff);
            zPos = zcurr;
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

}

