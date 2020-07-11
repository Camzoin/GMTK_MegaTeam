using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForce : MonoBehaviour
{
    float time, timeToDelete = 2;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-50f, 50f), Random.Range( 750f, 1000f), Random.Range(-50f, 50f)));
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;

        if (time > timeToDelete)
        {
            Destroy(transform.root.gameObject);
        }
    }
}
