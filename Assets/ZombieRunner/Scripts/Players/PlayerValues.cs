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

        private string[] russian = new string[]{
            "Увеличивает лимит зомби в группе. Максимальное число зомби:",
            "Добавляет значение к множителю очков. Добавляет:",
            "Удваивает значение получаемых мозгов. Вы можете иметь только одного толстяка в группе. Живет (сек):",
            "Увеличивает шанс появления бонусов. Увеличивает на:",
            "Ломает препятствия. Ломает:"
        };

        private string[] english = new string[]{
            "Increases the amount of zombies in party. Max amount of zombies:",
            "Adds value to score multiplier. Adds:",
            "Doubles the value of brains that he gathers. You can have only one fatso at a time. Lives (sec):",
            "Increases the chance of power-ups. Increases on:",
            "Can break the obstacles. Breaks:"
        };

		void Start ()
		{
            if (Localization.language == "English")
            {
                desc.text = english[0] + " " + Player.collection[player].prefs[PlayerManager.levels[player]];
            } 
            else if (Localization.language == "Russian")
            {
                desc.text = russian[0] + " " + Player.collection[player].prefs[PlayerManager.levels[player]];
            }
			
			price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Готово" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();

			int i;
			for(i = 0; i < PlayerManager.levels[player]; i++)
			{
				stars.GetChild(i).gameObject.SetActive(true);
			}
			for(; i < stars.childCount; i++)
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
			for(; i < stars.childCount; i++)
			{
				stars.GetChild(i).gameObject.SetActive(false);
			}
            if (Localization.language == "English")
            {
                desc.text = english[player] + " " + Player.collection[player].prefs[PlayerManager.levels[player]];
                price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
            } 
            else if (Localization.language == "Russian")
            {
                desc.text = russian[player] + " " + Player.collection[player].prefs[PlayerManager.levels[player]];
                price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
            }
		}
	}
}
