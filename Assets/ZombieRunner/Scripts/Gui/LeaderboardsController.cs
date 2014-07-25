using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
	public class LeaderboardsController : MonoBehaviour 
	{
		private static float prevGetTime;

		public List<LeaderboardSlot> slots;

		private void StartGetLeaders()
		{
			if(prevGetTime < Time.timeSinceLevelLoad)
			{
				prevGetTime = Time.timeSinceLevelLoad + 10;
				StartCoroutine (GetLeaders ());
			}
		}

		void OnEnable()
		{
			if (PlayerData.playerID == null)
				return;

			//if(PlayerData.creation <= 0)
				StartCoroutine (CreatePlayer ());

			StartGetLeaders();
		}

		private IEnumerator CreatePlayer()
		{
			string query = string.Format("http://red-clove.com.ua/modules/runner_create.php?id={0}&score={1}&realname={2}", PlayerData.playerID, PlayerData.Distance, PlayerData.realName);
			WWW www = new WWW(query);
			yield return www;
			if(www.text == "1")
			{
				PlayerData.creation = 1;
				PlayerPrefs.SetInt("Creation", PlayerData.creation);
			}
		}

		private IEnumerator GetLeaders()
		{
			WWW www = new WWW("http://red-clove.com.ua/modules/runner_score.php?id="+PlayerData.playerID+"&score="+PlayerData.Distance);
			yield return www;
			var array = www.text.Split(new string[]{"playerPosition"}, System.StringSplitOptions.None);
			var jsonleaders = SimpleJSON.JSON.Parse (array[0]).AsArray;
			var jsonplayer = SimpleJSON.JSON.Parse (array[1]).AsArray;

			for(int i = 0; i < jsonleaders.Count; i++)
			{
				slots[i].score.text = jsonleaders[i]["score"];
				slots[i].name.text = jsonleaders[i]["realname"];
				slots[i].gameObject.SetActive(true);
			}

			if(int.Parse(jsonplayer[0]["COUNT(*)"]) > 10)
			{
				slots[10].score.text = PlayerData.Distance.ToString();
				slots[10].name.text = PlayerData.realName;
				slots[10].place.text = jsonplayer[0]["COUNT(*)"];
				slots[10].gameObject.SetActive(true);
			}

		}
	}
}
