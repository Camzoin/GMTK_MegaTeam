using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroopGame : MonoBehaviour
{
    public List<GameObject> words;
    public List<Color> colors;
    public Transform wordHolder;
    string curWord;
    GameObject curWordObj;
    Color curWordColor;
    int colorInt;

    // Start is called before the first frame update
    void Start()
    {
        curWordObj = Instantiate(words[Random.Range(0, words.Count)], wordHolder);
        curWordObj.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Count)];
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
            GameManager.instance.ScorePoints(GameManager.games.STROOP, 1);

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

        curWordObj = Instantiate(words[Random.Range(0, words.Count)], wordHolder);
        SetColor();
    }

    void SetColor()
    {
        colorInt = Random.Range(0, 6);

        curWordObj.GetComponent<Renderer>().material.color = colors[colorInt];
    }
}