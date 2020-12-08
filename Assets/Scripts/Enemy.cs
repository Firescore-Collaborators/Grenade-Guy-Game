using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] Animator animator;
    [SerializeField] float minSpeed = 1f;
    [SerializeField] Vector3 enemyRotaion1;
    [SerializeField] Vector3 enemyRotaion2;

    float initialRotateSpeed;
    int curr;
    int end;
    bool turn = false;
    int x = -1;
    // Start is called before the first frame update
    void Start()
    {
        initialRotateSpeed = rotateSpeed;
        curr = 0;
        end = waypoints.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!turn)
            Move();
        // Debug.Log(' ');
        else
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Right Turn"))
            {
                turn = false;
                animator.SetBool("Turn", turn);
            }
        }

    }

    private void Move()
    {

        if (curr <= end)
        {
            //Vector3 dir;

            var targetPosition = waypoints[curr].transform.position;
            //  dir = targetPosition - transform.position;
            //  Quaternion rotation = Quaternion.LookRotation(dir);

            //moving towards the next waypoint
            transform.position = Vector3.MoveTowards
                      (transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //rotating toward in the direction of running
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                curr++;
                // iterating start pos when player has reached the current target waypoint
            }
        }

        else
        {
            turn = true;
            animator.SetBool("Turn", turn);
            StartCoroutine(Rotate(-1));


            var temp = waypoints[0];
            waypoints[0] = waypoints[end];
            waypoints[end] = temp;
            curr = 0;
        }
    }

    IEnumerator Rotate(int y)
    {
        x = x * y;
        rotateSpeed = initialRotateSpeed;
        //  Debug.Log(x);
        if (x > 0)
        {

            Quaternion target = transform.rotation * Quaternion.Euler(enemyRotaion1.x, enemyRotaion1.y, enemyRotaion1.z); //store the target angle we are trying to reach
            float amountRotated = 0; //keep track of the amount we have rotated
            while (amountRotated <= 180)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotateSpeed);
                amountRotated += rotateSpeed; //the 3rd parameter is the degrees change, so we can keep track of the amount of rotation by adding it each time
                rotateSpeed = Mathf.Lerp(rotateSpeed, minSpeed, .3f); //slowly reduce the speed, so the rotation has a more natural feel, slowing it down towards the end
                yield return null;
            }
        }

        else
        {
            // Debug.Log("reverse rotation");
            //Debug.Log(transform.rotation);
            Quaternion target = Quaternion.Euler(enemyRotaion2.x, enemyRotaion2.y, enemyRotaion2.z); //store the target angle we are trying to reach
                                                                                                     // Debug.Log(target);
            float amountRotated = 180; //keep track of the amount we have rotated
            while (amountRotated >= 0)
            {
                //Debug.Log(amountRotated);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotateSpeed);
                // Debug.Log(transform.rotation.ToEulerAngles());
                amountRotated -= rotateSpeed; //the 3rd parameter is the degrees change, so we can keep track of the amount of rotation by adding it each time
                rotateSpeed = Mathf.Lerp(rotateSpeed, minSpeed, .3f); //slowly reduce the speed, so the rotation has a more natural feel, slowing it down towards the end
                yield return null;
            }
        }

    }
}
