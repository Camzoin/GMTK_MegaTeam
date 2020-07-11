using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public bool Up = false;
    private bool shot = false;

    //CAMDEN DID THIS
    bool pointScored = false;

    public float shotCD = 0.0f;

    void Update()
    {
        if (Up)
        { 
           transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), 0.025f));

           //CAMDEN DID THIS
           pointScored = false;
        }

        else
            if (shot)
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(90, 0, 0), 0.08f));
                //CAMDEN DID THIS
                if (pointScored == false)
                {
                   GameManager.instance.ScorePoints(GameManager.games.AIMTRAIN, 1);
                   
                   pointScored = true;
                }
            else
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(90, 0, 0), 0.025f));

        shot = shotCD > 0;
        shotCD -= Time.deltaTime;
    }
}