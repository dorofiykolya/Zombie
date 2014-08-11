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
		public UILabel revive;
		public UILabel brains;

		public GameObject eatBrains;

		public int goldCount;
		public int reviveCount;
		private static float time;
		private float scoreCount;

		public override void Initialize ()
		{
			goldCount = 0;
			scoreCount = 0;

			reviveCount = 250;

			multi.text = "x" + (PlayerData.missionMulti / 3);
			gold.text = "0";
			score.text = "0";

			fatman.text = "";

			eatBrains.SetActive(false);
		}

		public override void GameStop ()
		{
			if(scoreCount > PlayerData.Distance)
			{
				PlayerData.Distance = (int)scoreCount;
			}
			revive.text = reviveCount.ToString ();
		}

		public override void GameRestart ()
		{
			Initialize ();
		}

		public void showEatBrains(int price)
		{
			brains.text = "+" + price.ToString ();
			eatBrains.SetActive(true);
			time = Time.timeSinceLevelLoad + 1;
		}

		void Update()
		{
			if (States.Current != State.GAME)
				return;

			scoreCount += (Player.Speed / Player.MinimumSpeed * ((1 + PlayerData.missionMulti / 3) + Player.GetMult()) * PowerUp.scorePowerup) / 5;
			score.text = ((int)scoreCount).ToString();
			multi.text = "x" + (((1 + PlayerData.missionMulti / 3) + Player.GetMult()) * PowerUp.scorePowerup);
			gold.text = goldCount.ToString ();

			if(Player.currentList[0].ID == 2 && Player.currentList.Count == 1 && !Player.isStop)
			{
                if(Localization.language == "Russian")
                {
                    fatman.text = "Укуси кого-то или умрешь через: " + Mathf.Round(Player.collection[2].prefs[PlayerManager.levels[2]] - (Time.timeSinceLevelLoad - Player.currentList[0].bornTime));
                }
                else if(Localization.language == "English")
                {
                    fatman.text = "Bite someone or you will die in: " + Mathf.Round(Player.collection[2].prefs[PlayerManager.levels[2]] - (Time.timeSinceLevelLoad - Player.currentList[0].bornTime));
                }
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
