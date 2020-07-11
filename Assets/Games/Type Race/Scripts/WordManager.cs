using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    public TextMeshProUGUI wordUI;
    public TMP_InputField inputField;

    List<int> wordsToUse = new List<int>();
    TMP_TextInfo wordTextInfo;
    string[] wordPool;
    int[] wordIndices;
    string currentWord;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset wordFile = (TextAsset)Resources.Load("Words");
        wordPool = wordFile.text.Split('\r');
        RemoveNewLineChars();
        wordIndices = GenerateIndexArray(wordPool.Length);
        ShuffleArray(wordIndices, wordIndices.Length);
        wordsToUse = SplitArray(wordIndices, 500);
        DisplayWords();
        currentWord = wordPool[wordsToUse[0]];
        wordUI.ForceMeshUpdate();
        wordTextInfo = wordUI.textInfo;
        wordTextInfo.characterInfo[wordTextInfo.wordInfo[0].firstCharacterIndex].color = new Color32(255, 0, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputField.text = inputField.text.Replace(" ", "");
            if (inputField.text == currentWord)
            {
                wordUI.text = wordUI.text.Replace(currentWord + " ", "");
                wordsToUse.RemoveAt(0);
                currentWord = wordPool[wordsToUse[0]];
                inputField.text = string.Empty;
                CameraShake.instance.Shake(0.3f);
                GameManager.instance.ScorePoints(GameManager.games.TYPERACE, 1);
            }
        }
    }

    string RandomWord()
    {
        return wordPool[Random.Range(0, wordPool.Length)];
    }

    void DisplayWords()
    {
        for (int i = 0; i < wordsToUse.Count - 1; i++)
        {
            wordUI.text += wordPool[wordsToUse[i]] + " ";
        }
        wordUI.text += wordPool[wordsToUse.Count];
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
    List<int> SplitArray(int[] arr, int range)
    {
        int[] newArr = new int[range];
        for (int i = 0; i < range; i++)
        {
            newArr[i] = arr[i];
        }
        return newArr.ToList<int>();
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

    void RemoveNewLineChars()
    {
        for (int i = 0; i < wordPool.Length; i++)
        {
            wordPool[i] = wordPool[i].Replace("\n", "");
        }
    }
}
