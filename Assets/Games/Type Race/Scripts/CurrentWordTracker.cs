using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentWordTracker : MonoBehaviour
{
    ObjectShake objectShake;
    // Start is called before the first frame update
    void Start()
    {
        objectShake = GetComponent<ObjectShake>();
        objectShake.BeforeShake += () =>
        {
            objectShake.origin = WordManager.instance.GetCurrentWord().transform.position;
        };
    }
}
