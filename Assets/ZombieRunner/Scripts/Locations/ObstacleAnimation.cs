using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleAnimation : ComponentManager
	{
		private const string RUN1 = "Run1";
		private const string RUN2 = "Run2";
		private const string IDLE = "Idle";
		private const string DEATH = "Death";
		
		public float animationOffset = 0.0f;
		
        public override void Initialize ()
        {
			animation[RUN1].wrapMode = WrapMode.Loop;
			animation[RUN2].wrapMode = WrapMode.Loop;
			animation[IDLE].wrapMode = WrapMode.Loop;
			animation[DEATH].wrapMode = WrapMode.ClampForever;

            idle();
        }

		void OnEnable()
		{
			idle ();
		}

		public void death()
		{
			animation.Play (DEATH);
		}
		
		public void run1()
		{
			animation.CrossFade(RUN1, 0.1f);
		}
		
		public void run2()
		{
			animation.CrossFade(RUN2, 0.1f);
		}
		
		public void idle()
		{
			animation.Play(IDLE);
		}
	}
}
