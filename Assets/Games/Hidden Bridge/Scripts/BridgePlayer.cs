using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BridgePlayer : MonoBehaviour
{
    Vector3 x, z;

    Rigidbody rb;

    public float playerSpeed = 1;

    public Transform forward;

    Vector3 realMovement;

    bool isMoving = false;

    Vector3 oldPos;

    Animator anim;

    RaycastHit hit;

    float rayDist = 1;

    Vector3 dir;

    float grav;

    bool isGrounded = true;

    int curCam = 11;

    public List<Transform> camPoints;

    public Transform cameraTrans;

    float range = 5;

    public Transform playerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        anim = gameObject.GetComponentInChildren<Animator>();

        dir = new Vector3(0, -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_InvisRange", range);


        if (isGrounded == true)
        {
            grav = 0;
        }
        else
        {
            grav = -4;
        }
       
        //Debug.DrawRay(transform.position, dir * rayDist, Color.green);
        
        if (Physics.Raycast(transform.position, dir, out hit, rayDist))
        {
            //the ray collided with something, you can interact
            // with the hit object now by using hit.collider.gameObject

            if (hit.collider.isTrigger)
            {
                isGrounded = false;
            }
            else
            {
                isGrounded = true;
            }

        }
        else
        {
            isGrounded = false;
        }

        //shdader
        Shader.SetGlobalVector("_PlayerPos", transform.position);


        x = forward.transform.right * Input.GetAxisRaw("Horizontal");
        z = forward.transform.forward * Input.GetAxisRaw("Vertical");

        if (isMoving == true)
        {
            rb.transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), transform.up);

            range = Mathf.Lerp(range, 6, 5 * Time.deltaTime);
        }
        else
        {
            range = Mathf.Lerp(range, 0, 0.5f * Time.deltaTime);
        }

        float dist = Vector3.Distance(oldPos, transform.position);


        if (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y) + Mathf.Abs(rb.velocity.z) < 0.5f)
        {
            rb.velocity = Vector3.zero;
        }


        if (rb.velocity != Vector3.zero)
        {           
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        oldPos = transform.position;

        //anim stuff
        anim.SetBool("isWalking", isMoving);

        //cam stuff
        cameraTrans.position = Vector3.Lerp(cameraTrans.position, camPoints[curCam - 1].position, Time.deltaTime);

        cameraTrans.rotation = Quaternion.Lerp(cameraTrans.rotation, camPoints[curCam - 1].rotation, Time.deltaTime);



    }

    private void FixedUpdate()
    {
        Vector3 movement = (x + z) * playerSpeed;

        realMovement = Vector3.Lerp(realMovement, movement, 10 * Time.deltaTime);

        realMovement.y = grav;

        rb.velocity = realMovement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Kill Box" || other.name != "Coin")
        {
            curCam = Convert.ToInt32(other.gameObject.name);
        }
        if (other.name == "Kill Box")
        {
            gameObject.transform.position = playerSpawn.position;
            gameObject.transform.rotation = playerSpawn.rotation;

            cameraTrans.position = camPoints[10].position;

            cameraTrans.rotation = camPoints[10].rotation;

            curCam = 11;
        }
    }
}
