using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
using System.Net;
using System.Text;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	private GameObject pauseMenu;
	private bool isPause = false;

	private GameObject sceneChangerObj;
	private SceneChanger sc;

	[SerializeField]
	private GameObject popinText;

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

	public Dictionary<games, string> popInText = new Dictionary<games, string>
	{
		{ games.LIMBO, "" },
		{ games.HIDEBRIDGE, "" },
		{ games.JUMPKING, "" },
		{ games.RUNNER, "" },
		{ games.OVERIT, "" },
		{ games.DHUNT, "Shoot bird!" },
		{ games.PONG, "" },
		{ games.STROOP, "" },
		{ games.AIMTRAIN, "" },
		{ games.TARGETS, "" },
		{ games.TANGLE, "" },
		{ games.TREADMILL, "" },
		{ games.WELL, "Get water!" },
		{ games.CARDFLIP, "Match two!" },
		{ games.UNDERTREE, "" },
		{ games.MAHJONG, "" },
		{ games.PACMAN, "" },
		{ games.COOKIE, "" },
		{ games.SPOTLIGHT, "" },
		{ games.TYPERACE, "Type!" },
		{ games.SUMO, "" },
		{ games.OSU, "Click the circles!" },
		{ games.TOILET, "Unroll!" },
		{ games.WHACKAMOLE, "Move hammer smash mole!" },
		{ games.SIMPLE, "" }
	};

	private List<games> workingGames = new List<games> { games.SUMO, games.WHACKAMOLE, games.JUMPKING, games.AIMTRAIN, games.TYPERACE, games.TOILET, games.STROOP, games.MAHJONG };

	private List<games> gamesQueue = new List<games>();
	//private List<float> gamesDurration = new List<float> { 30, 30, 28, 28, 26, 25, 23, 20, 18, 16, 15, 14, 13, 12, 10, 10, 9, 8, 7, 6, 5, 5 };
	private List<float> gamesDurration = new List<float> { 10, 10, 10, 10, 10 };
	private int currentGame = -1;

	private float score = 0f;
	private Dictionary<games, float> scorePerGame = new Dictionary<games, float>();

	private List<Dictionary<string, object>> scoreBoard;

	private bool scoreSubmitted = false;

	private bool sessionActive = false;
	private float startTime;
	private float thisGameStartTime;

	private bool needToShowMenuUI = false;

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

		sceneChangerObj = gameObject.transform.GetChild(2).gameObject;
		sc = sceneChangerObj.GetComponent<SceneChanger>();
	}

	// Start is called before the first frame update
	void Start()
	{
		ShowCorrectMenuStuff();
	}

	// Update is called once per frame
	void Update()
	{
		if (sessionActive)
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
		else
		{
			if (needToShowMenuUI)
			{
				ShowCorrectMenuStuff();
				needToShowMenuUI = false;
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

	public void ShowCorrectMenuStuff()
	{
		//If we're on the menu scene.
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			GameObject startMenu = GameObject.Find("StartMenu");
			GameObject endMenu = GameObject.Find("EndMenu");

			Debug.Log(endMenu.name);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			if (gamesQueue.Count == 0)
			{
				//Main menu
				startMenu.SetActive(true);
				endMenu.SetActive(false);
			}
			else
			{
				//end screen
				startMenu.SetActive(false);
				endMenu.SetActive(true);
				DownloadScores();
			}
		}
	}

	public void StartNewSession()
	{
		sessionActive = true;
		currentGame = -1;
		score = 0f;
		scorePerGame = new Dictionary<games, float>();
		startTime = Time.time;
		scoreSubmitted = false;

		//generate game queue
		for (int i = 0; i < gamesDurration.Count; i++)
		{
			//pick a random game in the working list
			games nextGame = workingGames[UnityEngine.Random.Range(0, workingGames.Count)];

			//make sure we don't get the same game twice in a row.
			while (i > 0 && nextGame == gamesQueue[i - 1])
			{
				nextGame = workingGames[UnityEngine.Random.Range(0, workingGames.Count)];
			}

			gamesQueue.Add(nextGame);
			Debug.Log("Game: " + i + " Name: " + gamesQueue[i].ToString() + " Durration: " + gamesDurration[i].ToString());
		}

		Debug.Log("durration list: " + gamesDurration.Count + " queue count: " + gamesQueue.Count);

		NextGame();
	}

	private void NextGame()
	{
		currentGame++;

		if (currentGame == gamesQueue.Count)
		{
			//if we have finished the game queue go back to the menu
			//need to implement score screen
			//End the game go to menu.
			sc.ChangeScene(0);
			ShowCorrectMenuStuff();
			sessionActive = false;
			needToShowMenuUI = true;
		}
		else
		{
			Debug.Log("current game: " + currentGame);
			Debug.Log("current game: " + gamesQueue[currentGame]);
			Debug.Log("Game scene num: " + gameSceneNumbers[gamesQueue[currentGame]]);

			sc.ChangeScene(gameSceneNumbers[gamesQueue[currentGame]]);
			StartCoroutine(DelayEnable(popinText, 1.4f, true));
			thisGameStartTime = Time.time;
		}
	}

	public void SubmitScore()
	{
		//Only allows 1 submission per session. (Gets reset when new session is started.)
		if (!scoreSubmitted)
		{
			scoreSubmitted = true;
			//Needs to get the name entered and reject if not alphanumeric.
			UploadScore("TTT", score, gamesQueue);
		}
	}

	private void UploadScore(string name, float score, List<games> gameList)
	{
		string uniqueID = name + UnityEngine.Random.Range((int)0, (int)10000) + "-" + UnityEngine.Random.Range((int)0, (int)10000);

		//Get the object used to communicate with the server.
		FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://35.163.108.180/" + uniqueID + ".csv");
		request.Method = WebRequestMethods.Ftp.UploadFile;

		request.Credentials = new NetworkCredential("GMTKGameClientUser", "BlazeIt420");

		//Generate string to store list of games played in the session using '|' as the delimiter.
		string gamesListString = "'";
		foreach (games gameN in gameList)
		{
			gamesListString += gameN.ToString() + "|";
		}
		gamesListString = gamesListString.TrimEnd('|');
		gamesListString += "'";

		//Generate location string.
		string externalip = new WebClient().DownloadString("http://icanhazip.com");
		string locString = CityStateCountByIp(externalip);

		string dataString = name + "," + System.DateTime.UtcNow + "," + score.ToString() + "," + gamesListString + "," + locString;

		byte[] fileContents;

		using (var ms = new MemoryStream())
		{
			TextWriter tw = new StreamWriter(ms);
			tw.Write("player_name,time_stamp,score,games,location\n\n");
			tw.Write(dataString);
			tw.Flush();
			ms.Position = 0;
			fileContents = ms.ToArray();
		}

		request.ContentLength = fileContents.Length;

		using (Stream requestStream = request.GetRequestStream())
		{
			requestStream.Write(fileContents, 0, fileContents.Length);
		}

		using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
		{
			Debug.Log("Upload File Complete, status " + response.StatusDescription);
		}
	}

	public void DownloadScores()
	{
		WebClient request = new WebClient();
		string url = "http://35.163.108.180/ScoreBoard.csv";

		try
		{
			byte[] newFileData = request.DownloadData(url);
			string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
			scoreBoard = CSVReader.Read(fileString);

			DisplayScores();
		}
		catch (WebException e)
		{
			Debug.Log(e.ToString());
		}
	}

	public void DisplayScores()
	{
		foreach (Dictionary<string, object> record in scoreBoard)
		{
			foreach (string key in record.Keys)
			{
				Debug.Log(key + ": " + record[key]);
			}
		}
	}

	private IEnumerator DelayEnable(GameObject gameObject, float delay, bool enabled)
	{
		float timer = 0;

		while (timer < delay)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		gameObject.SetActive(enabled);
	}

	public static string CityStateCountByIp(string IP)
	{
		string url = "http://api.ipstack.com/" + IP + "?access_key=1be2ededaf9892410b423aad6633bc65";
		var request = System.Net.WebRequest.Create(url);

		using (WebResponse wrs = request.GetResponse())
		using (Stream stream = wrs.GetResponseStream())
		using (StreamReader reader = new StreamReader(stream))
		{
			string jsonString = reader.ReadToEnd();
			var obj = JsonUtility.FromJson<Root>(jsonString);
			//var obj = JObject.Parse(json);
			string City = (string)obj.city;
			string State = (string)obj.region_name;
			string Country = (string)obj.country_name;

			return (City + " " + State + " " + Country);
		}
		return "Not Sure Where...";
	}
}

public class Language
{
	public string code { get; set; }
	public string name { get; set; }
	public string native { get; set; }

}

public class Location
{
	public int geoname_id { get; set; }
	public string capital { get; set; }
	public List<Language> languages { get; set; }
	public string country_flag { get; set; }
	public string country_flag_emoji { get; set; }
	public string country_flag_emoji_unicode { get; set; }
	public string calling_code { get; set; }
	public bool is_eu { get; set; }

}

public class Root
{
	public string ip { get; set; }
	public string type { get; set; }
	public string continent_code { get; set; }
	public string continent_name { get; set; }
	public string country_code { get; set; }
	public string country_name { get; set; }
	public string region_code { get; set; }
	public string region_name { get; set; }
	public string city { get; set; }
	public string zip { get; set; }
	public double latitude { get; set; }
	public double longitude { get; set; }
	public Location location { get; set; }
}