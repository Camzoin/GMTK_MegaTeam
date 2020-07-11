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
    float alpha = 0;
    private int nextScene;

    private void Awake()
    {
        fadeMat.SetInt("Pattern1", 0);
        fadeMat.SetInt("Pattern2", 0);
        fadeMat.SetInt("Pattern3", 0);
        fadeMat.SetInt("Pattern4", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        RandomPattern();

        fadeMat.SetFloat("Rotation", rotation);
        fadeMat.SetInt("Pattern" + Random.Range(1, 4), 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingOut == true)
        {
            size = Mathf.Lerp(size, 1, Time.deltaTime);
            alpha = Mathf.Lerp(alpha, 1, 5 * Time.deltaTime);

            if(Mathf.Approximately(size, 1))
            {
                fadingOut = false;
                fadingIn = true;
                SceneManager.LoadScene(nextScene);
            }
        }

        if (fadingIn == true)
        {
            size = Mathf.Lerp(size, 0, Time.deltaTime);
            alpha = Mathf.Lerp(alpha, 0, 5* Time.deltaTime);

            if (Mathf.Approximately(size, 0))
            {
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