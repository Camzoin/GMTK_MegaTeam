using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCamLerp : MonoBehaviour
{
    public Transform camPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float multi = 1 + ( 0.5f * Vector3.Distance(transform.position, camPos.position));

        //transform.position = Vector3.Lerp(transform.position, camPos.position, multi * Time.deltaTime);
    }
}
