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
		
		void OnDisable() 
		{
			movement.speed = startSpeed;
			gameObject.collider.enabled = true;
			GetComponent<ObstacleAnimation> ().idle ();
		}
    }
}
