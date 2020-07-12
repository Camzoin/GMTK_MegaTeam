using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightPlayer : MonoBehaviour
{
    Vector3 x, z;

    Rigidbody rb;

    public float playerSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        x = gameObject.transform.right * Input.GetAxisRaw("Horizontal");
        z = gameObject.transform.forward * Input.GetAxisRaw("Vertical");       
    }

    private void FixedUpdate()
    {
        Vector3 movement = (x + z) * playerSpeed;

        rb.AddForce(movement);
    }
}
