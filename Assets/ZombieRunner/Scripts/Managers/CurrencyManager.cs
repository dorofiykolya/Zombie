using UnityEngine;
using System.Collections;
namespace Runner
{
    public class CurrencyManager : ComponentManager 
	{
		public UILabel multi;
		public UILabel gold;
		public UILabel score;

		public static int goldCount;
		public static int currentMult;

		void Start()
		{
			currentMult = 1;
			goldCount = 0;

			multi.text = "x" + currentMult;
			gold.text = "0";
			score.text = "0";
		}

		void Update()
		{
			score.text = Mathf.Round(Player.Distance / Player.MinimumSpeed * (currentMult + Player.GetMult() + 5) * PowerUp.scorePowerup).ToString();
            multi.text = "x" + (currentMult + Player.GetMult());
			gold.text = goldCount.ToString ();
		}
	}
}
