using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed;
    public float xBounds;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Random.Range(30, 150) + Random.Range(0, 2) * 180;
        float radAngle = angle * Mathf.Deg2Rad;
        Vector2 moveVec = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));

        rb.velocity = moveVec * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.position.x > xBounds)
        {
            rb.position = Vector3.zero;

            Vector2 moveVec = Random.insideUnitCircle.normalized;

            rb.velocity = moveVec * moveSpeed;
        }
        if (rb.position.x < -xBounds)
        {
            rb.position = Vector3.zero;

            Vector2 moveVec = Random.insideUnitCircle.normalized;

            rb.velocity = moveVec * moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.AddForce(Random.insideUnitCircle.normalized * 5, ForceMode2D.Impulse);
        CameraShake.instance.Shake(0.1f);
    }
}
