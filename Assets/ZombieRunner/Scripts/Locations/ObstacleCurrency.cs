using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleCurrency : ComponentManager
    {
		private float progress = 0;
		private bool isTween = false;
		private Vector3 player;
		private Vector3 probe;

		public bool isPickUp = false;

		void Update () 
		{
			transform.Rotate(Vector3.up * Time.deltaTime * 100);

			if(isPickUp)
			{
				player = transform.InverseTransformPoint(WaypointManager.wayPoints[WaypointManager.currentWP].position);
				probe = transform.position;

				player.z = player.x * -1;
				player.y = transform.position.y / 2;
				player.x = WaypointManager.wayPoints[WaypointManager.currentWP].position.x;

				isPickUp = false;
				isTween = true;
			}

			if(isTween)
			{

				transform.position = Vector3.MoveTowards(transform.position, 
				                                         player, 
				                                         Time.deltaTime * Player.Speed * 5);
				transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);

				progress += Time.deltaTime * Player.Speed / 10;

				if(progress > 1)
				{
					isTween = false;
					transform.localScale = Vector3.zero;
				}
			}
		}

		void OnDisable() 
		{
			transform.localScale = Vector3.one;
			if(probe != Vector3.zero) transform.position = probe;
			gameObject.collider.enabled = true;
			isPickUp = false;
			isTween = false;
			progress = 0;
		}
    }
}
