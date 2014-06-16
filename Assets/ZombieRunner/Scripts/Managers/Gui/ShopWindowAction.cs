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

		public override void Initialize ()
		{
			switch(action)
			{
				case ShopAction.Magnet:
					price.text = PowerUp.List[0].prices[PowerUp.List[0].currentLevel].ToString();
					break;
				case ShopAction.Hoverboard:
					price.text = PowerUp.List[1].prices[PowerUp.List[1].currentLevel].ToString();
					break;
				case ShopAction.Score:
					price.text = PowerUp.List[2].prices[PowerUp.List[2].currentLevel].ToString();
					break;
				case ShopAction.Explosive:
					price.text = PowerUp.List[3].prices[PowerUp.List[3].currentLevel].ToString();
					break;
			}
		}

		void OnClick()
		{
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
					break;
				case ShopAction.Hoverboard:
					break;
				case ShopAction.Score:
					break;
				case ShopAction.Explosive:
					break;
			}
		}
	}
}
