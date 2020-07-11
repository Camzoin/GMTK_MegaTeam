using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharBackOnHit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == JumpKingController.character)
        {
            JumpKingController.reference.Respawn();
        }
    }
}
