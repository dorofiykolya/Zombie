using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Runner.QualityManager))]
[CanEditMultipleObjects]
public class QualityManagerEditor : UnityEditor.Editor
{
    private bool sFoldoutLocationObject = false;
    public override void OnInspectorGUI()
    {
        //GUI.color = ColorEditor.Title;
        //sFoldoutLocationObject = EditorGUILayout.Foldout(sFoldoutLocationObject, "Settings");
        //GUI.color = Color.white;
        //if (sFoldoutLocationObject == false) return;
        var names = QualitySettings.names;
        GUILayout.BeginHorizontal();
        GUILayout.Space(10.0f);

        GUI.color = Color.green;
        GUILayout.Box(QualitySettings.names[QualitySettings.GetQualityLevel()]);
        GUI.color = Color.white;
        EditorGUILayout.Separator();

        GUILayout.BeginVertical();
        var len = names.Length;
        for (var i = 0; i < len; i++)
        {
            float red = (float)i / (float)len;
            GUI.color = new Color(red, 1.0f-red, 1.0f-red);
            if (GUILayout.Button(names[i]))
            {
                QualitySettings.SetQualityLevel(i, true);
            }
        }
        GUI.color = Color.white;
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}

