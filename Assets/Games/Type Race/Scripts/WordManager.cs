using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public Transform dynamicCanvas;
    public Transform currentWordCanvas;
    public GameObject WordPrefab;
    public Color32 textColor;
    public int wordsOnScreen;

    List<int> wordsToUse = new List<int>();
    List<TextMeshProUGUI> wordList = new List<TextMeshProUGUI>();
    string[] wordPool;
    int[] wordIndices;
    string currentWord;

    TextMeshProUGUI previousWord;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset wordFile = (TextAsset)Resources.Load("Words");

        wordPool = wordFile.text.Split('\r');
        RemoveNewLineChars();
        wordIndices = GenerateIndexArray(wordPool.Length);
        ShuffleArray(wordIndices, wordIndices.Length);
        wordsToUse = SplitArray(wordIndices, 500);

        currentWord = wordPool[wordsToUse[0]];

        for (int i = 0; i < wordsOnScreen; i++)
        {
            wordList.Add(SpawnWord(wordPool[wordsToUse[0]]));
            wordsToUse.RemoveAt(0);
            previousWord = wordList[0];
            float newAlpha = 1.0f - i / (float)wordsOnScreen;
            wordList.Last().color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);
            wordList[0].color = Color.red;
            wordList[0].rectTransform.parent = currentWordCanvas;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputField.text = inputField.text.Replace(" ", "");
            if (inputField.text == currentWord)
            {
                Destroy(wordList[0].gameObject);
                wordList.RemoveAt(0);
                wordList.Add(SpawnWord(wordPool[wordsToUse[0]]));
                wordsToUse.RemoveAt(0);
                AdjustWordAlphas();

                currentWord = wordList[0].text;
                wordList[0].color = Color.red;
                wordList[0].rectTransform.parent = currentWordCanvas;
                inputField.text = string.Empty;
                CameraShake.instance.Shake(0.3f);
                GameManager.instance.ScorePoints(GameManager.games.TYPERACE, 1);
            }
        }
    }

    void AdjustWordAlphas()
    {
        for (int i = 0; i < wordList.Count; i++)
        {
            float newAlpha = 1.0f - i / (float)wordList.Count;
            wordList[i].color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);
        }
    }

    TextMeshProUGUI SpawnWord(string word)
    {
        Vector3 newPos = new Vector3(Random.Range(-90, 90), Random.Range(0, 50), 90);
        if (previousWord != null)
        {
            Rect testRect = new Rect(newPos, previousWord.rectTransform.rect.size);
            for (int i = 0; i < 100; i++)
            {
                if (previousWord.rectTransform.rect.Overlaps(testRect))
                {
                    newPos = new Vector3(Random.Range(-90, 90), Random.Range(0, 50), 90);
                    break;
                }
            }
        }

        TextMeshProUGUI textUI = Instantiate(WordPrefab, newPos, transform.rotation, dynamicCanvas).GetComponent<TextMeshProUGUI>();
        textUI.text = word;

        return textUI;
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
