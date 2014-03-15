using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Sprite))]
[CanEditMultipleObjects]
public class SpriteEditor : DisplayObjectEditor
{
	public override void OnInspectorGUI ()
	{
		Title("Sprite");
		Info("Num Children", TargetTransform.childCount);

		Separate();

		Title("Gizmos (Demo)");
		((Sprite)target).gizmosDrawBounds = ToggleField("Draw Bounds", ((Sprite)target).gizmosDrawBounds);
		Separate();

		base.OnInspectorGUI();
	}


}

