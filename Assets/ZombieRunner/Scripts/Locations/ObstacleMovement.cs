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
			}
			if(parent.CompareTag("Human"))
			{
				parent.GetComponent<ObstacleAnimation> ().run2();
			}
		}

		void Update()
		{
			if(isTriggered)
			{
				if (parent.localPosition.z <= parentCoord.z + transform.localPosition.z)
				{
					float distance = speed * (Player.Speed / Player.MinimumSpeed) * Time.deltaTime;
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
				parent.localPosition = parentCoord;
			}
		}
	}
}
