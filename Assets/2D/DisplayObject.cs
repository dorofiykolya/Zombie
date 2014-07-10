using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DisplayObject : MonoBehaviour 
{
	protected Transform mTransform;
	protected Renderer mRender;
	protected float x;
	protected float y;
	protected float rotation;
	protected float scaleX = 1.0f;
	protected float scaleY = 1.0f;
	protected float pivotX = 0.0f;
	protected float pivotY = 0.0f;
	protected float alpha = 1.0f;
	protected bool visiable = true;
	protected bool invalidated;

	public DisplayObjectContainer Parent
	{
		get 
		{
			if(mTransform == null) initialize();
			if(mTransform.parent != null)
			{
				return mTransform.parent.GetComponent<DisplayObjectContainer>() as DisplayObjectContainer;
			}
			return null;
		}
	}

	public virtual void ResetTransform()
	{
		if(mTransform == null) initialize();
		x = y = rotation = pivotX = pivotY = 0.0f;
		scaleX = scaleY = alpha = 1.0f;
		UpdateProperties();
	}

	public virtual Rect Bounds
	{
		get {return new Rect(x,y,0,0);}
	}

	public virtual Rect RootBounds
	{
		get
		{
			var rect = Bounds;
			var target = Parent;
			while(target != null)
			{
				rect.x += target.x;
				rect.y += target.y;
				if(target is Sprite)
				{
					rect.x += target.transform.localPosition.x;
					rect.y += target.transform.localPosition.y;
					break;
				}
				target = target.Parent;
			}

			return rect;
		}
	}

	public virtual float Width{get;set;}
	public virtual float Height{get;set;}

	public virtual Stage Stage
	{
		get
		{
			var target = Parent;
			while(target != null)
			{
				if(target is Stage) return (Stage)target;
				target = target.Parent;
			}
			return null;
		}
	}

	public virtual float Alpha
	{
		get
		{ 
			return alpha;
		}
		set
		{
			if(alpha != value)
			{
				alpha = value;
				invalidated = true;
				//UpdateAlpha();
			}
		}
	}

	public virtual float X
	{
		get
		{ 
			return x;
		}
		set
		{
			if(x != value)
			{
				x = value;
				invalidated = true;
				//UpdatePosition();
			}
		}
	}

	public virtual float Y
	{
		get
		{ 
			return y;
		}
		set
		{
			if(y != value)
			{
				y = value;
				invalidated = true;
				//UpdatePosition();
			}
		}
	}

	public virtual float ScaleX
	{
		get
		{ 
			return scaleX;
		}
		set
		{
			if(scaleX != value)
			{
				scaleX = value;
				invalidated = true;
				//UpdateScale();
			}
		}
	}
	
	public virtual float ScaleY
	{
		get
		{ 
			return scaleY;
		}
		set
		{
			if(scaleY != value)
			{
				scaleY = value;
				invalidated = true;
				//UpdateScale();
			}
		}
	}

	public virtual float PivotX
	{
		get
		{ 
			return pivotX;
		}
		set
		{
			if(pivotX != value)
			{
				pivotX = value;
				invalidated = true;
				//UpdateProperties();
			}
		}
	}
	
	public virtual float PivotY
	{
		get
		{ 
			return pivotY;
		}
		set
		{
			if(pivotY != value)
			{
				pivotY = value;
				invalidated = true;
				//UpdateProperties();
			}
		}
	}

	public virtual float Rotation
	{
		get{
			return rotation;
		}
		set
		{
			if(rotation != value)
			{
				rotation = value;
				invalidated = true;
				//UpdateProperties();
			}
		}
	}

	public virtual void UpdateProperties()
	{
		UpdateSize();
		UpdateScale();
		UpdatePosition();
		UpdateRotation();
		UpdatePivot();
		UpdateVisible();
		UpdateAlpha();
	}

	protected virtual void UpdateAlpha()
	{
		if(mTransform == null) initialize();
		if(mRender != null)
		{
			var sr = mRender as SpriteRenderer;
			if(sr != null)
			{
				float a = alpha;
				var target = Parent;
				while(target != null)
				{
					a *= target.Alpha;
					target = target.Parent;
				}
				
				var c = sr.color;
				c.a = a;
				sr.color = c;
			}
		}
	}

	internal virtual void SetAlpha(float parentAlpha = 1.0f)
	{
		if(mTransform == null) initialize();
		if(mRender != null)
		{
			var sr = mRender as SpriteRenderer;
			if(sr != null)
			{
				var c = sr.color;
				c.a = alpha * parentAlpha;
				sr.color = c;
			}
		}
	}

	protected virtual void UpdatePivot()
	{
		if(mTransform == null) initialize();
		mTransform.Translate(new Vector3(-pivotX, pivotY, 0));
	}

	protected virtual void UpdateVisible()
	{

	}

	protected virtual void UpdateSize()
	{

	}

	protected virtual void UpdatePosition()
	{
		if(mTransform == null) initialize();
		mTransform.localPosition = new Vector3(x, -y, 0);
	}

	protected virtual void UpdateScale()
	{
		if(mTransform == null) initialize();
		mTransform.localScale = new Vector3(scaleX, scaleY, 1);
	}

	protected virtual void UpdateRotation()
	{
		if(mTransform == null) initialize();
		mTransform.localRotation = Quaternion.Euler(new Vector3(0,0,rotation));
	}

	public virtual bool Visible
	{
		get{return visiable;}
		set
		{
			if(mTransform == null) initialize();
			visiable = value;
			if(mRender != null)
			{
				mRender.enabled = value;
			}
			UpdateVisible();
		}
	}

	public void Validate()
	{
		UpdateProperties();
		invalidated = false;
	}

	public void Invalidate()
	{
		invalidated = true;
	}

	protected void initialize()
	{
		mTransform = this.transform;
		mRender = this.renderer;
	}

	void Start () 
	{
		initialize();
	}
	

	void Update () 
	{
		if(invalidated)
		{
			Validate();
		}
	}
}
