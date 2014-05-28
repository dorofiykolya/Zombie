using UnityEngine;
using System.Collections;

namespace Runner
{
    public class ObstacleAnimation : ComponentManager
	{
		private const string RUN = "Run";
		private const string IDLE = "Idle";
		private const string DEATH = "Death";
		
		public float animationOffset = 0.0f;
		
        public override void Initialize ()
        {
			animation[RUN].wrapMode = WrapMode.Loop;
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
		
		public void run()
		{
			animation.CrossFade(RUN, 0.1f);
		}
		
		public void idle()
		{
			animation.Play(IDLE);
		}
	}
}
