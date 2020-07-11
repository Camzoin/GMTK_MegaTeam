using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public int worth = 1;
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == JumpKingController.character)
        {
            if(GameManager.instance != null)
                GameManager.instance.ScorePoints(GameManager.games.JUMPKING, worth);

            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
