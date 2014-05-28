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

		public static int goldCount;
		public static float scoreCount;
		public static int currentMult;

		public override void Initialize ()
		{
			currentMult = 1;
			goldCount = 0;
			scoreCount = 0;

			multi.text = "x" + currentMult;
			gold.text = "0";
			score.text = "0";

			fatman.text = "";
		}

		public override void GameStop ()
		{
			Initialize ();
		}

		void Update()
		{
			scoreCount += (Player.Speed / Player.MinimumSpeed * (currentMult + Player.GetMult ()) * PowerUp.scorePowerup) / 5;
			score.text = ((int)scoreCount).ToString();
			multi.text = "x" + (currentMult + Player.GetMult() + PowerUp.scorePowerup - 1);
			gold.text = goldCount.ToString ();

			if(Player.currentList[0].ID == 2 && Player.currentList.Count == 1 && !Player.isStop)
			{
				fatman.text = "Bite someone or you will die in: " + Mathf.Round(PlayerValues.player_3_prefs[PlayerValues.levels[2]] - (Time.timeSinceLevelLoad - Player.currentList[0].bornTime));
			}
			else
			{
				fatman.text = "";
			}
		}
	}
}
