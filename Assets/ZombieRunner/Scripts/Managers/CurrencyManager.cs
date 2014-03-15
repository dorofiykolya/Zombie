using UnityEngine;
using System.Collections;
namespace Runner
{
	public class CurrencyManager : MonoBehaviour 
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
			score.text = Mathf.Round(PlayerManager.Distance / PlayerManager.MinimumSpeed * currentMult).ToString();
			multi.text = "x" + (currentMult + PlayerManager.GetMult());
			gold.text = goldCount.ToString ();
		}
	}
}
