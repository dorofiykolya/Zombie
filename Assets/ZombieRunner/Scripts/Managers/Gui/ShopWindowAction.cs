﻿using UnityEngine;
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
		public UILabel brains;

		public override void Initialize ()
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

			if(PowerUp.List[current].currentLevel == PowerUp.List[current].prices.Length)
			{
				price.text = "Done";
			}
			else
			{
				price.text = PowerUp.List[current].prices[PowerUp.List[current].currentLevel].ToString();
			}

			brains.text = PlayerData.Brains.ToString();
		}

		public override void GameStop ()
		{
			brains.text = PlayerData.Brains.ToString();
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

			PowerUp.List[current].currentLevel = Mathf.Min(PowerUp.List[current].currentLevel + 1, PowerUp.List[current].prices.Length);
			if(PowerUp.List[current].currentLevel == PowerUp.List[current].prices.Length)
			{
				price.text = "Done";
			}
			else
			{
				PlayerData.Brains -= PowerUp.List[current].prices[PowerUp.List[current].currentLevel - 1];
				price.text = PowerUp.List[current].prices[PowerUp.List[current].currentLevel].ToString();
			}

			brains.text = PlayerData.Brains.ToString();
		}
	}
}
