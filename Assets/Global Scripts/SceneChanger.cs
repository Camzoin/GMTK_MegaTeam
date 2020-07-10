using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public Material fadeMat;
    float size = 0;
    float rotation = 45;
    bool fadingIn, fadingOut;
    float alpha = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        fadeMat.SetFloat("Rotation", rotation);
        fadeMat.SetInt("Pattern" + Random.Range(1, 4), 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingOut == true)
        {

        }

        if (fadingIn == true)
        {

        }

        //fade out

        fadeMat.SetFloat("Size", size);

        //size = size lerp 1

        fadeMat.SetColor("Color_78EBAF80", new Color(0,0,0, ))

        //color = color A 1


        //fade in

        //size = size lerp 0

        //color = color A 0
    }

    void RandomPattern()
    {
        fadeMat.SetInt("Pattern1", 0);
        fadeMat.SetInt("Pattern2", 0);
        fadeMat.SetInt("Pattern3", 0);
        fadeMat.SetInt("Pattern4", 0);

        fadeMat.SetInt("Pattern" + Random.Range(1, 4), 1);
    }

    void ChangeScene()
    {
        //New Pattern / rotation
        RandomPattern();
        fadeMat.SetFloat("Rotation", Random.Range(0f, 360f));

    }
}