using UnityEngine;
using System.Collections;

public class Sprite : DisplayObjectContainer {

	public bool gizmosDrawBounds;

	/*public bool useClipRect = true;
	public Rect clipRect = new Rect(0.0f, 0.0f, 100.0f, 100.0f);
*/
	void Update()
	{

	}

	protected virtual void OnDrawGizmos() 
	{
		if(gizmosDrawBounds)
		{
			Gizmos.color = Color.cyan;
			var bounds = RootBounds;
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
