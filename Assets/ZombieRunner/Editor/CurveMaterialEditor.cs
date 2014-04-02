using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class CurveMaterialEditor : MaterialEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!isVisible)
            return;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        GUI.color = Color.green;

        Material targetMat = target as Material;
        string[] keyWords = targetMat.shaderKeywords;

        bool redify = keyWords.Contains("UNITY_EDITOR_SHADOW_ON");
        EditorGUI.BeginChangeCheck();
        redify = EditorGUILayout.ToggleLeft("   FIX UNITY EDITOR SHADOW (only editor)", redify);
        if (EditorGUI.EndChangeCheck())
        {
            if (redify)
                targetMat.shaderKeywords = new string[1] { "UNITY_EDITOR_SHADOW_ON" };
            else
                targetMat.shaderKeywords = new string[1] { "UNITY_EDITOR_SHADOW_OFF" };

            EditorUtility.SetDirty(targetMat);
        }
        GUI.color = Color.white;
    }
}
