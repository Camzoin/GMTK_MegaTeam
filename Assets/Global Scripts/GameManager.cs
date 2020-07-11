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

	private games currentGame;

	private float score = 0f;
	private Dictionary<games, float> scorePerGame = new Dictionary<games, float>();

	public static float sessionLength;
	private float startTime;

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

		//GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		//gm.ScorePoints(GameManager.games.WhackAMole, 1f);

	}

	// Start is called before the first frame update
	void Start()
    {
		startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time > startTime + sessionLength)
		{
			//end the game / go to a summary screen and submit score to leader board
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
		SceneManager.LoadScene(gameSceneNumbers[currentGame]);
	}

	private void ResetScoring()
	{
		score = 0f;
		scorePerGame = new Dictionary<games, float>();

	}

	private void NextGame(games game)
	{
		sc.ChangeScene(gameSceneNumbers[game]);
	}

}
