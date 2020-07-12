using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    public GameObject destroyEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}