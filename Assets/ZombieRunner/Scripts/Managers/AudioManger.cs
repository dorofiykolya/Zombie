using UnityEngine;
using System.Collections;
namespace Runner
{
	public class AudioManger : ComponentManager
	{
		public AudioClip[] audio;

		public AudioSource audioSource;
		public AudioSource music;

		public void PlaySound(int id)
		{
			audioSource.PlayOneShot (audio [id]);
		}

		public override void Initialize ()
		{
			music.loop = true;
			music.clip = audio [0];
			music.Play ();
		}
	}
}
