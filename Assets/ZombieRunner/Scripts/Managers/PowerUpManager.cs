using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
	[Serializable]
	public class PowerUpVisual
	{
		[HideInInspector]
		public float time;
		public UILabel text;
		public GameObject visual;
	}

	public class PowerUpManager : ComponentManager
	{
		public static GameObject PowerUpContainer{get;private set;}
		private static ObstaclePowerUp[] ListById;
		private static Dictionary<int,List<ObstaclePowerUp>> PoolById = new Dictionary<int, List<ObstaclePowerUp>>();
		private static int bonusChance;
		public int _bonusChance;
		public int _boomDistance;
		public GameObject _boomPrefab;
		public ObstaclePowerUp[] List;
		public List<PowerUpVisual> visualList;

		public int scorePowerup{ get; private set; }

		public override void Initialize()
		{
			PowerUpContainer = new GameObject("PowerUpContainer");
			ListById = List;
			scorePowerup = 1;
			States.OnChanged += OnChanged;
		}
		
		public override void GameStop ()
		{
			for(int i = 0; i < visualList.Count; i++)
			{
				visualList[i].visual.SetActive(false);
			}

			StopAllCoroutines ();

			scorePowerup = 1;

			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.magnet.enabled = false;
				}
			}
		}

		public void OnChanged(State state)
		{
			if(state == State.GAME)
			{
				bonusChance = _bonusChance + Player.collection[3].prefs[PlayerManager.levels[3]];
			}
		}

		void Update()
		{
			if (Player.isStop)
				return;

			for(int i = 0; i < visualList.Count; i++)
			{
				if(visualList[i].time < Time.timeSinceLevelLoad)
				{
					if(visualList[i].visual.activeSelf)
					{
						if(i == 0)
						{
							foreach(PlayerController player in Player.currentList)
							{
								if(player.isPatientZero)
								{
									player.magnet.enabled = false;
								}
							}
						}
						else if(i == 1)
						{
							foreach(PlayerController player in Player.currentList)
							{
								if(player.isPatientZero)
								{
									player.OnPowerUpEnded();
								}
								else
								{
									player.TargetPosition = new Vector3(Player.currentList[0].transform.position.x, player.transform.position.y, 0);
								}
							}
						}
						else if(i == 2)
						{
							scorePowerup = 1;
						}
						visualList[i].visual.SetActive(false);
					}
					continue;
				}

				if(!visualList[i].visual.activeSelf)
					visualList[i].visual.SetActive(true);

				visualList[i].text.text = ((int)(visualList[i].time - Time.timeSinceLevelLoad)).ToString();
			}
		}

		public void UseBonus(ObstaclePowerUp p)
		{
			RemovePowerUpObject (p);
			if (p.currentLevel == 0)
				return;
			Missions.Dispatch("pickupanybonus", 1);
			switch (p.Id)
			{
			case 1:
				Magnet(p);
				Missions.Dispatch("pickupmagnet", 1);
				if(Player.Current.ID == 1)
				{
					Missions.Dispatch("pickupmagnetbyjessy", 1);
				}
				break;
			case 3:
				Boom(p);
				Missions.Dispatch("pickupexplosive", 1);
				break;
			case 4:
				Flight(p);
				Missions.Dispatch("pickupboard", 1);
				break;
			case 5:
				ScoreBoost(p);
				Missions.Dispatch("pickupstar", 1);
				break;
			}
		}

		public void Magnet(ObstaclePowerUp p)
		{
			visualList [0].time = Time.timeSinceLevelLoad + p.effect[p.currentLevel];

			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.magnet.enabled = true;
				}
			}
		}

		public void Flight(ObstaclePowerUp p)
		{
			visualList [1].time = Time.timeSinceLevelLoad + p.effect[p.currentLevel];

			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.OnPowerUpStarted();
					return;
				}
			}
		}

		public void ScoreBoost(ObstaclePowerUp p)
		{
			visualList [2].time = Time.timeSinceLevelLoad + p.effect[p.currentLevel];

			scorePowerup = 2;
		}

		public void Boom(ObstaclePowerUp p)
		{
			Collider[] boomCollider = Physics.OverlapSphere(new Vector3(0, 0, _boomDistance), p.effect[p.currentLevel], 1 << 12);

			foreach(Collider boom in boomCollider)
			{
				boom.transform.localScale = Vector3.zero;
				boom.gameObject.collider.enabled = false;
				Instantiate(_boomPrefab, boom.transform.position, Quaternion.identity);
				if(boom.gameObject.name.ToLower().Contains("red"))
				{
					Missions.Dispatch("dieredcar", 1);
				}
				if(Player.Current.ID == 1)
				{
					Missions.Dispatch("destroyobstaclesjessy", 1);
				}
				Missions.Dispatch("destroyexplosivecars", 1);
				Missions.Dispatch("destroyexplosive", 1);
			}
			Audio.PlaySound (14);
		}

		public static void AddPowerUpObjects(PlatformObject p)
		{
			foreach(Transform child in p.transform)
			{
				if(child.CompareTag("Bonus"))
				{
					if(UnityEngine.Random.Range(1, 100) <= bonusChance)
					{
						ObstaclePowerUp o = PopPowerUp(UnityEngine.Random.Range(0, ListById.Length));
						p.gameObject.AddChild(o.gameObject);
						o.gameObject.SetActive(true);
						o.transform.position = child.position;
					}
				}
			}
		}

		public static void RemovePowerUpObjects(PlatformObject p)
		{
			for (int i = 0; i < p.transform.childCount; i++)
			{
				if(p.transform.GetChild(i).CompareTag("PowerUp"))
				{
					RemovePowerUpObject(p.transform.GetChild(i).GetComponent<ObstaclePowerUp>());
					i--;
				}
			}
		}

		public static void RemovePowerUpObject(ObstaclePowerUp p)
		{
			PushPowerUp (p);
			p.gameObject.SetActive (false);
			PowerUpContainer.AddChild (p.gameObject);
		}

		public static ObstaclePowerUp PopPowerUp(int id)
		{
			List<ObstaclePowerUp> array;
			ObstaclePowerUp result = null;
			int count;
			if(PoolById.TryGetValue(id, out array))
			{
				count = array.Count;
				if(count > 0)
				{
					result = (ObstaclePowerUp)array[count - 1];
					array.RemoveAt(count - 1);
				}
			}
			if(result == null)
			{
				result = ListById[id].Clone();
			}
			return result;
		}
		
		public static void PushPowerUp(ObstaclePowerUp p)
		{
			List<ObstaclePowerUp> array;
			if(!PoolById.TryGetValue(1, out array))
			{
				array = new List<ObstaclePowerUp>();
				PoolById.Add(1, array);
			}
			p.gameObject.SetActive(false);
			array.Add(p);
		}
	}
}
