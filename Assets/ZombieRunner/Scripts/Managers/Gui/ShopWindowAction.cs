using UnityEngine;
using System.Collections;
namespace Runner
{
	public enum ShopAction
	{
		CoinPack1,
		CoinPack2,
		CoinPack3,
		CoinPack4,
		CoinPack5,
		SuperBrain,
		Magnet,
		Hoverboard,
		Score,
		Explosive
	}

	public class ShopWindowAction : ComponentManager 
	{
		public ShopAction action;
		public UILabel price;
		public UILabel desc;

		private string descText;

		void Start ()
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
				case ShopAction.Explosive:
					current = 3;
					break;
			}

			if(current == -1)
				return;

			if(PowerUpManager.levels[current] == PowerUp.List[current].prices.Length)
			{
				price.text = "Готово";
			}
			else
			{
				price.text = PowerUp.List[current].prices[PowerUpManager.levels[current]].ToString();
			}

			if (desc == null)
				return;
			
			descText = desc.text;
			
			if(current != 3)
				desc.text = descText + " (длится " + PowerUp.List [current].effect [PowerUpManager.levels[current]] + " сек)";
			else
				desc.text = descText + " (" + PowerUp.List [current].effect [PowerUpManager.levels[current]] + " м)";
		}

		void OnClick()
		{
			int current = -1;

			switch(action)
			{
				case ShopAction.CoinPack1:
					break;
				case ShopAction.CoinPack2:
					break;
				case ShopAction.CoinPack3:
					break;
				case ShopAction.CoinPack4:
					break;
				case ShopAction.CoinPack5:
					break;
				case ShopAction.Magnet:
					current = 0;
					break;
				case ShopAction.Hoverboard:
					current = 1;
					break;
				case ShopAction.Score:
					current = 2;
					break;
				case ShopAction.Explosive:
					current = 3;
					break;
			}

			if(current == -1)
				return;

			if(PowerUpManager.levels[current] == PowerUp.List[current].prices.Length) return;

			if(PlayerData.SetBrains(-PowerUp.List[current].prices[PowerUpManager.levels[current]]))
			{
				PowerUpManager.levels[current] = Mathf.Min(PowerUpManager.levels[current] + 1, PowerUp.List[current].prices.Length);
				if(PowerUpManager.levels[current] == PowerUp.List[current].prices.Length)
				{
					price.text = "Готово";
				}
				else
				{
					price.text = PowerUp.List[current].prices[PowerUpManager.levels[current]].ToString();
				}
			}

			if(current != 3)
				desc.text = descText + " (длится " + PowerUp.List [current].effect [PowerUpManager.levels[current]] + " сек)";
			else
				desc.text = descText + " (" + PowerUp.List [current].effect [PowerUpManager.levels[current]] + " м)";

			StorageManager.Save ();
			Audio.PlaySound (17);
		}
	}
}
