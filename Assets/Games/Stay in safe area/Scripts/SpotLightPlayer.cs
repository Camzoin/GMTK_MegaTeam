using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightPlayer : MonoBehaviour
{
    Vector3 x, z;

    Rigidbody rb;

    public float playerSpeed = 1;

    public Transform forward;

    Vector3 realMovement;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        x = forward.transform.right * Input.GetAxisRaw("Horizontal");
        z = forward.transform.forward * Input.GetAxisRaw("Vertical");       
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            //left?
            transform.rotation = Quaternion.Lerp(transform.rotation, forward.transform.rotation, Time.deltaTime);
        
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            //right?
            transform.rotation = Quaternion.Lerp(transform.rotation, forward.transform.rotation, Time.deltaTime);
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            //up
            transform.rotation = Quaternion.Lerp(transform.rotation, forward.transform.rotation, Time.deltaTime);
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            //down
            transform.rotation = Quaternion.Lerp(transform.rotation, forward.transform.rotation, Time.deltaTime);
        }


        Vector3 movement = (x + z) * playerSpeed;

        realMovement = Vector3.Lerp(realMovement, movement, 10 * Time.deltaTime);

        rb.velocity = realMovement;
    }
}
