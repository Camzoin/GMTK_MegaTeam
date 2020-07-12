using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneMover : MonoBehaviour
{
    public float speed = 1;

    float time, timeToNewDest = 5;

    Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {
        dest = new Vector3(Random.Range(-6.83f, 7f), 0, Random.Range(3.27f, -9.35f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, dest, Time.deltaTime * speed);

        time = time + Time.deltaTime;

        if (time > timeToNewDest)
        {
            //new dest
            dest = new Vector3(Random.Range(-6.83f, 7f), 0, Random.Range(3.27f, -9.35f));


            timeToNewDest = Random.Range(0f, 7f);

            time = 0;
        }
        
        if (Vector3.Distance(transform.position, dest) < 1)
        {
            dest = new Vector3(Random.Range(-6.83f, 7f), 0, Random.Range(3.27f, -9.35f));
        }
    }
}
