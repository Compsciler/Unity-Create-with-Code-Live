using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Source: https://www.youtube.com/watch?v=KZuqEyxYZCc&t=616s

public class LeaderboardManager : MonoBehaviour
{
	internal static LeaderboardManager instance;

	private string[] privateCodes = {
										"XH-C7irxHU69Du5NSODUIAPcassrlFlkKAaSWvS4Kbqw",
										"twf8XCfbREq9JkpLmy_X_gPoo9nbmWbk-xExOodKpWYw",
										"QExs4kDVL0etrqFUNqMa6QwjbqTqUIMk-kTdJ83wK-Vw",
										"VngGAYUZf0CM2oJfL23Ehwvji82GEQQ0aGe3xfmBs-fw",
										"DPiaLrq-GUWxthSDy7B9Ogen3SNSVYlUq2CeTIoXXNQQ",
										"2WhenWoV9UmvZHPrAIWlUwAYp5xC2kpkenzcI-HXKqag",
										"Wlby2p9aO0mcZtNC7r28tQX7CK07qqk0mQ6RichtwVZw",
										"cYYLuViMakeo0uF5reXVsQjc6s-uHd3Eun9iSt3okMtg",
										"wmnuebpHbUe8eXXsTh5BEAST8X3mGzKEuY6Uc-iq_IKQ"  // Overall leaderboard
									};
	private string[] publicCodes =	{
										
										"5f2b94d3eb371809c4afbd01",
										"5f2b9be1eb371809c4afce15",
										"5f2b9c2beb371809c4afcecb",
										"5f2b9d05eb371809c4afd0fa",
										"5f2b9d1aeb371809c4afd132",
										"5f2b9e24eb371809c4afd3f4",
										"5f2b9f33eb371809c4afd6a9",
										"5f2ba0bbeb371809c4afdaaf",
										"5f2b9566eb371809c4afbe68"
									};
	const string webURL = "http://dreamlo.com/lb/";

	public HighScore[] onlineHighScores;

	internal static string username;

    void Awake()
    {
		instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
		AddNewHighScore("Sebastian", 50);
		AddNewHighScore("Mary", 85);
		AddNewHighScore("Bob", 92);

		DownloadHighScores();
	}

	public void AddNewHighScore(string username, int score)
	{
		StartCoroutine(UploadNewHighScore(username, score));
	}

	IEnumerator UploadNewHighScore(string username, int score)
	{
		// Post?
		UnityWebRequest www = UnityWebRequest.Get(webURL + privateCodes[0] + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
		www.SendWebRequest();
		yield return www;

		if (string.IsNullOrEmpty(www.error))
        {
			Debug.Log("Upload Successful");
		}
		else
		{
			Debug.Log("Error uploading: " + www.error);
		}
	}

	public void DownloadHighScores()
	{
		StartCoroutine("DownloadHighscoresFromDatabase");
	}

	IEnumerator DownloadHighScoresFromDatabase()
	{
		UnityWebRequest www = UnityWebRequest.Get(webURL + publicCodes[0] + "/pipe/");
		yield return www;

		if (string.IsNullOrEmpty(www.error))
        {
			FormatHighScores(www.downloadHandler.text);
		}
		else
		{
			Debug.Log("Error Downloading: " + www.error);
		}
	}

	void FormatHighScores(string textStream)
	{
		string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		onlineHighScores = new HighScore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			onlineHighScores[i] = new HighScore(username, score);
			Debug.Log(onlineHighScores[i].username + ": " + onlineHighScores[i].score);
		}
	}

}

public struct HighScore
{
	public string username;
	public int score;

	public HighScore(string username, int score)
	{
		this.username = username;
		this.score = score;
	}
}
