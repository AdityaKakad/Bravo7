using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Highscores : MonoBehaviour
{
	const string serverURL = "https://avian-casing-309102.wl.r.appspot.com/";

	DisplayHighscores highscoreDisplay;
	public Highscore[] highscoresList;
	static Highscores instance;
	string scene;
	string user;

	void Awake()
	{
		instance = this;
		instance.scene = SceneManager.GetActiveScene().name;
		if(instance.scene != "MainGame")
			highscoreDisplay = GetComponent<DisplayHighscores>();
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

	public static Highscore[] DownloadHighscoresForUser(string username)
	{
		string result = WebGet(username);
		return getFormattedHighscores(result);
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

	public static Highscore[] getFormattedHighscores(string textStream)
	{
		string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		Highscore[] userHighscores = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] { '|' });
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			userHighscores[i] = new Highscore(username, score);
			//print(highscoresList[i].username + ": " + highscoresList[i].score);
		}
		return userHighscores;
	}

	static string WebGet(string user)
	{
		Response result = new Response();
		IEnumerator e = GetStuff(result, user);

		// blocks here until UnityWebRequest() completes
		while (e.MoveNext()) ;

		Debug.Log(result.result);
		return result.result;
	}

	public class Response
	{
		public string result = "";
	}

	static IEnumerator GetStuff(Response res, string user)
	{
		UnityWebRequest www = UnityWebRequest.Get(serverURL + "user?user=" + user);

		yield return www.SendWebRequest();

		while (!www.isDone)
			yield return true;

		if (www.isNetworkError || www.isHttpError)
			res.result = www.error;
		else
			res.result = www.downloadHandler.text;
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