using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	private GameObject pauseMenu;
	private bool isPause = false;

	private GameObject sceneChangerObj;
	private SceneChanger sc;

	public enum games { LIMBO, HIDEBRIDGE, JUMPKING, RUNNER, OVERIT, DHUNT, PONG, STROOP, AIMTRAIN, TARGETS, TANGLE, TREADMILL, WELL, CARDFLIP, UNDERTREE, MAHJONG, PACMAN, COOKIE, SPOTLIGHT, TYPERACE, SUMO, OSU, TOILET, WHACKAMOLE, SIMPLE };
	public Dictionary<games, int> gameSceneNumbers = new Dictionary<games, int>
	{
		//Menu Scene: 0
		{ games.LIMBO, 1 },
		{ games.HIDEBRIDGE, 2 },
		{ games.JUMPKING, 3 },
		{ games.RUNNER, 4 },
		{ games.OVERIT, 5 },
		{ games.DHUNT, 6 },
		{ games.PONG, 7 },
		{ games.STROOP, 8 },
		{ games.AIMTRAIN, 9 },
		{ games.TARGETS, 10 },
		{ games.TANGLE, 11 },
		{ games.TREADMILL, 12 },
		{ games.WELL, 13 },
		{ games.CARDFLIP, 14 },
		{ games.UNDERTREE, 15 },
		{ games.MAHJONG, 16 },
		{ games.PACMAN, 17 },
		{ games.COOKIE, 18 },
		{ games.SPOTLIGHT, 19 },
		{ games.TYPERACE, 20 },
		{ games.SUMO, 21 },
		{ games.OSU, 22 },
		{ games.TOILET, 23 },
		{ games.WHACKAMOLE, 24 },
		{ games.SIMPLE, 25 }
	};

	private List<games> workingGames = new List<games> { games.SUMO, games.WHACKAMOLE, games.JUMPKING, games.AIMTRAIN, games.TYPERACE, games.TOILET};

	private List<games> gamesQueue = new List<games>();
	private List<float> gamesDurration = new List<float> { 30, 30, 28, 28, 26, 25, 23, 20, 18, 16, 15, 14, 13, 12, 10, 10, 9, 8, 7, 6, 5, 5 };
	private int currentGame = -1;

	private float score = 0f;
	private Dictionary<games, float> scorePerGame = new Dictionary<games, float>();

	private float startTime;
	private float thisGameStartTime;

	private void Awake()
	{
		//Make sure this is the only Game Manager
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

		GameObject existingGameManager = GameObject.Find("GameManager");
		if (existingGameManager != gameObject)
		{
			Destroy(gameObject);
			return;
		}

		pauseMenu = gameObject.transform.GetChild(0).gameObject;
		pauseMenu.SetActive(false);

		sceneChangerObj = gameObject.transform.GetChild(1).gameObject;
		sc = sceneChangerObj.GetComponent<SceneChanger>();
	}

	// Start is called before the first frame update
	void Start()
    {
		//Cursor.visible = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (currentGame > -1)
		{
			if (Time.time >= thisGameStartTime + gamesDurration[currentGame])
			{
				NextGame();
			}
		}
		

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!isPause)
			{
				//Pause Menu Needs:
				//Abandon Run - Back to menu
				//Exit - Close Game
				//Volume - Mute/Unmute & Slider
				//Display Score
				isPause = true;
				pauseMenu.SetActive(true);

			}
			else
			{
				isPause = false;
				pauseMenu.SetActive(false);
			}
		}

    }

	public float GetTotalScore()
	{
		return score;
	}

	public float GetScore(games game)
	{
		if (scorePerGame.ContainsKey(game))
		{
			return scorePerGame[game];
		}
		else
		{
			return 0f;
		}
	}

	public void ScorePoints(games game, float points)
	{
		score += points;

		if (scorePerGame.ContainsKey(game))
		{
			scorePerGame[game] += points;
		}
		else
		{
			scorePerGame.Add(game, points);
		}
	}

	public void RestartThisGame()
	{
		SceneManager.LoadScene(gameSceneNumbers[gamesQueue[currentGame]]);
	}

	public void StartNewSession()
	{
		currentGame = -1;
		score = 0f;
		scorePerGame = new Dictionary<games, float>();
		startTime = Time.time;

		//generate game queue
		for (int i = 0; i < gamesDurration.Count; i++)
		{
			//pick a random game in the working list
			games nextGame = workingGames[Random.Range(0, workingGames.Count)];

			//make sure we don't get the same game twice in a row.
			//while (i > 0 && nextGame == gamesQueue[i - 1])
			//{
			//	nextGame = workingGames[Random.Range(0, workingGames.Count)];
			//}

			gamesQueue.Add(nextGame);
			Debug.Log("Game: " + i + " Name: " + gamesQueue[i].ToString() + " Durration: " + gamesDurration[i].ToString());
		}

		NextGame();
	}

	private void NextGame()
	{
		currentGame++;
		sc.ChangeScene(gameSceneNumbers[gamesQueue[currentGame]]);
		thisGameStartTime = Time.time;
	}

}
