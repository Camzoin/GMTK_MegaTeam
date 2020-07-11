using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public bool Up = false;

    void Update()
    {
        if (Up)
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), 0.01f));
        else
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(90, 0, 0), 0.01f));
    }
}
