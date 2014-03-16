using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LocationObject))]
[CanEditMultipleObjects]
public class LocationObjectEditor : Editor
{
	protected LocationManager location;

    private bool sFoldoutLocationObject = true;

	public override void OnInspectorGUI()
	{
		location = GameObject.FindObjectOfType<Runner.LocationManager>();
		if (location == null)
		{
			GUI.color = Color.red;
			GUILayout.Label("ERROR, LocationManager not found!!!");
			GUI.color = Color.white;
			return;
		}
        GUI.color = ColorEditor.Title;
        sFoldoutLocationObject = EditorGUILayout.Foldout(sFoldoutLocationObject, "LocationObject Settings");
        GUI.color = Color.white;
        if (sFoldoutLocationObject == false) return;
        Draw();
	}

    private void Draw()
    {
        var locationObject = target as LocationObject;
        if (locationObject != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical();

            GUI.color = Color.green;
            locationObject.shaderCulling = EditorGUILayout.Toggle("Shader Culling", locationObject.shaderCulling);
            GUI.color = Color.white;
            EditorGUILayout.Space();

            var shaders = locationObject.shaders;
            if (shaders == null || shaders.Length == 0)
            {
                GUI.color = Color.yellow;
                GUILayout.Box("empty texture, use default");
                GUI.color = Color.white;
            }
            else
            {
                var list = shaders.ToList();
                list.Sort((s1, s2) => {
                    if (s1.Distance > s2.Distance) return -1;
                    if (s1.Distance < s2.Distance) return 1;
                    return 0;
                });
                foreach (var shaderInfo in list)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();

                    var result = list.Where(s => s.Distance == shaderInfo.Distance && s != shaderInfo);
                    if (result.Any())
                    {
                        GUI.color = Color.red;
                    }
                    else
                    {
                        GUI.color = Color.green;
                    }
                    if (shaderInfo.Shader != null)
                    {
                        GUILayout.Box(shaderInfo.Shader.name, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        GUILayout.Box("null", GUILayout.ExpandWidth(true));
                    }

                    
                    if (result.Any())
                    {
                        GUI.color = Color.red;
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }
                    //shaderInfo.Shader = (Shader)EditorGUILayout.ObjectField("Shader To Distance", shaderInfo.Shader, typeof(Shader));
                    shaderInfo.Distance = EditorGUILayout.FloatField("Distance To Camera", shaderInfo.Distance);
                    EditorGUILayout.EndVertical();
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.ExpandHeight(true), GUILayout.Width(20.0f)) || shaderInfo.Shader == null)
                    {
                        list.Remove(shaderInfo);
                        break;
                    }
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                locationObject.shaders = list.ToArray();
            }
            GUI.color = Color.yellow;
            var shader = EditorGUILayout.ObjectField("DragToAdd", null, typeof(Shader));
            if (shader != null && shader is Shader)
            {
                var list = shaders != null ? shaders.ToList() : new List<ShaderInfo>();
                var lastDist = 0.0f;
                if (list.Where(s => s.Shader == shader).Any())
                {
                    EditorUtility.DisplayDialog("Error", "Shader is already exist, " + shader.name, "close");
                }
                else
                {
                    list.Add(new ShaderInfo((Shader)shader, lastDist));
                    locationObject.shaders = list.ToArray();
                }
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }

	protected void DrawBoolLabel(string label, bool value)
	{
		GUILayout.BeginHorizontal();
		if (value)
		{
			GUI.color = Color.grey;
			GUILayout.Label(label, GUILayout.Width(150.0f));
			GUI.color = Color.cyan;
			GUILayout.Label("True");
			GUI.color = Color.white;
		}
		else
		{
			GUI.color = Color.grey;
			GUILayout.Label(label, GUILayout.Width(150.0f));
			GUILayout.Label("False");
			GUI.color = Color.white;
		}
		GUILayout.EndHorizontal();
	}
}


