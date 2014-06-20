using UnityEngine;
using System.Collections;
namespace Runner
{
    public class CurrencyManager : ComponentManager 
	{
		public UILabel multi;
		public UILabel gold;
		public UILabel score;
		public UILabel fatman;

		public GameObject _eatBrains;

		public static int goldCount;
		private static GameObject eatBrains;
		private static float time;
		private float scoreCount;

		public override void Initialize ()
		{
			goldCount = 0;
			scoreCount = 0;

			multi.text = "x" + (PlayerData.missionMulti / 3);
			gold.text = "0";
			score.text = "0";

			fatman.text = "";

			eatBrains = _eatBrains;
			eatBrains.SetActive(false);
		}

		public override void GameRestart ()
		{
			PlayerData.SetBrains(goldCount);
			Initialize ();
		}

		public static void showEatBrains()
		{
			eatBrains.SetActive(true);
			time = Time.timeSinceLevelLoad + 1;
		}

		void Update()
		{
			scoreCount += (Player.Speed / Player.MinimumSpeed * ((1 + PlayerData.missionMulti / 3) + Player.GetMult()) * PowerUp.scorePowerup) / 5;
			score.text = ((int)scoreCount).ToString();
			multi.text = "x" + (((1 + PlayerData.missionMulti / 3) + Player.GetMult()) * PowerUp.scorePowerup);
			gold.text = goldCount.ToString ();

			if(Player.currentList[0].ID == 2 && Player.currentList.Count == 1 && !Player.isStop)
			{
				fatman.text = "Укуси кого-то или умрешь через: " + Mathf.Round(Player.collection[2].prefs[PlayerManager.levels[2]] - (Time.timeSinceLevelLoad - Player.currentList[0].bornTime));
			}
			else
			{
				fatman.text = "";
			}

			if (!eatBrains.activeSelf || time > Time.timeSinceLevelLoad)
				return;

			eatBrains.SetActive (false);
		}
	}
}
