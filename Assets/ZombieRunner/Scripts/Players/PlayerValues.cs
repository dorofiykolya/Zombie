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
		public static int player = 0;
		private static Renderer currentRenderer;

		public CharacterAction action;
		public Transform stars;
		public Renderer humanRenderer;
		public UILabel desc;
		public UILabel price;

		void Start ()
		{
			desc.text = "Andy, manager Increases the amount of zombies in party\r\n(Amount of Zombies: " + Player.collection[player].prefs[PlayerManager.levels[player]] + ")";
			price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();

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
					if(price.text == "Done")
						return;
					switch(player)
					{
						case 0:
							PlayerData.Brains -= Player.collection[player].prices[PlayerManager.levels[player]];
							Missions.Dispatch ("upgradeandy", PlayerManager.levels[player]);
							break;
						case 1:
							PlayerData.Brains -= Player.collection[player].prices[PlayerManager.levels[player]];
							Missions.Dispatch ("upgradejessy", PlayerManager.levels[player]);
							break;
						case 2:
							PlayerData.Brains -= Player.collection[player].prices[PlayerManager.levels[player]];
							if(PlayerManager.levels[player] == 1)
								Missions.Dispatch ("unlockbobby", PlayerManager.levels[player]);
							else
								Missions.Dispatch ("upgradebobby", PlayerManager.levels[player]);
							break;
						case 3:
							PlayerData.Brains -= Player.collection[player].prices[PlayerManager.levels[player]];
							if(PlayerManager.levels[player] == 1)
								Missions.Dispatch ("unlockdrwhite", PlayerManager.levels[player]);
							else
								Missions.Dispatch ("upgradedrwhite", PlayerManager.levels[player]);
							break;
						case 4:
							PlayerData.Brains -= Player.collection[player].prices[PlayerManager.levels[player]];
							if(PlayerManager.levels[player] == 1)
								Missions.Dispatch ("unlocksgtwall", PlayerManager.levels[player]);
							else
								Missions.Dispatch ("upgradesgtwall", PlayerManager.levels[player]);
							break;
					}

					PlayerManager.levels[player] = Mathf.Clamp(PlayerManager.levels[player] + 1, 0, 5);

					StorageManager.Save();

					break;
			}
			if(currentRenderer != null)
				currentRenderer.sharedMaterial.color = Color.white;

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
					desc.text = "Andy, manager Increases the amount of zombies in party\r\n(Amount of Zombies: " + Player.collection[player].prefs[PlayerManager.levels[player]] + ")";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 1:
					desc.text = "Jessy, housewife Increases the score multiplier\r\n(Multiplier: " + Player.collection[player].prefs[PlayerManager.levels[player]] + "x)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 2:
					desc.text = "Bobby, junk food lover Multiplies the amount of mutagen that he gather\r\n(Lifetime: " + Player.collection[player].prefs[PlayerManager.levels[player]] + "s)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 3:
					desc.text = "Dr. White, Activates the power-ups that he can find on his way\r\n(Power-ups chance: +" + Player.collection[player].prefs[PlayerManager.levels[player]] + "%)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
				case 4:
					desc.text = "Sergeant Wall, military Can break the obstacle\r\n(Power: " + Player.collection[player].prefs[PlayerManager.levels[player]] + "strikes)";
					price.text = PlayerManager.levels[player] == Player.collection[player].prices.Length ? "Done" : Player.collection[player].prices[PlayerManager.levels[player]].ToString();
					break;
			}

			if(PlayerManager.levels[player] == 0)
			{
				humanRenderer.sharedMaterial.color = Color.black;
				currentRenderer = humanRenderer;
				desc.text = "";
			}
		}
	}
}
