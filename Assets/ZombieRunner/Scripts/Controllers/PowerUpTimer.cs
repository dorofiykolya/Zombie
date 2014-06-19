using UnityEngine;
using System.Collections;

namespace Runner
{
	public class PowerUpTimer : ComponentManager
	{
		public ShopAction action;
		private float time;

		void OnEnable()
		{
			int current = -1;
			
			switch(action)
			{
				case ShopAction.Magnet:
					current = 0;
					break;
				case ShopAction.Hoverboard:
					current = 1;
					break;
				case ShopAction.Score:
					current = 2;
					break;
			}

			time = Time.timeSinceLevelLoad + PowerUp.List [current].effect [PowerUp.List [current].currentLevel];
		}

		// Update is called once per frame
		void Update () 
		{
			gameObject.GetComponent<UILabel> ().text = ((int)(time - Time.timeSinceLevelLoad)).ToString();
		}
	}
}
