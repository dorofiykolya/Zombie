using UnityEngine;
using System.Collections;

namespace Runner
{
	[ExecuteInEditMode]
	public class CameraManager : MonoBehaviour {
	
		public static Transform CameraTransform {get; private set;}
		
		void Awake ()
		{
			CameraTransform = transform;
		}
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
