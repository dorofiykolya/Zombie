using UnityEngine;
using System.Collections;

namespace Runner
{
	public class ObstacleHuman : MonoBehaviour
    {
		public ObstacleMovement movement;
		public int ID;

		private float startSpeed;

		void Awake()
		{
			startSpeed = movement.speed;
		}
		
		void OnDisable() 
		{
			movement.speed = startSpeed;
			gameObject.collider.enabled = true;
			GetComponent<ObstacleAnimation> ().idle ();
		}
    }
}
