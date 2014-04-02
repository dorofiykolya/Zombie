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

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Obstacle"))
			{
				transform.localPosition = Vector3.zero;
			}
		}
		
		void OnDisable() 
		{
			movement.speed = startSpeed;
			gameObject.collider.enabled = true;
			GetComponent<ObstacleAnimation> ().idle ();
		}
    }
}
