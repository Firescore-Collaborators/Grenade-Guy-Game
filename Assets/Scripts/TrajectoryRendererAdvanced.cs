using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRendererAdvanced : MonoBehaviour
{
  //  [SerializeField] TrajectoryRenderer trajectory;
    private LineRenderer lineRenderer;
   // Level level;
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

    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        //lineRenderer.positionCount = points.Length;

        float shortesDistance = 1000f;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.04f;
            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

            var dir = points[i] - origin;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir.normalized, out hit, dir.magnitude))
            {
                //if (hit.collider.gameObject.tag == "Bounce")
                //{
                Debug.DrawLine(origin, hit.point, Color.green);
                //Gizmos.DrawSphere(hit.point, 2f);
                //Gizmos.DrawSphere()
                //Debug.DrawRay(transform.position, transform.forward, Color.green);
                //break;

                //}
                //Debug.Log(hit.point.z);
                if (hit.point.z < shortesDistance)
                {
                    shortesDistance = hit.point.z;
                }

            }

            if (points[i].z > shortesDistance)
            {
                lineRenderer.positionCount = i;
                float prevTime = (i - 1) * 0.04f;
                Vector3 finalVelocity = CalculateFinalVelocity(points[i-1], origin, prevTime);
                //Vector3 finalVelocity = CalculateFinalVelocity(hit.point, origin, time);
                Debug.DrawRay(points[i-1], finalVelocity.normalized, Color.red);
                //InstantiateNewTrajectory(hit.point, hit.normal, finalVelocity, speed, time);
                InstantiateNewTrajectory(points[i - 1], hit.normal, finalVelocity, speed, prevTime);
                break;
            }


            //Debug.Log(points[i]);

            if (points[i].z < 0f)
            {
                lineRenderer.positionCount = i + 1;
                //break;
            }
            if (points[i].y < 0)
            {
                lineRenderer.positionCount = i + 1;
                break;
            }
        }

        lineRenderer.SetPositions(points);
    }

    private Vector3 CalculateFinalVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz * time;
        float Vy = (sY / time) + (0.5f * (Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }


    private void InstantiateNewTrajectory(Vector3 origin,Vector3 normal, Vector3 oldVel, Vector3 initialVel, float time)
    {
        //Vector3 normal = collision.contacts[0].normal; 
        var direction = Vector3.Reflect(oldVel.normalized, normal);
        Vector3 vel = direction.normalized * initialVel.magnitude;

        //TrajectoryRenderer tra = Instantiate(trajectory, origin, Quaternion.identity);
        FindObjectOfType<Grenade>().SetReboundTrajectoryParm(origin, vel, time);
        //trajectory.GetComponent<LineRenderer>().enabled = true;
        //trajectory.ShowTrajectory(origin, vel);
    }
}
