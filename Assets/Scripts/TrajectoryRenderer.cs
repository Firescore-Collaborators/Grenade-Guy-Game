using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
   
   // [SerializeField] TrajectoryRenderer trajectory;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
    }

   

    public void Dest()
    {
        Destroy(gameObject);    
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed, float reboundTime, Transform impactPoint)
    {
        Vector3[] points = new Vector3[100];       
       lineRenderer.positionCount = points.Length;

        //float shortesDistance = 1000f;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

           
           
            if (points[i].y < 0)
            {
                // Instantiate(impactPoint, points[i], Quaternion.identity);
                impactPoint.position = new Vector3(points[i-1].x, 0.29f, points[i-1].z);
                lineRenderer.positionCount = i + 1;
                break;
            }
        }

        lineRenderer.SetPositions(points);
    }

  
}
