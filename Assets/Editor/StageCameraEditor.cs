using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(StageCamera))]
[CanEditMultipleObjects]
public class StageCameraEditor : DisplayObjectEditor
{
	public override void OnInspectorGUI ()
	{
		Title("Camera");

		var cam = (StageCamera)target;

		var disabled = ToggleField("Disabled", cam.disabled);
		var auto = ToggleField("Auto Size", cam.autoSize);
		var width = FloatField("Width", cam.width);
		var height = FloatField("Height", cam.height);

		cam.disabled = disabled;
		cam.autoSize = auto;
		cam.width = width;
		cam.height = height;
	}
}

