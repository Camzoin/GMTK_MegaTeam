using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoSpawner : MonoBehaviour
{
    public List<Transform> spawns;

    public float timeToSpawn;

    float time;

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
       foreach(Transform child in transform)
        {
            spawns.Add(transform);
            Instantiate(enemy, child.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;

        if (time > timeToSpawn)
        {
            //spawn

            Instantiate(enemy, spawns[Random.Range(0, spawns.Count)].position, Quaternion.identity);

            time = 0;
        }
    }

    void StartSpawn()
    {

    }
}
