using UnityEngine;
using System.Collections;

namespace Runner
{
    public class AnimationManager : MonoBehaviour
    {
        private Animation thisAnimation;

		private const string RUN = "Run";
		private const string JUMP = "Jump";
		private const string SLIDE = "Slide";
		private const string SLIDE_RIGHT = "SlideRight";
		private const string SLIDE_LEFT = "SlideLeft";
		private const string EAT = "Eat";
		private const string DEATH = "Death";

		public float animationOffset = 0.0f;

        // Use this for initialization
        void Awake()
        {
            thisAnimation = animation;

			thisAnimation[RUN].wrapMode = WrapMode.Loop;
			thisAnimation[JUMP].wrapMode = WrapMode.ClampForever;
			thisAnimation[SLIDE_RIGHT].wrapMode = WrapMode.ClampForever;
			thisAnimation[SLIDE_LEFT].wrapMode = WrapMode.ClampForever;
			thisAnimation[EAT].wrapMode = WrapMode.ClampForever;
			thisAnimation[DEATH].wrapMode = WrapMode.ClampForever;
			thisAnimation[SLIDE].wrapMode = WrapMode.Loop;

            run();
        }

		public void updateSpeed()
		{
			var speed = PlayerManager.Speed / PlayerManager.MinimumSpeed + animationOffset;
			thisAnimation[RUN].speed = speed; 
			thisAnimation[JUMP].speed = speed;
			thisAnimation[SLIDE].speed = speed;
			thisAnimation[SLIDE_RIGHT].speed = speed;
			thisAnimation[SLIDE_LEFT].speed = speed;
			thisAnimation[EAT].speed = speed;
		}

        public void run()
        {
			thisAnimation.CrossFade(RUN, 0.2f, PlayMode.StopAll);
        }

        public void jump()
        {
			thisAnimation.CrossFade(JUMP, 0.2f);
        }

        public void slide()
        {
			thisAnimation.CrossFade(SLIDE, 0.2f);
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

		public void eat()
		{
			thisAnimation.CrossFade(EAT, 0.2f);
		}

		public void death()
		{
			thisAnimation.Play(DEATH);
		}
    }
}
