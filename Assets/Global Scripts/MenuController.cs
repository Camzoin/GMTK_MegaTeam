using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

	public GameObject init1;
	public GameObject init2;
	public GameObject init3;
	
	public TextMeshProUGUI finalScore;

	int curLetter = 0;

	private List<char> allChars = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

	private void Update()
	{
		if (SceneManager.GetActiveScene().buildIndex == 26)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				//move left
				if (curLetter != 0)
				{
					curLetter--;
				}
				else
				{
					curLetter = 2;
				}

			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				Text curObj = init1.GetComponent<Text>();

				if (curLetter == 0)
				{
					curObj = init1.GetComponent<Text>();
				}
				else if (curLetter == 1)
				{
					curObj = init2.GetComponent<Text>();
				}
				else if (curLetter == 2)
				{
					curObj = init3.GetComponent<Text>();
				}

				int curLetterIndex = allChars.IndexOf(curObj.text[0]);

				if (curLetterIndex < allChars.Count)
				{
					curObj.text = "" + allChars[curLetterIndex - 1];
				}
				else
				{
					curObj.text = "" + allChars[0];
				}
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				//move right
				if (curLetter != 2)
				{
					curLetter++;
				}
				else
				{
					curLetter = 0;
				}
			}
			else if (Input.GetKeyDown(KeyCode.W))
			{
				Text curObj = init1.GetComponent<Text>();

				if (curLetter == 0)
				{
					curObj = init1.GetComponent<Text>();
				}
				else if (curLetter == 1)
				{
					curObj = init2.GetComponent<Text>();
				}
				else if (curLetter == 2)
				{
					curObj = init3.GetComponent<Text>();
				}

				int curLetterIndex = allChars.IndexOf(curObj.text[0]);

				if (curLetterIndex > 0)
				{
					curObj.text = "" + allChars[curLetterIndex + 1];
				}
				else
				{
					curObj.text = "" + allChars[allChars.Count - 1];
				}
			}
		}
	}

	private void Start()
	{
		try
		{
			float score = GameManager.instance.GetTotalScore();
			finalScore.text = "" + score;
		}
		catch
		{
			Debug.Log("Getting the score for the menu did not work.");
		}
		
	}

	public void RefreshScores()
	{
		GameManager.instance.DownloadScores();
	}

	public void PlayGame()
	{
		GameManager.instance.StartNewSession();
	}

	public void SubmitScore()
	{
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			SceneManager.LoadScene(26);
		}
		else
		{
			;
			GameManager.instance.SetName(init1.GetComponent<Text>().text + init2.GetComponent<Text>().text + init3.GetComponent<Text>().text);
			GameManager.instance.SubmitScore();
		}
	}

}
