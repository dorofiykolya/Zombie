using UnityEngine;
using System.Collections;
using Runner;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Runner.ObstacleObject))]
[CanEditMultipleObjects]
public class ObstacleObjectEditor : LocationObjectEditor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}
