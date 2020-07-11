using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKingController : MonoBehaviour
{
    public static GameObject character;
    public static JumpKingController reference;

    public float maxJumpDistance = 20f;
    public float jumpLengthX = 20f;
    public float jumpLengthY = 20f;
    public float jumpChargeSpeed = 0.05f;
    public float cameraSmoothing = 1f;
    public float fillBarSmoothing = 1f;
    public bool grounded = false;

    public Animator animator;
    public GameObject cameraObject;
    public new Rigidbody2D rigidbody2D;
    public UnityEngine.UI.Image fillBar;
    public SpriteRenderer sprite;
    public Vector3 respawnLocation;

    [HideInInspector] public float currentHeldCharge = 0f;
        
    private void OnEnable()
    {
        character = gameObject;
        reference = this;
    }

    void Update()
    {
        float inputAxis = Input.GetAxis("Horizontal");

        Collider2D[] cast = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y) + (Vector2.down * 0.75f), new Vector2(0.7f, 0.1f), 0f);
        grounded = cast.Length != 0;
        
        if (inputAxis >= 0.05)
        {
            sprite.flipX = true;
        }
        else if (inputAxis <= -0.05)
        {
            sprite.flipX = false;
        }

        if((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && grounded)
        {
            currentHeldCharge += Time.deltaTime * jumpChargeSpeed;
            animator.SetBool("InputState", true);
        }
        else
        {
            animator.SetBool("InputState", false);

            if (sprite.flipX)
                rigidbody2D.AddForce(new Vector2(Mathf.Clamp(Time.deltaTime * currentHeldCharge * jumpLengthX, maxJumpDistance * -1, maxJumpDistance), Mathf.Clamp(Time.deltaTime * currentHeldCharge * jumpLengthY, maxJumpDistance * -1, maxJumpDistance)), ForceMode2D.Force);

            if (!sprite.flipX)
                rigidbody2D.AddForce(new Vector2(-Mathf.Clamp(Time.deltaTime * currentHeldCharge * jumpLengthX, maxJumpDistance * -1, maxJumpDistance), Mathf.Clamp(Time.deltaTime * currentHeldCharge * jumpLengthY, maxJumpDistance * -1, maxJumpDistance)), ForceMode2D.Force);

            currentHeldCharge = 0f;
        }

        int state = 0;

        if (currentHeldCharge == 0f)
            state = 0;
        else if (currentHeldCharge < 0.4f)
            state = 1;
        else if (currentHeldCharge < 0.65f)
            state = 2;
        else if (currentHeldCharge > 0.75f)
            state = 3;

        animator.SetBool("Grounded", grounded);
        animator.SetInteger("Jump", state);

        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, transform.position + new Vector3(0, 5, -10), Time.deltaTime * cameraSmoothing);

        fillBar.fillAmount = Mathf.Lerp(fillBar.fillAmount, Mathf.Clamp(Mathf.Clamp(currentHeldCharge * jumpLengthX, 0, maxJumpDistance) / maxJumpDistance, 0, 1), Time.deltaTime * fillBarSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + (Vector3.down * 0.75f), new Vector3(0.7f, 0.1f, 0.1f));
    }

    public void Respawn()
    {
        transform.position = respawnLocation;
        rigidbody2D.velocity = Vector2.zero;
        cameraObject.transform.position = transform.position + new Vector3(0, 0, -10);
    }
}