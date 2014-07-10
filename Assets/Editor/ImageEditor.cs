using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Image))]
[CanEditMultipleObjects]
public class ImageEditor : DisplayObjectEditor
{

	void OnEnable()
	{
		
	}
	
	public override void OnInspectorGUI ()
	{
		Title("Texture");

		Info("Name", ((Image)target).textureName);

		Info("X", ((Image)target).textureOffset.x);
		Info("Y", ((Image)target).textureOffset.y);
		Info("Width", ((Image)target).width);
		Info("Height", ((Image)target).height);

		Separate();
		base.OnInspectorGUI();
	}
}

