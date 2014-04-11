using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace Runner
{
    public class PlayerManager : ComponentManager 
	{
        public override void GameStart()
        {
            Distance = 0;
            PlayerData.PlatformTypeRemainingDistance = 0.0f;
            PlayerData.Distance = 0.0f;

            Camera.main.transform.parent = null;

            for (int i = 0; i < currentList.Count; i++)
            {
                Destroy(currentList[i].gameObject);
            }
            currentList.Clear();
            Select(PlayerData.CharacterId);
        }
		
		public void Move (float moveSpeed)
		{
			Distance += Mathf.Abs(moveSpeed);
			PlayerData.PlatformTypeRemainingDistance -= Mathf.Abs(moveSpeed);
			PlayerData.Distance = Distance;
            PlayerData.MaxDistance = Mathf.Max(PlayerData.MaxDistance, Distance);
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
		public bool isStop;

		public Vector3 defaultCameraPosition;

        public override void Initialize()
        {
            defaultCameraPosition = Camera.main.transform.position;
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
					return speed > 80 ? 80 : speed;
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
		
		public void Select(int id)
		{
            currentList.Add((Runner.PlayerController)GameObject.Instantiate(GetById(PlayerValues.player)));
			
			PlayerData.CharacterId = id;
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
				currentList[i].gameID = i;
			}

			Camera.main.transform.parent = currentList[0].gameObject.transform;
			Camera.main.transform.localPosition = defaultCameraPosition;
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
				return PlayerValues.player_2_prefs[PlayerValues.levels[1]];
			}
			
			return 0;
		}

		public int GetGoldBonus()
		{
			if(GetCurrentById(2).ID == 2)
			{
				return 1;
			}
			
			return 0;
		}

		public int GetMaxPlayers()
		{
			if(GetCurrentById(0).ID == 0)
			{
				return PlayerValues.player_1_prefs[PlayerValues.levels[0]];
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
		}
		
	}
}
