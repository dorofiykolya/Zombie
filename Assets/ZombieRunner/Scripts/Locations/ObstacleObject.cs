using UnityEngine;
using System.Collections;

namespace Runner
{
	[AddComponentMenu("Runner/Obstacle")]
    public class ObstacleObject : Obstacle
    {
		void OnEnable() 
		{
			transform.localScale = Vector3.one;
			gameObject.collider.enabled = true;
		}
    }
}
