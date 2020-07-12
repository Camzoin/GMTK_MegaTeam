using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSumo : MonoBehaviour
{
    public GameObject smashExplosion;

    Vector3 x, z;

    public float playerSpeed;

    public Rigidbody rb;

    public GameObject stage;

    public GameObject spawn;

    float time = 0.5f, spawnTime = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;

        //player movement
        if (time > spawnTime)
        {
            x = stage.transform.right * Input.GetAxisRaw("Horizontal");
            z = stage.transform.forward * Input.GetAxisRaw("Vertical");
        }

        Vector3 movement = (x + z) * playerSpeed;

        rb.AddForce(movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        //take player points

        GameManager.instance.ScorePoints(GameManager.games.SUMO, -5);

        //smash bros VFX

        Instantiate(smashExplosion, transform.position, Quaternion.identity);
        CameraShake.instance.Shake(0.2f);

        transform.position = spawn.transform.position;

        rb.velocity = new Vector3(0, 0, 0);

        time = 0;
    }
}
