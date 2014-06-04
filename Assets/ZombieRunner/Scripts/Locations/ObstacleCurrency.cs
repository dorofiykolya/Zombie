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

		public override void Initialize ()
		{
			probe = transform.localPosition;
			if(QualityManager.CurrentQuality == 2)
			{
				gameObject.transform.FindChild("probirka").gameObject.SetActive(false);
			}
			else
			{
				gameObject.transform.FindChild("Coin").gameObject.SetActive(false);
			}
		}

		void Update () 
		{
			transform.Rotate(Vector3.up * Time.deltaTime * 100);

			if(isPickUp)
			{
				player = transform.InverseTransformPoint(Waypoint.wayPoints[Waypoint.currentWP].position);

				player.z = player.x * -1;
				player.y = transform.position.y / 2;
				player.x = Waypoint.wayPoints[Waypoint.currentWP].position.x;

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

					transform.localPosition = probe;
				}
			}
		}

		void OnEnbale() 
		{
			transform.localScale = Vector3.one;
			gameObject.collider.enabled = true;
			isPickUp = false;
			isTween = false;
			progress = 0;
		}
    }
}
