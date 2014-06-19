using UnityEngine;
using System.Collections;

namespace Runner
{
    public class AnimationManager : ComponentManager
    {
		private const string RUN = "Run";
		private const string JUMP = "Jump";
		private const string SLIDE = "Slide";
		private const string SLIDE_RIGHT = "SlideRight";
		private const string SLIDE_LEFT = "SlideLeft";
		private const string EAT = "Eat";
		private const string DEATH = "Death";
		private const string IDLE = "Idle";

		public float animationOffset = 0.0f;

		public string current;

        public override void Initialize()
        {
			animation[RUN].wrapMode = WrapMode.Loop;
			animation[IDLE].wrapMode = WrapMode.Loop;
			animation[JUMP].wrapMode = WrapMode.ClampForever;
			animation[SLIDE_RIGHT].wrapMode = WrapMode.ClampForever;
			animation[SLIDE_LEFT].wrapMode = WrapMode.ClampForever;
			animation[DEATH].wrapMode = WrapMode.ClampForever;
			animation[SLIDE].wrapMode = WrapMode.Loop;

			if(Player.isStop)
				idle();
			else
				run();
        }

		public void updateSpeed()
		{
            var speed = Player.Speed / Player.MinimumSpeed;
			animation[RUN].speed = speed; 
			animation[JUMP].speed = speed + animationOffset;
			animation[SLIDE].speed = speed;
			animation[SLIDE_RIGHT].speed = speed;
			animation[SLIDE_LEFT].speed = speed;
		}

        public void idle()
        {
			if(current == IDLE) return;
			current = IDLE;
			animation.CrossFade(IDLE, 0.1f);
        }

		public void run()
		{
			if(current == RUN) return;
			current = RUN;
			if(!Player.isStop && this)
				animation.CrossFade(RUN, 0.1f);
		}

        public void jump()
        {
			if(current == JUMP) return;
			current = JUMP;
			animation.Rewind (JUMP);
			animation.CrossFade(JUMP, 0.1f);
        }

        public void slide()
        {
			if(current == SLIDE) return;
			current = SLIDE;
			animation.Rewind (SLIDE);
			animation.CrossFade(SLIDE, 0.1f);
        }

		public IEnumerator slideRight()
		{
			if(current == SLIDE_RIGHT) yield return null;
			current = SLIDE_RIGHT;
			animation.PlayQueued(SLIDE_RIGHT, QueueMode.PlayNow);
			yield return new WaitForSeconds( animation[SLIDE_RIGHT].length );
			run();
		}

		public IEnumerator slideLeft()
		{
			if(current == SLIDE_LEFT) yield return null;
			current = SLIDE_LEFT;
			animation.PlayQueued(SLIDE_LEFT, QueueMode.PlayNow);
			yield return new WaitForSeconds( animation[SLIDE_LEFT].length );
			run();
		}

		public void death()
		{
			if(current == DEATH) return;
			current = DEATH;
			animation.Play(DEATH);
		}
    }
}
