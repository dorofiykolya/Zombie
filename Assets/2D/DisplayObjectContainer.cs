using UnityEngine;
using System.Collections;

public class DisplayObjectContainer : DisplayObject 
{
	protected Vector3 SCALE_ZERO = new Vector3(0,0,0);

	protected override void UpdateVisible ()
	{
		base.UpdateVisible ();
		UpdateScale();
	}

	public override Rect Bounds {
		get 
		{
			if(mTransform == null) initialize();
			var rect = new Rect(x,y,0,0);
			var count = mTransform.childCount;
			var i = 0;
			for(i = 0; i <count; i++)
			{
				var child = mTransform.GetChild(i).GetComponent<DisplayObject>();
				if(child != null)
				{
					var bounds = child.Bounds;
					rect = union(rect, bounds);
				}
			}
			rect.width *= scaleX;
			rect.height *= scaleY;
			return rect;
		}
	}

	public override float Width {
		get {
			return Bounds.width;
		}
		set {
			scaleX = 1;
			var width = Bounds.width;
			if(width != 0.0f)
			{
				scaleX = value / width;
				mTransform.localScale = new Vector3(scaleX, scaleY, 1.0f);
				UpdateSize();
			}
		}
	}

	public override float Height {
		get {
			return Bounds.height;
		}
		set {
			scaleY = 1;
			var height = Bounds.height;
			if(height != 0.0f)
			{
				scaleY = value / height;
				mTransform.localScale = new Vector3(scaleX, scaleY, 1.0f);
				UpdateSize();
			}
		}
	}

	private Rect union(Rect r, Rect r2) 
	{
		var x1 = Mathf.Min(r2.x, r.x);
		var x2 = Mathf.Max(r2.x + r2.width, r.x + r.width);
		var y1 = Mathf.Min(r2.y, r.y);
		var y2 = Mathf.Max(r2.y + r2.height, r.y + r.height);
		return new Rect (x1, y1, x2 - x1, y2 - y1);
	}

	protected int SortingOrder(int parentIndex)
	{
		if(mTransform == null) initialize();

		var numChildren = mTransform.childCount;
		for(var i = 0; i < numChildren; i++)
		{
			var child = mTransform.GetChild(i);
			var obj = child.GetComponent<DisplayObject>();
			if(obj is DisplayObjectContainer)
			{
				parentIndex = ((DisplayObjectContainer)(obj)).SortingOrder(parentIndex);
			}
			else if(obj is Image)
			{
				((Image)(obj)).SortingOrder = parentIndex++;
			}
		}
		return parentIndex;
	}

	protected override void UpdateAlpha ()
	{
		var list = GetComponentsInChildren<DisplayObject>();
		foreach(var l in list)
		{
			if(l == this) continue;
			l.SetAlpha(alpha * RootAlpha);
		}
	}

	protected float RootAlpha
	{
		get
		{
			float a = 1.0f;
			var target = Parent;
			if(target != null)
			{
				if(target is Stage) return a;
				a *= target.Alpha;
				target = target.Parent;
			}
			return a;
		}
	}

	internal override void SetAlpha (float parentAlpha)
	{
		var list = GetComponentsInChildren<DisplayObject>();
		foreach(var l in list)
		{
			if(l == this) continue;
			l.SetAlpha(alpha * parentAlpha);
		}
	}

	protected override void UpdateScale ()
	{
		if(visiable)
		{
			base.UpdateScale ();
		}
		else
		{
			if(mTransform == null) initialize();
			mTransform.localScale = SCALE_ZERO;
		}
	}

}
