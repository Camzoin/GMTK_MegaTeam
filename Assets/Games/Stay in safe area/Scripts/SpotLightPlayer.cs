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

    bool isSafe = true;

    bool isMoving = false;

    Vector3 oldPos;

    Animator anim;

    float scoreTime = 0.2f;

    float gainScoreTime, loseScoreTime;

    public List<GameObject> environments;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        anim = gameObject.GetComponentInChildren<Animator>();

        environments[Random.Range(0, 2)].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        x = forward.transform.right * Input.GetAxisRaw("Horizontal");
        z = forward.transform.forward * Input.GetAxisRaw("Vertical");

        if (isMoving == true)
        {
            rb.transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), transform.up);
        }

        float dist = Vector3.Distance(oldPos, transform.position);

        if (dist > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        oldPos = transform.position;

        //score
        if (isSafe == true)
        {
            gainScoreTime = gainScoreTime + Time.deltaTime;
            //add
            if (gainScoreTime > scoreTime)
            {
                GameManager.instance.ScorePoints(GameManager.games.SPOTLIGHT, 0.1f);
                gainScoreTime = 0;
            }
        }
        else
        {
            loseScoreTime = loseScoreTime + Time.deltaTime;
            //subtract
            if (loseScoreTime > scoreTime)
            {
                GameManager.instance.ScorePoints(GameManager.games.SPOTLIGHT, -0.05f);
                loseScoreTime = 0;
            }
        }

        //anim stuff
        anim.SetBool("isWalking", isMoving);
    }

    private void FixedUpdate()
    {
        Vector3 movement = (x + z) * playerSpeed;

        realMovement = Vector3.Lerp(realMovement, movement, 10 * Time.deltaTime);

        rb.velocity = realMovement;
    }

    void OnTriggerEnter(Collider other)
    {
        isSafe = true;
    }

    void OnTriggerExit(Collider other)
    {
        isSafe = false;
    }
}
