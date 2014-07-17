using UnityEngine;
using System.Collections;

namespace Runner
{
	public enum CharacterAction
	{
		Player1,
		Player2,
		Player3,
		Player4,
		Player5,
		Upgrade
	}

    public class PlayerValues : ComponentManager 
	{
		[HideInInspector]
		public static int player = 0;
		private static Renderer currentRenderer;

		public CharacterAction action;
		public Transform stars;
		public Renderer humanRenderer;
		public UILabel desc;
		public UILabel price;

		void Start ()
		{
			desc.text = "Увеличивает лимит зомби в группе\r\n(Макс. кол-во зомби: " + Player.collection[player].prefs[PlayerManager.levels[player]] + ")";
			price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();

			int i;
			for(i = 0; i < PlayerManager.levels[player]; i++)
			{
				stars.GetChild(i).gameObject.SetActive(true);
			}
			for(i = i; i < stars.childCount; i++)
			{
				stars.GetChild(i).gameObject.SetActive(false);
			}
		}

		void OnClick()
		{
			switch(action)
			{
				case CharacterAction.Player1:
					player = 0;
					Player.Change(player);
					break;
				case CharacterAction.Player2:
					player = 1;
					Player.Change(player);
					break;
				case CharacterAction.Player3:
					player = 2;
					Player.Change(player);
					break;
				case CharacterAction.Player4:
					player = 3;
					Player.Change(player);
					break;
				case CharacterAction.Player5:
					player = 4;
					Player.Change(player);
					break;
				case CharacterAction.Upgrade:
					if(PlayerManager.levels[player] == 5)
						return;

					if(!PlayerData.SetBrains(-Player.collection[player].prices[PlayerManager.levels[player]]))
						return;

					PlayerManager.levels[player] = Mathf.Clamp(PlayerManager.levels[player] + 1, 0, 5);
					
					switch(player)
					{
						case 0:
							Missions.Dispatch ("upgradeandy", PlayerManager.levels[player]);
							break;
						case 1:
							Missions.Dispatch ("upgradejessy", PlayerManager.levels[player]);
							break;
						case 2:
							if(PlayerManager.levels[player] == 1)
							{
								Player.Change(player);
								Missions.Dispatch ("unlockbobby", PlayerManager.levels[player]);
							}
							else
								Missions.Dispatch ("upgradebobby", PlayerManager.levels[player]);
							break;
						case 3:
							if(PlayerManager.levels[player] == 1)
							{
								Player.Change(player);
								Missions.Dispatch ("unlockdrwhite", PlayerManager.levels[player]);
							}
							else
								Missions.Dispatch ("upgradedrwhite", PlayerManager.levels[player]);
							break;
						case 4:
							if(PlayerManager.levels[player] == 1)
							{
								Player.Change(player);
								Missions.Dispatch ("unlocksgtwall", PlayerManager.levels[player]);
							}
							else
								Missions.Dispatch ("upgradesgtwall", PlayerManager.levels[player]);
							break;
					}

					Audio.PlaySound (17);

					StorageManager.Save();

					break;
			}

			if(PlayerManager.levels[player] == 0)
			{
				humanRenderer.sharedMaterial.color = Color.black;
				currentRenderer = humanRenderer;
				desc.text = "";
			}
			else if(currentRenderer != null)
			{
				currentRenderer.sharedMaterial.color = Color.white;
			}

			int i;
			for(i = 0; i < PlayerManager.levels[player]; i++)
			{
				stars.GetChild(i).gameObject.SetActive(true);
			}
			for(i = i; i < stars.childCount; i++)
			{
				stars.GetChild(i).gameObject.SetActive(false);
			}

			switch(player)
			{
				case 0:
					desc.text = "Увеличивает лимит зомби в группе\r\n(Макс. кол-во зомби: " + Player.collection[player].prefs[PlayerManager.levels[player]] + ")";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 1:
					desc.text = "Добавляет значение к множителю очков\r\n(Добавляет: " + Player.collection[player].prefs[PlayerManager.levels[player]] + "x)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 2:
					desc.text = "Увеличивает на 2 кол-во подбираемых мозгов\r\n(Живет: " + Player.collection[player].prefs[PlayerManager.levels[player]] + "с)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 3:
					desc.text = "Увеличивает шанс появления бонусов\r\n(+" + Player.collection[player].prefs[PlayerManager.levels[player]] + "%)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 4:
					desc.text = "Ломает препятствия\r\n(Сила: " + Player.collection[player].prefs[PlayerManager.levels[player]] + "ударов)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
			}
		}
	}
}
