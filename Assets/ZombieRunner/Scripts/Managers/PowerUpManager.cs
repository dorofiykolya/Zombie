using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
	public class PowerUpManager : ComponentManager
	{
		public static GameObject PowerUpContainer{get;private set;}
		private static ObstaclePowerUp[] ListById;
		private static Dictionary<int,List<ObstaclePowerUp>> PoolById = new Dictionary<int, List<ObstaclePowerUp>>();
		private static int bonusChance;
		public int _bonusChance;
		public int _boomDistance;
		public ObstaclePowerUp[] List;

		public int scorePowerup{ get; private set; }

		public override void Initialize()
		{
			PowerUpContainer = new GameObject("PowerUpContainer");
			ListById = List;

			States.OnChanged += OnChanged;
		}
		
		public override void GameStop ()
		{
			States.OnChanged -= OnChanged;
		}

		public void OnChanged(State state)
		{
			if(state == State.GAME)
			{
				bonusChance = _bonusChance + PlayerValues.player_4_prefs[PlayerValues.levels[3]];
				scorePowerup = 1;
			}
		}

		public void UseBonus(ObstaclePowerUp p)
		{
			RemovePowerUpObject (p);
			Missions.Dispatch("pickupanybonus", 1);
			switch (p.Id)
			{
			case 1:
				StartCoroutine(Magnet(p));
				Missions.Dispatch("pickupmagnet", 1);
				if(Player.Current.ID == 1)
				{
					Missions.Dispatch("pickupmagnetbyjessy", 1);
				}
				break;
			case 2:
				break;
			case 3:
				Boom(p);
				Missions.Dispatch("pickupexplosive", 1);
				break;
			case 4:
				StartCoroutine(Flight(p));
				Missions.Dispatch("pickupboard", 1);
				break;
			case 5:
				StartCoroutine(ScoreBoost(p));
				Missions.Dispatch("pickupstar", 1);
				break;
			}
		}

		public IEnumerator Magnet(ObstaclePowerUp p)
		{
			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.magnet.enabled = true;
				}
			}
			yield return new WaitForSeconds (p.effect[p.currentLevel]);
			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.magnet.enabled = false;
				}
			}
		}

		public IEnumerator Flight(ObstaclePowerUp p)
		{
			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.OnPowerUpStarted();
				}
				else
				{
					player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -15);
				}
			}
			yield return new WaitForSeconds (p.effect[p.currentLevel]);
			foreach(PlayerController player in Player.currentList)
			{
				if(player.isPatientZero)
				{
					player.OnPowerUpEnded();
				}
				else
				{
					player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
				}
			}
		}

		public IEnumerator ScoreBoost(ObstaclePowerUp p)
		{
			scorePowerup = 2;
			yield return new WaitForSeconds (p.effect[p.currentLevel]);
			scorePowerup = 1;
		}

		public void Boom(ObstaclePowerUp p)
		{
			Collider[] boomCollider = Physics.OverlapSphere(new Vector3(0, 0, _boomDistance), p.effect[p.currentLevel], 1 << 12);

			foreach(Collider boom in boomCollider)
			{
				boom.transform.localScale = Vector3.zero;
				boom.gameObject.collider.enabled = false;
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
		}

		public static void AddPowerUpObjects(PlatformObject p)
		{
			foreach(Transform child in p.transform)
			{
				if(child.CompareTag("Bonus"))
				{
					if(Random.Range(1, 100) <= bonusChance)
					{
						ObstaclePowerUp o = PopPowerUp(Random.Range(0, ListById.Length));
						p.gameObject.AddChild(o.gameObject);
						o.gameObject.SetActive(true);
						o.transform.position = child.position;
					}
				}
			}
		}

		public static void RemovePowerUpObjects(PlatformObject p)
		{
			foreach (Transform child in p.transform)
			{
				if(child.CompareTag("PowerUp"))
				{
					RemovePowerUpObject(child.GetComponent<ObstaclePowerUp>());
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
