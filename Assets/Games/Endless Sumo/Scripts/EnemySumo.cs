using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySumo : MonoBehaviour
{
    public Renderer wireBall;
    public GameObject player;
    public Rigidbody rb;
    public GameObject smashExplosion;
    public float difficulty = 1;

    float time = 0.5f, spawnTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        //random color
        wireBall.material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //roll twoards player
        time = time + Time.deltaTime;

        if (time > spawnTime)
        {
            rb.AddForce(new Vector3((player.transform.position.x - transform.position.x) * difficulty, 0, (player.transform.position.z - transform.position.z) * difficulty));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy died");

        //give player points

        //GameManager.instance.ScorePoints(GameManager.games.SUMO, 1);


        //smash bros VFX

        Instantiate(smashExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
