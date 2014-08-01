using UnityEngine;
using System.Collections;
namespace Runner
{
	public class AudioManger : ComponentManager
	{
		public AudioClip[] audioclip;

		public AudioSource audioSource;
		public AudioSource music;

		public void PlaySound(int id)
		{
			audioSource.PlayOneShot (audioclip [id]);
		}

		public override void Initialize ()
		{
			music.loop = true;
			music.clip = audioclip [0];
			music.Play ();
		}
	}
}
