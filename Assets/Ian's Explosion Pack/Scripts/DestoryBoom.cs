using UnityEngine;
using System.Collections;

public class DestoryBoom : MonoBehaviour 
{
	public float lifeTime;
	private float destroyTime;

	// Use this for initialization
	void Start () 
	{
		destroyTime = Time.timeSinceLevelLoad + lifeTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.timeSinceLevelLoad > destroyTime)
		{
			Destroy(this.gameObject);
		}
	}
}
