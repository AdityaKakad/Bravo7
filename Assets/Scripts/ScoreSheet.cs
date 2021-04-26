using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSheet : MonoBehaviour
{
	static Highscore[] highscoreFields;
	public static Dictionary<string, Highscore> highscores = new Dictionary<string, Highscore>();
	public static Highscores highscoresManager;

	void Start()
	{
		highscoresManager = GetComponent<Highscores>();
		StartCoroutine("RefreshHighscores");
	}

	public void OnHighscoresDownloaded(Highscore[] highscoreList)
	{
		highscoreFields = highscoreList;
		highscores.Clear();
		foreach(Highscore hs in highscoreList)
        {
            if (!highscores.ContainsKey(hs.username))
            {
				highscores[hs.username] = hs;
			}
			else
            {
				if(highscores[hs.username].score < hs.score)
                {
					highscores[hs.username] = hs;
				}
            }
		}
	}

	IEnumerator RefreshHighscores()
	{
		while (true)
		{
			highscoresManager.DownloadHighscoresForScoreSheet();
			yield return new WaitForSeconds(10);
		}
	}


}
