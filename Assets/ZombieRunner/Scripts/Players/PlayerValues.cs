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
		public static int[] levels = new int[]{0, 0, 0, 0, 0};
		public static int[] player_1_prefs = new int[]{2, 3, 4, 5, 6};
		public static int[] player_2_prefs = new int[]{1, 2, 3, 4, 5};
		public static int[] player_3_prefs = new int[]{10, 20, 30, 40, 50};
		public static int[] player_4_prefs = new int[]{5, 7, 9, 11, 13};
		public static int[] player_5_prefs = new int[]{1, 2, 3, 4, 5};

		public static int player = 0;

		public CharacterAction action;
		public Transform stars;
		public UILabel desc;

		void OnClick()
		{
			switch(action)
			{
				case CharacterAction.Player1:
					player = 0;
					desc.text = "Andy, manager Increases the amount of zombies in party";
					break;
				case CharacterAction.Player2:
					player = 1;
					desc.text = "Jessy, housewife Increases the score multiplier";
					break;
				case CharacterAction.Player3:
					player = 2;
					desc.text = "Bobby, junk food lover Multiplies the amount of mutagen that he gather";
					break;
				case CharacterAction.Player4:
					player = 3;
					desc.text = "Dr.White, he started this Activates the power-ups that he can find on his way";
					break;
				case CharacterAction.Player5:
					player = 4;
					desc.text = "Sergeant Wall, military Can break the obstacle";
					break;
				case CharacterAction.Upgrade:
					levels[player] = Mathf.Clamp(levels[player] + 1, 0, 4);
					break;
			}
			int i;
			for(i = 0; i <= levels[player]; i++)
			{
				stars.GetChild(i).gameObject.SetActive(true);
			}
			for(i = i; i < stars.childCount; i++)
			{
				stars.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
