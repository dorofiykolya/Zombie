using Runner;
using UnityEngine;
using System.Collections;

namespace Runners
{
    public class PlayerController : ComponentManager
	{
		private Transform currentTransform;
		
		public int ID;
		
        public override void Initialize()
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