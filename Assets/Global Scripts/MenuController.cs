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

	private List<char> allChars = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!'};

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

			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				Text curObj;

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

				//int curLetterIndex = index

			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				//move right
				if (curLetter != 2)
				{
					curLetter++;
				}
			}
			else if (Input.GetKeyDown(KeyCode.W))
			{
				
			}



		}
	}

	private void Start()
	{
		float score = GameManager.instance.GetTotalScore();

		finalScore.text = "" + score;
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
			GameManager.instance.SubmitScore();
		}
	}

}
