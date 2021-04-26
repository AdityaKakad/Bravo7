using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Highscores : MonoBehaviour
{
	const string serverURL = "https://avian-casing-309102.wl.r.appspot.com/";

	DisplayHighscores highscoreDisplay;
	ScoreSheet scoreSheet;
	public Highscore[] highscoresList;
	public Highscore[] highscores;
	static Highscores instance;
	string scene;

	void Awake()
	{
		instance = this;
		instance.scene = SceneManager.GetActiveScene().name;
		if(instance.scene != "MainGame" && instance.scene != "Settings")
			highscoreDisplay = GetComponent<DisplayHighscores>();
		if (instance.scene == "Settings")
			scoreSheet = GetComponent<ScoreSheet>();
	}

	public static void AddNewHighscore(string username, int score)
	{
		instance.StartCoroutine(instance.UploadNewHighscore(username, score));
	}

	IEnumerator UploadNewHighscore(string username, int score)
	{
		//WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
		WWW www = new WWW(serverURL + "send" + "?name=" + WWW.EscapeURL(username) + "&score=" + score);
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			print("Upload Successful");
			DownloadHighscores();
		}
		else
		{
			print("Error uploading: " + www.error);
		}
	}

	public void DownloadHighscores()
	{
		StartCoroutine("DownloadHighscoresFromDatabase");
	}

	public void DownloadHighscoresForScoreSheet()
	{
		StartCoroutine("DownloadHighscoresFromDatabaseServer");
	}

	IEnumerator DownloadHighscoresFromDatabase()
	{
		WWW www = new WWW(serverURL+"receive");
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			FormatHighscores(www.text);
			if(highscoreDisplay!=null) highscoreDisplay.OnHighscoresDownloaded(highscoresList);
		}
		else
		{
			print("Error Downloading: " + www.error);
		}
	}

	IEnumerator DownloadHighscoresFromDatabaseServer()
	{
		WWW www = new WWW(serverURL + "receive");
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			getFormattedHighscores(www.text);
			if (scoreSheet != null) scoreSheet.OnHighscoresDownloaded(highscores);
		}
		else
		{
			print("Error Downloading: " + www.error);
		}
	}

	void FormatHighscores(string textStream)
	{
		string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] { '|' });
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username, score);
			//print(highscoresList[i].username + ": " + highscoresList[i].score);
		}
	}

	void getFormattedHighscores(string textStream)
	{
		string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		highscores = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] { '|' });
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscores[i] = new Highscore(username, score);
			//print(highscoresList[i].username + ": " + highscoresList[i].score);
		}
	}

}

public struct Highscore
{
	public string username;
	public int score;

	public Highscore(string _username, int _score)
	{
		username = _username;
		score = _score;
	}

}