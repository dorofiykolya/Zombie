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

	public class PlayerValues : MonoBehaviour 
	{
		public static int[] levels = new int[]{0, 0, 0, 0, 0};
		public static int[] player_1_prefs = new int[]{2, 3, 4, 5, 6};
		public static int[] player_2_prefs = new int[]{1, 2, 3, 4, 5};
		public static int[] player_3_prefs = new int[]{10, 20, 30, 40, 50};

		public static int player = 0;

		public CharacterAction action;
		public Transform stars;

		void OnClick()
		{
			switch(action)
			{
				case CharacterAction.Player1:
					player = 0;
					break;
				case CharacterAction.Player2:
					player = 1;
					break;
				case CharacterAction.Player3:
					player = 2;
					break;
				case CharacterAction.Player4:
					player = 3;
					break;
				case CharacterAction.Player5:
					player = 4;
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
