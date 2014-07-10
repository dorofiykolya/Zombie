using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Stage))]
[CanEditMultipleObjects]
public class StageEditor : SpriteEditor
{
	void OnEnable()
	{
		
	}
	
	public override void OnInspectorGUI ()
	{
		Title("Stage");
		Info("Stage Width", ((Stage)target).StageWidth);
		Info("Stage Height", ((Stage)target).StageHeight);
		Info("Num Children", TargetTransform.childCount);
		if(Button("SortOrder"))
		{
			((Stage)target).SortingImageOrder(); 
		}
		Separate();
		Title("Gizmos (Demo)");
		((Stage)target).gizmosDrawSize = ToggleField("Draw Size", ((Stage)target).gizmosDrawSize);
		((Stage)target).gizmosDrawBounds = ToggleField("Draw Bounds", ((Stage)target).gizmosDrawBounds);
		Separate();
	}
}

