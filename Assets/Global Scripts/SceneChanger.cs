using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Material fadeMat;
    float size = 0;
    float rotation = 45;
    bool fadingIn, fadingOut;
    float alpha;
    private int nextScene;
    

    private void Awake()
    {
        //fadeMat.SetInt("Pattern1", 0);
        //fadeMat.SetInt("Pattern2", 0);
        //fadeMat.SetInt("Pattern3", 0);
        //fadeMat.SetInt("Pattern4", 0);
        //alpha = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        RandomPattern();

        fadeMat.SetFloat("Rotation", rotation);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        if (fadingOut == true)
        {
            size = Mathf.Lerp(size, 1.1f, 2 * Time.deltaTime);
            alpha = Mathf.Lerp(alpha, 1, 5 * Time.deltaTime);

            if(size >= 1 - float.Epsilon)
            {
                fadingOut = false;
                fadingIn = true;
                SceneManager.LoadScene(nextScene);



            }
        }

        if (fadingIn == true)
        {
            size = Mathf.Lerp(size, -0.3f, 0.5f * Time.deltaTime);
            alpha = Mathf.Lerp(alpha, 0, 5 * Time.deltaTime);

            if (size < 0)
            {
                size = 0;
                fadingIn = false;
            }
        }

        fadeMat.SetFloat("Size", size);

        fadeMat.SetColor("Color_78EBAF80", new Color(0, 0, 0, alpha));
    }

    void RandomPattern()
    {
        fadeMat.SetInt("Pattern1", 0);
        fadeMat.SetInt("Pattern2", 0);
        fadeMat.SetInt("Pattern3", 0);
        fadeMat.SetInt("Pattern4", 0);

        fadeMat.SetInt("Pattern" + Random.Range(1, 4), 1);
    }

    public void ChangeScene(int sceneNumber)
    {
        //New Pattern / rotation
        RandomPattern();
        fadeMat.SetFloat("Rotation", Random.Range(0f, 360f));

        //start "anim"
        fadingOut = true;

		//Set to scene
		nextScene = sceneNumber;
	}
}