using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    public GameObject bullet = null;
    public float shootRate;
    public float projectileSpeed = 1.0f;

    private float shootCD;

    void Update()
    {
        Vector3 difference = transform.position - JumpKingController.character.transform.position;
        
        float zRadians = Mathf.Atan2(difference.y, difference.x);

        transform.rotation = Quaternion.Euler(0, 0, (zRadians * Mathf.Rad2Deg) + 90);

        shootCD += Time.deltaTime;

        if (shootCD >= shootRate)
        {
            shootCD = 0f;

            GameObject b = Instantiate(bullet, transform.position + (transform.forward * 0.5f), transform.rotation);
            b.GetComponent<Rigidbody2D>().AddForce(transform.up * Time.deltaTime * projectileSpeed, ForceMode2D.Force);
        }
    }
}
