using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroopGame : MonoBehaviour
{
    public List<GameObject> words;
    public List<GameObject> shatWords;
    public List<Color> colors;
    public Transform wordHolder;
    string curWord;
    GameObject curWordObj;
    Color curWordColor;
    int colorInt;
    int wordInt;

    // Start is called before the first frame update
    void Start()
    {
        wordInt = Random.Range(0, 6);
        colorInt = Random.Range(0, 6);

        curWordObj = Instantiate(words[wordInt], wordHolder);
        curWordObj.GetComponent<Renderer>().material.color = colors[colorInt];
    }

    // Update is called once per frame
    void Update()
    {
        curWord = curWordObj.name;

        //Debug.Log(curWord);
    }

    public void ColorGuess(int color)
    {
        if(colorInt == color)
        {
            //point
            GameManager.instance.ScorePoints(GameManager.games.STROOP, 1f);

            //new word
            NewWord();
        }
        else
        {
            //lose point
            GameManager.instance.ScorePoints(GameManager.games.STROOP, -1);
        }
    }

    void NewWord()
    {
        //make explode at some point
        Destroy(curWordObj);

        GameObject crumbleWord = Instantiate(shatWords[wordInt], curWordObj.transform.position, curWordObj.transform.rotation);

        foreach (Renderer child in crumbleWord.GetComponentsInChildren<Renderer>())
        {
            child.material.color = colors[colorInt];
        }

        wordInt = Random.Range(0, 6);

        curWordObj = Instantiate(words[wordInt], wordHolder);
        SetColor();
    }

    void SetColor()
    {
        colorInt = Random.Range(0, 6);

        curWordObj.GetComponent<Renderer>().material.color = colors[colorInt];
    }
}