using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace Runner
{
    public class PlayerManager : ComponentManager 
	{
        public override void GameRestart()
        {
            Distance = 0;
			isJumpPowerUp = false;
			isRevive = false;
			isStart = true;
            PlayerData.PlatformTypeRemainingDistance = 0.0f;

            Camera.main.transform.parent = null;

            for (int i = 0; i < currentList.Count; i++)
            {
                Destroy(currentList[i].gameObject);
            }
            currentList.Clear();
            Select(PlayerData.CharacterId);
        }

		public override void GameStop ()
		{
			isStop = true;
		}
		
		public void Move (float moveSpeed)
		{
			Distance += Mathf.Abs(moveSpeed);
			PlayerData.PlatformTypeRemainingDistance -= Mathf.Abs(moveSpeed);
		}
		
	
		public Runner.PlayerController[] list;
		public float distance;
		public float speed;
		public float minimumSpeed = 5.0f;
		public float speedDistanceMultiply = 1f;
		public float sideScrollSpeed = 5;

        public Runner.PlayerController[] collection;
        public List<Runner.PlayerController> currentList = new List<PlayerController>();
		public float Distance {get;private set;}
		public float MinimumSpeed{get; private set;}
		public float SpeedDistanceMultiply{get; private set;}
		public float SideScrollSpeed{get;private set;}
		[HideInInspector]
		public bool isStop;
		[HideInInspector]
		public bool isJumpPowerUp;
		[HideInInspector]
		public bool isRevive;
		[HideInInspector]
		public bool isStart;
		[HideInInspector]
		public Vector3 defaultCameraPosition;
		private Vector3 startCameraPosition = new Vector3(0, 10, -11);

		public static int[] levels;
		public static int[] defaultLevels = new int[]{1, 1, 0, 0, 0};

        public override void Initialize()
        {
            defaultCameraPosition = Camera.main.transform.position;
			isStart = true;
        }
		
		public float Speed
		{
			get 
			{
				if (isStop)
				{
					return 0;
				}
				else
				{
					float speed = (Distance * 0.01f * SpeedDistanceMultiply) + MinimumSpeed;
					if(Current.bInAir)
					{
						speed += MinimumSpeed * 0.1f;
						return Mathf.Clamp(speed, MinimumSpeed, 80 + MinimumSpeed * 0.1f);
					}
					if(isJumpPowerUp)
					{
						return 80;
					}
					return Mathf.Clamp(speed, MinimumSpeed, 80);
				}
			}
		}
		
		public Runner.PlayerController Current
		{
			get{
				if(currentList.Count == 0)
				{
					return null;	
				}
				return currentList[0];	
			}
		}

		public void Revive ()
		{
			isRevive = true;
			isStop = false;

			Waypoint.currentWP = Waypoint.transitWP;

			currentList[0].Revive ();

			Missions.Dispatch ("revive", 1);

			if(Current.ID == 1)
			{
				Missions.Dispatch("revivejessy", 1);
			}
		}

		public void Change(int id)
		{
			if (levels [id] == 0)
				return;

			if(currentList.Count > 0)
			{
				GameObject go = currentList[0].gameObject;

				currentList[0].GameStop();
				currentList[0] = (Runner.PlayerController)GameObject.Instantiate(GetById(id));

				PlayerData.CharacterId = id;
				var game = GameObject.FindGameObjectWithTag("Player");
				if(game == null)
				{
					game = new GameObject("Player");
					game.tag = "Player";
				}

				currentList[0].Initialize();
				currentList[0].gameObject.transform.parent = game.transform;

				Camera.main.transform.localPosition = startCameraPosition;

				Destroy(go);

				Audio.PlaySound (12);
			}
		}
		
		public void Select(int id)
		{
            currentList.Add((Runner.PlayerController)GameObject.Instantiate(GetById(PlayerData.CharacterId)));

			var game = GameObject.FindGameObjectWithTag("Player");
			if(game == null)
			{
				game = new GameObject("Player");
				game.tag = "Player";
			}

			for(int i = 0; i < currentList.Count; i++)
			{
				if(i != 0)
				{
					currentList[i].isPatientZero = false;
				}
				currentList[i].Initialize();
				currentList[i].gameObject.transform.parent = game.transform;
			}

			Camera.main.transform.localPosition = startCameraPosition;
		}
		
		public Runner.PlayerController GetById(int id)
		{
			foreach(var a in collection)
			{
				if(a.ID == id)
				{
					return a;
				}
			}
			return collection[0];
		}

		public Runner.PlayerController GetCurrentById(int id)
		{
			foreach(var a in currentList)
			{
				if(a.ID == id)
				{
					return a;
				}
			}
			return currentList[0];
		}

		public int GetMult()
		{
			if(GetCurrentById(1).ID == 1)
			{
				return Player.collection[1].prefs[PlayerManager.levels[1]];
			}
			
			return 0;
		}

		public int GetGoldBonus()
		{
			if(GetCurrentById(2).ID == 2)
			{
				return 2;
			}
			
			return 1;
		}

		public int GetMaxPlayers()
		{
			if(GetCurrentById(0).ID == 0)
			{
				return Player.collection[0].prefs[PlayerManager.levels[0]];
			}

			return 2;
		}
		
		void Start()
		{
			collection = list;
			distance = 0.0f;
			SideScrollSpeed = sideScrollSpeed;
			Select(PlayerData.CharacterId);
		}
		
		void Update()
		{
			this.distance = Distance;
			this.speed = Speed;
			SideScrollSpeed = this.sideScrollSpeed;
			MinimumSpeed = this.minimumSpeed;
			SpeedDistanceMultiply = this.speedDistanceMultiply;

			if(States.Current == State.GAME && isStart)
			{
				if(Camera.main.transform.localPosition != defaultCameraPosition)
				{
					Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.position, defaultCameraPosition, 20f * Time.deltaTime);
				}
				else
				{
					isStart = false;
				}
			}
		}
		
	}
}
