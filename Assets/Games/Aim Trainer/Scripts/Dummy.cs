using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public bool Up = false;
    private bool shot = false;

    public float shotCD = 0.0f;

    void Update()
    {
        if (Up)
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), 0.02f));
        else
            if(shot)
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(90, 0, 0), 0.08f));
            else
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(90, 0, 0), 0.01f));

        shot = shotCD > 0;
        shotCD -= Time.deltaTime;
    }
}