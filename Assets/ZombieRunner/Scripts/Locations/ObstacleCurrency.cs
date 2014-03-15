using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleCurrency : MonoBehaviour
    {
		void OnDisable() 
		{
			transform.localScale = Vector3.one;
		}
    }
}
