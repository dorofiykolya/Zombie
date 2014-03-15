using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace Runner
{
	public class PlayerManager : MonoBehaviour 
	{
		public static void Restart ()
		{
			Distance = 0;
			PlayerData.PlatformTypeRemainingDistance = 0.0f;
			PlayerData.Distance = 0.0f;
			
			Camera.main.transform.parent = null;
			
			for(int i = 0; i < currentList.Count; i++)
			{
				Destroy(currentList[i].gameObject);
			}
			currentList.Clear();
			Select(PlayerData.CharacterId);
		}
		
		public static void Move (float moveSpeed)
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
		public float speedDistanceMultiply = 0.01f;
		public float sideScrollSpeed = 5;

        public static Runner.PlayerController[] collection;
        public static List<Runner.PlayerController> currentList = new List<PlayerController>();
		public static float Distance {get;private set;}
		public static float MinimumSpeed{get; private set;}
		public static float SpeedDistanceMultiply{get; private set;}
		public static float SideScrollSpeed{get;private set;}
		public static bool isStop;
		
		public static float Speed
		{
			get {return !isStop ? (Distance * SpeedDistanceMultiply) + MinimumSpeed : 0; }
		}
		
		public static Runner.PlayerController Player
		{
			get{
				if(currentList.Count == 0)
				{
					return null;	
				}
				return currentList[0];	
			}
		}
		
		public static void Select(int id)
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
			Camera.main.transform.localPosition = new Vector3(0, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
		}
		
		public static Runner.PlayerController GetById(int id)
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

		public static Runner.PlayerController GetCurrentById(int id)
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

		public static int GetMult()
		{
			if(GetCurrentById(1).ID == 1)
			{
				return PlayerValues.player_2_prefs[PlayerValues.levels[1]];
			}
			
			return 0;
		}

		public static int GetGoldBonus()
		{
			if(GetCurrentById(2).ID == 2)
			{
				return 1;
			}
			
			return 0;
		}

		public static int GetMaxPlayers()
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
