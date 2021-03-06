﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float moveSpeed;
    public float yBounds;
    float yInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        yInput = Input.GetAxisRaw("Vertical");
        
        if (yInput != 0)
        {
            Vector3 moveVec = new Vector3(0, yInput, 0);
            transform.position = transform.position + moveVec * Time.deltaTime * moveSpeed;
        }
        if (transform.position.y > yBounds)
            transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
        if (transform.position.y < -yBounds)
            transform.position = new Vector3(transform.position.x, -yBounds, transform.position.z);
    }
}
