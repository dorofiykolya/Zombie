using UnityEngine;
using System.Collections;

namespace Runner
{
	public class TutorialAction : MonoBehaviour 
	{
		public const string HUMANS = "001";
		public const string	SLIDE_DOWN = "002";
		public const string	SLIDE_UP = "004";
		public const string	SLIDE_SIDES = "003";

		public GameObject[] _tutorialScenes;
		private static GameObject[] tutorialScenes;
		private static GameObject current;

		void Awake()
		{
			tutorialScenes = _tutorialScenes;
		}

        void OnEnable()
        {
            hideTutorial();
        }

		public static void showTutorial(string type)
		{
			for(int i = 0; i < tutorialScenes.Length; i++)
			{
				if(tutorialScenes[i].name == type)
				{
					if(current != null)
						current.SetActive(false);
					current = tutorialScenes[i];
					current.SetActive(true);
					return;
				}
			}
		}

		public static void hideTutorial()
		{
            if (current != null)
            {
                current.SetActive(false);
            }
		}
	}
}
