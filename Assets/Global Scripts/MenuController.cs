using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{

	public TextMeshProUGUI finalScore;

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
		GameManager.instance.SubmitScore();
	}
	

}
