using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class ShaderQualityWindowEditor : EditorWindow
{
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        var objects = Selection.GetFiltered(typeof(Runner.LocationObject), SelectionMode.Assets).Cast<Runner.LocationObject>();
        GUI.color = Color.green;
        foreach(var o in objects)
        {
            GUILayout.Label(o.name);
        }
        GUI.color = Color.white;
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("CLEAR"))
        {
            ClearShaders(objects);
        }
        GUILayout.EndHorizontal();
        ShowShaders(objects);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void ShowShaders(IEnumerable<Runner.LocationObject> objects)
    {
        if (objects.Count() == 0) return;
        

    }

    private void ClearShaders(IEnumerable<Runner.LocationObject> objects)
    {
        foreach (var l in objects)
        {
            l.shaderList = new Shader[0];
            l.shaderDistances = new float[0];
            l.shaderQualities = new int[0];
        }
    }

    [MenuItem("Runner/Shader Quality Settings")]
    public static void ShowShaderQualityWindowEditor()
    {
        EditorWindow.GetWindow<ShaderQualityWindowEditor>("Shader Quality Settings", true).Show();
    }
}

