using UnityEngine;
using System.Collections;
using Runner;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Runner.ObstaclePowerUp))]
[CanEditMultipleObjects]
public class ObstaclePowerUpEditor : LocationObjectEditor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}
