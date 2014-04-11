using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleCurrency : ComponentManager
    {
		void OnDisable() 
		{
			transform.localScale = Vector3.one;
		}
    }
}
