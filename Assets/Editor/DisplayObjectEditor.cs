using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

public enum VAlign
{
	TOP,
	MIDDLE,
	BOTTOM
}
public enum HAlign
{
	LEFT,
	CENTER,
	RIGHT
}

[CustomEditor(typeof(DisplayObject))]
[CanEditMultipleObjects]
public class DisplayObjectEditor : UnityEditor.Editor
{

	void OnEnable()
	{
		
	}
	
	public override void OnInspectorGUI ()
	{
		Title("Properties");

		var targetWidth = Target.Width;
		var targetHeight = Target.Height;
		var targetScaleX = Target.ScaleX;
		var targetScaleY = Target.ScaleY;

		var x = FloatField("X", Target.X);
		var y = FloatField("Y", Target.Y);
		var width = FloatField("Width", targetWidth);
		var height = FloatField("Height", targetHeight);
		var pivotX = FloatField("PivotX", Target.PivotX);
		var pivotY = FloatField("PivotY", Target.PivotY);
		var scaleX = FloatField("ScaleX", targetScaleX);
		var scaleY = FloatField("ScaleY", targetScaleY);
		var rotation = FloatField("Rotation", Target.Rotation);
		var alpha = Slider("Alpha", Target.Alpha, 0.0f, 1.0f);
		var visiable = ToggleField("Visible", Target.Visible);

		var scaleXChanged = scaleX != targetScaleX;
		var scaleYChanged = scaleY != targetScaleY;
		var widthChanged = width != targetWidth;
		var heightChanged = height != targetHeight;

		Separate();
		Title("Align Pivot");
		GUI.color = new Color((float)100/255,(float)180/255,(float)255/255);
		EditorGUILayout.BeginHorizontal();
		if(Button("*", 20)) AlignPivot(VAlign.TOP, HAlign.LEFT, ref pivotX, ref pivotY);
		if(Button("*", 20)) AlignPivot(VAlign.TOP, HAlign.CENTER, ref pivotX, ref pivotY);
		if(Button("*", 20)) AlignPivot(VAlign.TOP, HAlign.RIGHT, ref pivotX, ref pivotY);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if(Button("*", 20)) AlignPivot(VAlign.MIDDLE, HAlign.LEFT, ref pivotX, ref pivotY);
		if(Button("*", 20)) AlignPivot(VAlign.MIDDLE, HAlign.CENTER, ref pivotX, ref pivotY);
		if(Button("*", 20)) AlignPivot(VAlign.MIDDLE, HAlign.RIGHT, ref pivotX, ref pivotY);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if(Button("*", 20)) AlignPivot(VAlign.BOTTOM, HAlign.LEFT, ref pivotX, ref pivotY);
		if(Button("*", 20)) AlignPivot(VAlign.BOTTOM, HAlign.CENTER, ref pivotX, ref pivotY);
		if(Button("*", 20)) AlignPivot(VAlign.BOTTOM, HAlign.RIGHT, ref pivotX, ref pivotY);
		EditorGUILayout.EndHorizontal();
		GUI.color = Color.white;
		Separate();

		Target.X = x;
		Target.Y = y;
		if(widthChanged)
		{
			Target.Width = width;
		}
		if(heightChanged)
		{
			Target.Height = height;
		}
		Target.PivotX = pivotX;
		Target.PivotY = pivotY;
		if(scaleXChanged)
		{
			Target.ScaleX = scaleX;
		}
		if(scaleYChanged)
		{
			Target.ScaleY = scaleY;
		}
		Target.Rotation = rotation;
		Target.Visible = visiable;
		Target.Alpha = alpha;


		GUI.color = new Color((float)100/255,(float)180/255,(float)255/255);
		if(Button("UpdateProperties"))
		{
			UpdateProperties();
		}
		if(Button("ResetTransform"))
		{
			ResetTransform();
		}
		GUI.color = Color.white;
	}

	protected void AlignPivot(VAlign v, HAlign h, ref float pivotX, ref float pivotY)
	{
		switch(v)
		{
		case VAlign.TOP:
			pivotY = 0;
			break;
		case VAlign.MIDDLE:
			pivotY = Target.Height / 2;
			break;
		case VAlign.BOTTOM:
			pivotY = Target.Height;
			break;
		}

		switch(h)
		{
		case HAlign.LEFT:
			pivotX = 0;
			break;
		case HAlign.CENTER:
			pivotX = Target.Width / 2;
			break;
		case HAlign.RIGHT:
			pivotX = Target.Width;
			break;
		}
	}

	protected bool IsNotEquals(object value, object target)
	{
		return value != target;
	}

	protected DisplayObject Target
	{
		get{ return (DisplayObject)target;}
	}

	protected Transform TargetTransform
	{
		get{ return ((DisplayObject)target).transform;}
	}

	protected void ResetTransform()
	{
		var t = target as DisplayObject;
		if(t != null)
		{
			t.ResetTransform();
		}
	}

	protected void UpdateProperties()
	{
		var t = target as DisplayObject;
		if(t != null)
		{
			t.UpdateProperties();
		}
	}

	protected void Title(string title)
	{
		GUI.color = new Color((float)100/255,(float)180/255,(float)255/255);
		EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
		GUI.color = Color.white;
	}

	protected bool ToggleField(string text, bool value)
	{
		return EditorGUILayout.Toggle(text, value);
	}

	protected float Slider(string text, float value, float min, float max)
	{
		return EditorGUILayout.Slider(text, value, min, max, GUILayout.ExpandWidth(false));
	}

	protected float FloatField(string text, float value)
	{
		return EditorGUILayout.FloatField(text, value);
	}

	protected void Label(string text, bool bold = false)
	{
		if(bold)
		{
			EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
		}
		else
		{
			EditorGUILayout.LabelField(text);
		}
	}
	
	protected void Separate()
	{
		EditorGUILayout.Separator();	
	}
	
	protected bool Button(string label, float width = float.NaN)
	{
		return GUILayout.Button(label, GUILayout.Width(width));
	}
	
	protected void Info(string label, object value)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label);
		EditorGUILayout.LabelField(value.ToString());
		EditorGUILayout.EndHorizontal();
	}
}



