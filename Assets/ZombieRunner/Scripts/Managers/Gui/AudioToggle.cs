using UnityEngine;
using System.Collections;

public class AudioToggle : MonoBehaviour 
{
	public AudioSource audioSource;

	public void ToogleSound()
	{
		audioSource.enabled = GetComponent<UIToggle> ().value;
	}
}
