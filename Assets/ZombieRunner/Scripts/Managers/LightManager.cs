using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour {

	private float time = 0.0f;

	void Start () {

	}

	void Update () 
	{
		time += Time.deltaTime / 2.0f;

		light.intensity = 1.3f + Mathf.Sin (time) / 2.0f;

		var r = transform.eulerAngles;
		r.x = 50.0f;
		r.z = 50.0f;
		r.y += Time.deltaTime * 5;
		transform.eulerAngles = r;
	}
}
