using UnityEngine;
using System.Collections;

namespace Runners
{
	public class PlayerController : MonoBehaviour
	{
		private Transform currentTransform;
		
		public int ID;
		
		void Awake()
		{
			currentTransform = gameObject.transform;	
		}
		// Use this for initialization
		void Start ()
		{
			//animation["Run"].wrapMode = WrapMode.Loop;
			//animation.Play("Run");
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
		
		public void Move (float distance)
		{
			//animation["Run"].speed = Runner.PlayerManager.Speed/10;
		}
	}

}