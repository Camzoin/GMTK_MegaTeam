using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperAI : MonoBehaviour
{
    public float moveSpeed;
    public float yBounds;

    Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("PongBall").GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.position.y >= transform.position.y)
        {
            transform.position = transform.position + Vector3.up * Time.deltaTime * moveSpeed;
        }
        if (ball.transform.position.y <= transform.position.y)
        {
            transform.position = transform.position - Vector3.up * Time.deltaTime * moveSpeed;
        }

        if (transform.position.y > yBounds)
            transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
        if (transform.position.y < -yBounds)
            transform.position = new Vector3(transform.position.x, -yBounds, transform.position.z);
    }
}
