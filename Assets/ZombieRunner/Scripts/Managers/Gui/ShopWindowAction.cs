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

		void OnClick()
		{
			Debug.Log (action);
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
