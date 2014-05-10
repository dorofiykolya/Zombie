using UnityEngine;
using System.Collections;

namespace Runner
{
    public class AnimationManager : ComponentManager
    {
        private Animation thisAnimation;

		private const string RUN = "Run";
		private const string JUMP = "Jump";
		private const string SLIDE = "Slide";
		private const string SLIDE_RIGHT = "SlideRight";
		private const string SLIDE_LEFT = "SlideLeft";
		private const string EAT = "Eat";
		private const string DEATH = "Death";
		private const string IDLE = "Idle";

		public float animationOffset = 0.0f;

        public override void Initialize()
        {
            thisAnimation = animation;
            thisAnimation[RUN].wrapMode = WrapMode.Loop;
			thisAnimation[IDLE].wrapMode = WrapMode.Loop;
            thisAnimation[JUMP].wrapMode = WrapMode.ClampForever;
            thisAnimation[SLIDE_RIGHT].wrapMode = WrapMode.ClampForever;
            thisAnimation[SLIDE_LEFT].wrapMode = WrapMode.ClampForever;
            thisAnimation[EAT].wrapMode = WrapMode.ClampForever;
            thisAnimation[DEATH].wrapMode = WrapMode.ClampForever;
            thisAnimation[SLIDE].wrapMode = WrapMode.Loop;

			if(Player.isStop)
				idle();
			else
				run();
        }

		public void updateSpeed()
		{
            var speed = Player.Speed / Player.MinimumSpeed + animationOffset;
			thisAnimation[RUN].speed = speed; 
			thisAnimation[JUMP].speed = speed;
			thisAnimation[SLIDE].speed = speed;
			thisAnimation[SLIDE_RIGHT].speed = speed;
			thisAnimation[SLIDE_LEFT].speed = speed;
			thisAnimation[EAT].speed = speed;
		}

        public void idle()
        {
			thisAnimation.CrossFade(IDLE, 0.1f);
        }

		public void run()
		{
			if(!Player.isStop)
				thisAnimation.CrossFade(RUN, 0.1f);
		}

        public void jump()
        {
			thisAnimation.Rewind (JUMP);
			thisAnimation.CrossFade(JUMP, 0.1f);
        }

        public void slide()
        {
			thisAnimation.Rewind (SLIDE);
			thisAnimation.CrossFade(SLIDE, 0.1f);
        }

		public IEnumerator slideRight()
		{
			thisAnimation.PlayQueued(SLIDE_RIGHT, QueueMode.PlayNow);
			yield return new WaitForSeconds( thisAnimation[SLIDE_RIGHT].length );
			run();
		}

		public IEnumerator slideLeft()
		{
			thisAnimation.PlayQueued(SLIDE_LEFT, QueueMode.PlayNow);
			yield return new WaitForSeconds( thisAnimation[SLIDE_LEFT].length );
			run();
		}

		public IEnumerator eat()
		{
			thisAnimation.PlayQueued(EAT, QueueMode.PlayNow);
			yield return new WaitForSeconds( thisAnimation[EAT].length );
			run();
		}

		public void death()
		{
			thisAnimation.Play(DEATH);
		}
    }
}
