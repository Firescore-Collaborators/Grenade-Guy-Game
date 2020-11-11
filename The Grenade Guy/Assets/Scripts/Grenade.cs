using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{   
    //Serializing fields
    [Header("Explosion")]
    [SerializeField] float explosionDelay = 3f;
    [SerializeField] float blastRadius = 5f;
    [SerializeField] float explosionForce = 700f;

    [Header("Grenade Effects")]
    [SerializeField] GameObject explosionEffectSpikyPrefab;
    [SerializeField] GameObject explosionEffectWavePrefab;
    [SerializeField] GameObject explosionEffectBrokenWallPrefab;
    [SerializeField] GameObject grenadeContactEffectPrefab;

    [Header("Trajectory")]
    [SerializeField] TrajectoryRenderer trajectoryPrefab;

    //Serialized fields for debugging purpose
    [Header("Tweaks")]
    [SerializeField] float forceFactorZeroGravity = 0f;
    [SerializeField] float forceFactorOriginaalGravity = 0f;
    [SerializeField] float bounceFactor = 1.4f;
    [SerializeField] float elapseTime = 0.1f;
    [SerializeField] float yDirectionTweak = 1.1f;
    [SerializeField] float zeroGravityTweak = -1f;
    [SerializeField] Vector3 grenadeAngularVelocity = new Vector3(4f, -3f, 2.4f);

    Camera camera;
    Vector3 initVel;
    private Vector3 startPos;                //grenade pos at beginning before it starts getting dragged
    private Vector3 endPos;                  //grendae pos when mouse drag is relesed
    private Vector3 forceAtPlayer;
    private Rigidbody rb;
    private Vector3 originalGravity;        //gravity for Set 2 Physics
    private Vector3 zeroGravity;             //gravity for Set 1 Physics
    private float forceFactor = 0f;
    private bool isZeroGrav = false;
    //float timer = 0f;
    private Vector3 initialVel;
    
    Vector3 pos = Vector3.zero;
    Vector3 normal;
    Vector3 finalVel;
    Vector3 oldVel = Vector3.zero;
    
    private float countDown;
    private bool hasThrown = false;
    private Vector3 cameraInitialPos;
    private Vector3 grenadeinitialPos;
    private Level level;
    private TrajectoryRenderer trajectory;
    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
        originalGravity = new Vector3(0, -9.81f, 0);
        zeroGravity = new Vector3(0, zeroGravityTweak, 0);
     
        countDown = explosionDelay;
        cameraInitialPos = new Vector3(-9.78f, 9.19f, -14.32f);
        grenadeinitialPos = new Vector3(-9.8f, 1.374f, -9.450001f);
        level = FindObjectOfType<Level>();
        trajectory = Instantiate(trajectoryPrefab, transform.position, Quaternion.identity);

    }

   

    private void FixedUpdate()
    {
        //Storing the previous velocity of the grenade
        oldVel = rb.velocity; 
    }

    private void Update()
    {
        //decreasing grenade timer. When it hits 0,
        //grenade goes kaboom :)
        if(hasThrown)
            countDown -= Time.deltaTime;
        if(countDown <= 0f)
        {
            Explode();
        }

        //  Debug.Log()
        Throw();
    }

    private void Explode()
    {
        //Instantiating all effects related to grenade explosion and destroying
        //them after they have finished doing their thing
        GameObject explosionEffectSpiky = Instantiate(
            explosionEffectSpikyPrefab, 
            transform.position, 
            transform.rotation);
        Destroy(explosionEffectSpiky, 0.3f);

        GameObject explosionEffectWave = Instantiate(
            explosionEffectWavePrefab,
            transform.position,
            transform.rotation);
        Destroy(explosionEffectWave, 0.25f);

        GameObject explosionEffectBrokenWall = Instantiate(
            explosionEffectBrokenWallPrefab,
            transform.position,
            transform.rotation);
        Destroy(explosionEffectBrokenWall, 1f);

        Collider[] colliders = Physics.OverlapSphere(transform.position,
            blastRadius);                                                       //storing all coinciding colliders of gameobjects
                                                                                //within the blast radius
        
        bool enemyHit = false;                                                  //to check whether any enemy has been killed or not

        //iterating all colliders to get the ones who are enemy
        foreach(Collider nearbyObject in colliders)
        {
            if(nearbyObject.gameObject.tag != "Enemy")
            {
                continue;
            }

            Rigidbody rigidbody = nearbyObject.GetComponent<Rigidbody>(); //geting the rigidbody of the enemy in blast radius
            if(rigidbody != null)
            {
                //freezing current animation on the enemy in blast radius and adding
                //an explosion force to him
                nearbyObject.gameObject.GetComponent<Animator>().enabled = false;
                rigidbody.AddExplosionForce(explosionForce, transform.position, blastRadius);
                level.DecrementNumEnemies();                               //decreasing the total number of enemies alive and
                                                                           // removing the enemy in blast radius from the list
                enemyHit = true;
            }
        }

        if(!enemyHit)
        {
            //if no enemy is injured instantiate a new greande
            FindObjectOfType<Player>().InstantiateGrenade(0.1f);
        }
        //destroy current grenade
        Destroy(gameObject);                                               
    }

   

    private void Throw()
    {
        //if mouse gets even clicked get the start pos of grenade
        if (Input.GetMouseButtonDown(0))
        {
            
            startPos = transform.position;
           
        }

        //if mouse gets dragged the required stuff gets done
        if (Input.GetMouseButton(0)) 
        {
          // Debug.Log(Mathf.Abs(forceAtPlayer.y));

            //if the mouse dosent get dragged too much downwards, use a particular phyics, say Set 1
            if (Mathf.Abs(forceAtPlayer.y) < 0.005f)
            {
                Physics.gravity = zeroGravity;
                forceFactor = forceFactorZeroGravity;
                isZeroGrav = true;
            }

            //if the mouse gets dragged too much downwards, use another particular phyics, say Set 2
            else if (Mathf.Abs(forceAtPlayer.y) >= 0.005f)
            {
                Physics.gravity = originalGravity;
                forceFactor = forceFactorOriginaalGravity;
                isZeroGrav = false;
            }

            //Calculating the endposition from where the grenade will be launched
            Vector3 greandeCorrection = grenadeinitialPos - startPos;
           // Debug.Log(greandeCorrection);
            Vector3 cameraCorrection = cameraInitialPos - camera.transform.position;

            endPos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.nearClipPlane)) + new Vector3(0.2011909f, -8.665852f + 1.124f, 4.747328f)
             + cameraCorrection - greandeCorrection;

            // + new Vector3(0.1823087f, -5.631144f + 1.124f, 7.963995f);
            // new Vector3(-0.1572821f, -5.662302f + 1.124f, -7.961307f);
            //+ new Vector3(0.3605785f, -1.1419f + 1.124f, 15.91763f);

            transform.position = endPos;
            forceAtPlayer = endPos - startPos;

            //Draw trajectory using Line Renderer
            trajectory.GetComponent<LineRenderer>().enabled = true;
            trajectory.ShowTrajectory(endPos, new Vector3(-forceAtPlayer.x * forceFactor,
                    -forceAtPlayer.y * forceFactor,
                   (-forceAtPlayer.z / yDirectionTweak) * forceFactor));
           
        }

        
        //Do stuff when mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            //destroy the trajectory
            trajectory.Dest();
            
            //start the grenade toss animation
            FindObjectOfType<Player>().SetGrenadeActive(true);
            StartCoroutine(ActualThrow());       //the calculation of grenade throw physics
           
        }
    }
    
    IEnumerator ActualThrow()
    {
        yield return new WaitForSeconds(0.6f); //waiting for the grenade toss animation to complete

        //enabling the mesh renderers of all child objects of grenade
        var allChildren = GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren)
        {
            if (child.gameObject.GetComponent<MeshRenderer>())
            {
                child.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        //adding some angular velocity to the grenade
        GetComponent<Rigidbody>().angularVelocity = grenadeAngularVelocity;
        transform.parent = null;                   //this statement is for debugging purpose
        rb.useGravity = true;                     //enabling grenade's gravity

        //if the grenade is follwing Set 1 pysics, add velocity in a certain way
        if (isZeroGrav)
        {
            initVel = initialVel = rb.velocity = new Vector3(
                -forceAtPlayer.normalized.x * forceFactor,
                  0f, -forceAtPlayer.normalized.z * forceFactor);
        }

        //if the grenade is follwing Set 2 pysics, add velocity in another way
        else
        {
            initVel = initialVel = rb.velocity = new Vector3(
                -forceAtPlayer.x * forceFactor,
                    -forceAtPlayer.y * forceFactor,
                   (-forceAtPlayer.z / yDirectionTweak) * forceFactor);

        }

        hasThrown = true;                     //grenade has been thrown

    }


    //Calculating the rebound the grenade on colliding
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bounce"))  //this statement is not necessary now, a better way to do 
        {                                               //it is remove it and also "Bounce" tag fromm all objects like floor,
                                                        //buildings, etc.

            Vector3 normal = collision.contacts[0].normal;  //calculating the normal at collision point
            var direction = Vector3.Reflect(oldVel.normalized, normal);  //calculating the rebound direction based on previous
                                                                         //velocity of grenade just before collision & normal
            //To visulise the path of the grenade on collision with anything
           /* var pointC = p + direction.normalized * 10;
            Debug.DrawRay(collision.contacts[0].point, oldVel.normalized, Color.blue, 10, false);      //direction of grende just before collision
            Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 10, false); //direction of normal at collision point
            Debug.DrawRay(collision.contacts[0].point, direction.normalized, Color.red, 10, false);*/   //rebound direction of grenade after collision

           //decresing the magnitude of grenade's velocity at every collision by a factor of bounceFactor
           initialVel = rb.velocity = direction.normalized * initialVel.magnitude / bounceFactor;

            //Instantiating grenade collision effect and destroying it after its done doing its shit
           GameObject grenadeContactEffect = Instantiate(grenadeContactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(grenadeContactEffect, 1f);
           
        }
    }
    
    
}


