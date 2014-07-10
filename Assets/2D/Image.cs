using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Image : DisplayObject 
{
	private SpriteRenderer spriteRender;
	private UnityEngine.Sprite sprite;
	private int sortinOrder;

	public float width;
	public float height;
	public string textureName;
	public Vector2 textureOffset;

	void Start () 
	{
		UpdateProperties();
	}

	public override void UpdateProperties()
	{
		base.UpdateProperties();
		spriteRender = (SpriteRenderer)GetComponent<SpriteRenderer>();
		sprite = spriteRender.sprite;
		var rect = sprite.rect;
		width = rect.width;
		height = rect.height;
		textureName = sprite.name;
		textureOffset = sprite.textureRectOffset;
	}

	public int SortingOrder
	{
		get{return sortinOrder;}
		set
		{
			sortinOrder = value;
			if(spriteRender != null)
			{
				spriteRender.sortingOrder = sortinOrder;
			}
		}
	}

	public override Rect Bounds {
		get 
		{
			return new Rect(x, y, width * scaleX, height * scaleY);
		}
	}

	public override float Width
	{
		get{return width * scaleX;}
		set
		{
			var newScale = value / width;
			if(newScale != scaleX)
			{
				scaleX = newScale;
				mTransform.localScale = new Vector3(scaleX, scaleY, 1.0f);
				UpdateSize();
			}
		}
	}

	public override float Height
	{
		get{return height * scaleY;}
		set
		{
			var newScale = value / height;
			if(newScale != scaleY)
			{
				scaleY = newScale;
				mTransform.localScale = new Vector3(scaleX, scaleY, 1.0f);
				UpdateSize();
			}
		}
	}
}
