using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public GameObject bounceEffect;
    public Image right, left;
    public float moveSpeed;
    public float maxSpeed;
    public float xBounds;

    Rigidbody2D rb;

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
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        if (rb.position.x > xBounds)
        {
            StartCoroutine(FlashSide(left, 0.8f, 0, 1f));

            rb.position = Vector3.zero;

            Vector2 moveVec = Random.insideUnitCircle.normalized;

            rb.velocity = moveVec * moveSpeed;
        }
        if (rb.position.x < -xBounds)
        {
            StartCoroutine(FlashSide(right, 0.8f, 0, 1f));

            rb.position = Vector3.zero;

            Vector2 moveVec = Random.insideUnitCircle.normalized;

            rb.velocity = moveVec * moveSpeed;

            GameManager.instance.ScorePoints(GameManager.games.PONG, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.AddForce(Random.insideUnitCircle.normalized * 3, ForceMode2D.Impulse);
        CameraShake.instance.Shake(0.1f);
        Instantiate(bounceEffect, transform.position, Quaternion.identity);
    }

    IEnumerator FlashSide(Image image, float start, float end, float duration)
    {
        float timer = 0;

        while (timer <= 1)
        {
            timer += Time.deltaTime / duration;
            float newAlpha = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, timer));
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }
    }
}
