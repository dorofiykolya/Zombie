using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class StageCamera : MonoBehaviour 
{
	public bool autoSize = false;
	public bool disabled = false;
	public float width = 320.0f;
	public float height = 480.0f;
	public Stage stage;
	private Transform mTransform;

	// Use this for initialization
	void Start () 
	{
		mTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(disabled) return;
		if(stage != null)
		{
			if(stage.Parent != this)
			{
				stage.ResetTransform();
				stage.transform.parent = this.transform;
			}
			if(autoSize)
			{
				width = camera.pixelWidth;
				height = camera.pixelHeight;
			}
			stage.StageWidth = width;
			stage.StageHeight = height;
			camera.orthographicSize = height / 2.0f;
			stage.transform.position = new Vector3(-width / 2.0f, height / 2.0f, 1);
		}
	}

	void LateUpdate()
	{

	}
}
