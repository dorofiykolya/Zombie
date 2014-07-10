using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleMovement : ComponentManager 
	{
		private bool isTriggered;
		private Transform parent;
		private Vector3 parentCoord;

		public float speed;

		void Start()
		{
			parent = transform.parent;
			parentCoord = parent.localPosition;
		}

		void OnTriggerEnter(Collider other) 
		{
			if (other.gameObject.CompareTag ("Player")) 
			{
				isTriggered = true;
				collider.enabled = false;

				if(parent.name.ToLower().Contains("blue") || parent.name.ToLower().Contains("orange") || parent.name.ToLower().Contains("green"))
					Audio.PlaySound (5);
				else if(parent.name.ToLower().Contains("red"))
					Audio.PlaySound (4);
				else if(parent.name.ToLower().Contains("er"))
					Audio.PlaySound (11);
			}
			if(parent.CompareTag("Human"))
			{
				parent.GetComponent<ObstacleAnimation> ().run();
			}
		}

		public void Stop()
		{
			isTriggered = false;
			collider.enabled = false;
		}

		void Update()
		{
			if(isTriggered)
			{
				if (parent.localPosition.z <= parentCoord.z + transform.localPosition.z)
				{
					float distance = speed * (Player.Speed / Player.MinimumSpeed) * Time.deltaTime * PowerUp.magnetPowerup;
					parent.position = Vector3.MoveTowards(parent.position, transform.position, distance);
				}
				else
				{
					isTriggered = false;
				}
			}
		}

		void OnDisable() 
		{
			if(parent != null)
			{
				isTriggered = false;
				collider.enabled = true;
				parent.localPosition = parentCoord;
			}
		}
	}
}
