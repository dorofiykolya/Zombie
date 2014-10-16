using UnityEngine;
using System.Collections;

namespace Runner
{
	public class TutorialAction : MonoBehaviour 
	{
		public const string HUMANS = "009";
		public const string	SLIDE_DOWN = "003";
		public const string	SLIDE_UP = "002";
		public const string	SLIDE_SIDES = "001";
        public const string HUMAN1 = "004";
        public const string HUMAN2 = "005";
        public const string HUMAN3 = "006";
        public const string HUMAN4 = "007";
        public const string HUMAN5 = "008";

		public GameObject[] _tutorialScenes;
		private static GameObject[] tutorialScenes;
		private static GameObject current;

        public static bool tutorialShown;

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

                var Player = GameObject.Find("Game").GetComponent<PlayerManager>();

                if(current.name == HUMAN2)
                {
                    Player.currentList.Add((Runner.PlayerController)GameObject.Instantiate(Player.GetById(1)));
                    Player.currentList[Player.currentList.Count - 1].isPatientZero = false;
                    Player.currentList[Player.currentList.Count - 1].Initialize();
                    Player.currentList[Player.currentList.Count - 1].particle.Emit(50);

                    for(int i = 0; i < Player.currentList.Count - 1; i++)
                    {
                        Player.currentList[i].onDeath();
                        i--;
                    }
                    
                    var game = GameObject.FindGameObjectWithTag("Player");
                    Player.currentList[Player.currentList.Count - 1].gameObject.transform.parent = game.transform;
                }
                else  if(current.name == HUMAN3)
                {
                    Player.currentList.Add((Runner.PlayerController)GameObject.Instantiate(Player.GetById(2)));
                    Player.currentList[Player.currentList.Count - 1].isPatientZero = false;
                    Player.currentList[Player.currentList.Count - 1].Initialize();
                    Player.currentList[Player.currentList.Count - 1].particle.Emit(50);
                    
                    for(int i = 0; i < Player.currentList.Count - 1; i++)
                    {
                        Player.currentList[i].onDeath();
                        i--;
                    }
                    
                    var game = GameObject.FindGameObjectWithTag("Player");
                    Player.currentList[Player.currentList.Count - 1].gameObject.transform.parent = game.transform;
                }
                else  if(current.name == HUMAN4)
                {
                    Player.currentList.Add((Runner.PlayerController)GameObject.Instantiate(Player.GetById(3)));
                    Player.currentList[Player.currentList.Count - 1].isPatientZero = false;
                    Player.currentList[Player.currentList.Count - 1].Initialize();
                    Player.currentList[Player.currentList.Count - 1].particle.Emit(50);
                    
                    for(int i = 0; i < Player.currentList.Count - 1; i++)
                    {
                        Player.currentList[i].onDeath();
                        i--;
                    }
                    
                    var game = GameObject.FindGameObjectWithTag("Player");
                    Player.currentList[Player.currentList.Count - 1].gameObject.transform.parent = game.transform;
                }
                else  if(current.name == HUMAN5)
                {
                    Player.currentList.Add((Runner.PlayerController)GameObject.Instantiate(Player.GetById(4)));
                    Player.currentList[Player.currentList.Count - 1].isPatientZero = false;
                    Player.currentList[Player.currentList.Count - 1].Initialize();
                    Player.currentList[Player.currentList.Count - 1].particle.Emit(50);
                    
                    for(int i = 0; i < Player.currentList.Count - 1; i++)
                    {
                        Player.currentList[i].onDeath();
                        i--;
                    }
                    
                    var game = GameObject.FindGameObjectWithTag("Player");
                    Player.currentList[Player.currentList.Count - 1].gameObject.transform.parent = game.transform;
                }
            }
		}
	}
}
