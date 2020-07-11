using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    public TextMeshProUGUI wordUI;
    public TextMeshProUGUI inputField;

    static string[] wordPool;
    int[] wordIndices;
    int[] wordsToUse;
    string currentWord;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset wordFile = (TextAsset)Resources.Load("Words");
        wordPool = wordFile.text.Split('\r');
        wordIndices = GenerateIndexArray(wordPool.Length);
        ShuffleArray(wordIndices, wordIndices.Length);
        wordsToUse = SplitArray(wordIndices, 500);
        DisplayWords();
        currentWord = wordPool[wordsToUse[0]];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inputField.text == currentWord)
            {
                wordUI.text.Replace(currentWord, "");
            }
        }
    }

    string RandomWord()
    {
        return wordPool[Random.Range(0, wordPool.Length)];
    }

    void DisplayWords()
    {
        for (int i = 0; i < wordsToUse.Length - 1; i++)
        {
            wordUI.text += wordPool[wordsToUse[i]].Replace("\n","") + " ";
        }

    }

    void ShuffleArray(int[] arr, int wordCount)
    {
        for (int i = wordCount - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
    int[] SplitArray(int[] arr, int range)
    {
        int[] newArr = new int[range];
        for (int i = 0; i < range; i++)
        {
            newArr[i] = arr[i];
        }
        return newArr;
    }

    int[] GenerateIndexArray(int range)
    {
        int[] newArr = new int[range];
        for (int i = 0; i < range; i++)
        {
            newArr[i] = i;
        }
        return newArr;
    }
}
