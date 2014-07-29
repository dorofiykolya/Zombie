using UnityEngine;
using System.Collections;

public class DanceAnimation : MonoBehaviour 
{
	private const string DANCE1 = "Dance1";
	private const string DANCE2 = "Dance2";
	private const string DANCE3 = "Dance3";

	void OnEnable()
	{
		StopAllCoroutines ();
		StartCoroutine (DanceDanceRevolution ());
	}

	private IEnumerator DanceDanceRevolution()
	{
		animation.Rewind (DANCE1);
		animation.CrossFade(DANCE1, 0.1f);
		yield return new WaitForSeconds( animation[DANCE1].length );
		animation.Rewind (DANCE2);
		animation.CrossFade(DANCE2, 0.1f);
		yield return new WaitForSeconds( animation[DANCE2].length );
		animation.Rewind (DANCE3);
		animation.CrossFade(DANCE3, 0.1f);
		yield return new WaitForSeconds( animation[DANCE3].length );
		StartCoroutine (DanceDanceRevolution ());
	}
}
