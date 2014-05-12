using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleHuman : ComponentManager
    {
		public ObstacleMovement movement;
		public int ID;

		private float startSpeed;

        public override void Initialize()
        {
            startSpeed = movement.speed;
        }

		void OnTriggerEnter(Collider other) 
		{
			if (other.gameObject.CompareTag("Obstacle"))
			{
				movement.Stop();
				GetComponent<ObstacleAnimation> ().death ();
			}
		}
		
		void OnDisable() 
		{
			StopAllCoroutines ();
			movement.speed = startSpeed;
			gameObject.collider.enabled = true;
			GetComponent<ObstacleAnimation> ().idle ();
		}
    }
}
