using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleAnimation : ComponentManager
	{
		private Animation thisAnimation;
		
		private const string RUN1 = "Run1";
		private const string RUN2 = "Run2";
		private const string IDLE = "Idle";
		private const string DEATH = "Death";
		
		public float animationOffset = 0.0f;
		
        public override void Initialize()
        {
            thisAnimation = animation;

            thisAnimation[RUN1].wrapMode = WrapMode.Loop;
            thisAnimation[RUN2].wrapMode = WrapMode.Loop;
            thisAnimation[IDLE].wrapMode = WrapMode.Loop;
			thisAnimation[DEATH].wrapMode = WrapMode.ClampForever;

            idle();
        }

		public void death()
		{
			thisAnimation.Play (DEATH);
		}
		
		public void run1()
		{
			thisAnimation.CrossFade(RUN1, 0.1f);
		}
		
		public void run2()
		{
			thisAnimation.CrossFade(RUN2, 0.1f);
		}
		
		public void idle()
		{
			thisAnimation.CrossFade(IDLE, 0.1f);
		}
	}
}
