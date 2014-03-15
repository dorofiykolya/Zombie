using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Stage : Sprite 
{
	public bool gizmosDrawSize = true;

	private float stageWidth;
	private float stageHeight;

	public float StageWidth
	{
		get{return stageWidth;}
		set{stageWidth = value;}
	}
	public float StageHeight
	{
		get{return stageHeight;}
		set{stageHeight =  value;}
	}

	public override void ResetTransform ()
	{
		if(mTransform == null) initialize();
		X = Y = Rotation = PivotX = PivotY = 0.0f;
		ScaleX = ScaleY = Alpha = 1.0f;
	}

	public void SortingImageOrder()
	{
		base.SortingOrder(0);
	}

	protected override void OnDrawGizmos() 
	{
		if(gizmosDrawSize)
		{
			Gizmos.color = Color.magenta;

			float left = -stageWidth/2.0f;
			float top = stageHeight/2.0f;
			float right = stageWidth/2.0f;
			float bottom = -stageHeight/2.0f;

			//top
			Gizmos.DrawLine(new Vector2(left, top), 
			                new Vector2(right, top));
			//bottom
			Gizmos.DrawLine(new Vector2(left, bottom), 
			                new Vector2(right, bottom));
			//left
			Gizmos.DrawLine(new Vector2(left, top), 
			                new Vector2(left, bottom));
			//right
			Gizmos.DrawLine(new Vector2(right, top), 
			                new Vector2(right, bottom));
		}
		if(gizmosDrawBounds)
		{
			Gizmos.color = Color.cyan;
			var bounds = RootBounds;
			bounds.x -= stageWidth / 2.0f;
			bounds.y += stageHeight / 2.0f;
			Gizmos.DrawLine(new Vector2(bounds.x, bounds.y), 
			                new Vector2(bounds.x + bounds.width, bounds.y));
			
			Gizmos.DrawLine(new Vector2(bounds.x, bounds.y), 
			                new Vector2(bounds.x, bounds.y - bounds.height));
			
			Gizmos.DrawLine(new Vector2(bounds.x + bounds.width, bounds.y), 
			                new Vector2(bounds.x + bounds.width, bounds.y - bounds.height));
			
			Gizmos.DrawLine(new Vector2(bounds.x, bounds.y - bounds.height), 
			                new Vector2(bounds.x + bounds.width, bounds.y - bounds.height));
		}
	}
}
