using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManager : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();

    public float interval = 2.5f;
    public int perInterval = 4;

    private float counter;

    void Update()
    {
        counter += Time.deltaTime;

        if(counter >= interval)
        {
            counter = 0f;

            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].GetComponent<Dummy>().Up = false;
            }

            for (int i = 0; i < perInterval; i++)
            {
                targets[Random.Range(0, targets.Count)].GetComponent<Dummy>().Up = true;
            }
        }
    }
}
