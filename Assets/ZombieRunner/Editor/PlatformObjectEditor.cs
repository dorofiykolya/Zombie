using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformObject))]
[CanEditMultipleObjects]
class PlatformObjectEditor : LocationObjectEditor
{
    private bool sFoldoutStatus;
    private bool sFoldoutNext;
    private bool sFoldoutPlatformObject = true;

    public override void OnInspectorGUI()
    {
		base.OnInspectorGUI();

        if (location == null)
        {
            return;
        }
        GUI.color = ColorEditor.Title;
        sFoldoutPlatformObject = EditorGUILayout.Foldout(sFoldoutPlatformObject, "PlatformObject Settings");
        GUI.color = Color.white;
        if (sFoldoutPlatformObject == false) return;
        GUILayout.BeginHorizontal();
        GUILayout.Space(10.0f);
        GUILayout.BeginVertical();
        Draw();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void Draw()
    {
        var platform = target as PlatformObject;
        if (platform != null)
        {
            platform.MinimumDistance = EditorGUILayout.FloatField("Min Distance", platform.MinimumDistance);
            platform.Type = EditorGUILayout.IntField("Type", platform.Type);
            platform.Mode = (PlatformMode)EditorGUILayout.EnumPopup("Mode", platform.Mode);
            if (platform.Mode == PlatformMode.Transition)
            {
                if (location.startPlatforms.Contains(platform))
                {
                    EditorUtility.DisplayDialog("Error", "You can not set this mode, the object is already in the startPlatforms list", "close");
                    platform.Mode = PlatformMode.Platform;
                    return;
                }
                if (location.platforms.Contains(platform))
                {
                    EditorUtility.DisplayDialog("Error", "You can not set this mode, the object is already in the platforms list", "close");
                    platform.Mode = PlatformMode.Platform;
                    return;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(10.0f);
                GUI.color = Color.cyan;
                platform.TypeTo = EditorGUILayout.IntField("Type To", platform.TypeTo);
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
            }
            else
            {
                if (location.transitionPlatforms.Contains(platform))
                {
                    EditorUtility.DisplayDialog("Error", "You can not set this mode, the object is already in the transition list", "close");
                    platform.Mode = PlatformMode.Transition;
                    return;
                }
            }

            NextPlatform(platform);

            sFoldoutStatus = EditorGUILayout.Foldout(sFoldoutStatus, "Status");
            if (sFoldoutStatus)
            {
                GUI.color = Color.grey;
                GUILayout.Label("Id: " + platform.Id);
                GUILayout.Label("Size: " + platform.Size.ToString());
                DrawBoolLabel("In View Platform List: ", platform.InPlatformList);
                DrawBoolLabel("Is Start Platform: ", platform.IsStartPlatform);
                DrawBoolLabel("Allow Dispose: ", platform.AllowDispose);
            }
        }
    }

    private void NextPlatform(PlatformObject platform)
    {
        sFoldoutNext = EditorGUILayout.Foldout(sFoldoutNext, "Next Platforms");
        if (sFoldoutNext)
        {
            var result = from p in platform.NextPlatforms
                         where p != null
                         select p;
            if (result.Count() != platform.NextPlatforms.Length)
            {
                GUI.color = Color.red;
                GUILayout.Label("Has Error, correct!!!");
                GUI.color = Color.green;
                if (GUILayout.Button("Correct"))
                {
                    platform.NextPlatforms = result.ToArray();
                }
                GUI.color = Color.white;
                return;
            }

            GUI.color = Color.yellow;
            var add = (Runner.PlatformObject)EditorGUILayout.ObjectField("DragToAdd", null, typeof(Runner.PlatformObject));
            GUI.color = Color.white;
            if (add)
            {
                if (add == platform)
                {
                    EditorUtility.DisplayDialog("Error", "This reference to your object", "close");
                }
                else
                    if (result.Contains(add))
                    {
                        EditorUtility.DisplayDialog("Error", "This object is exist already", "close");
                    }
                    else
                    {
                        var list = result.ToList();
                        list.Add(add);
                        platform.NextPlatforms = list.ToArray();
                    }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();
            foreach (var nextPlatform in platform.NextPlatforms)
            {
                GUILayout.BeginHorizontal();
                GUI.color = Color.cyan;
                GUILayout.Label(nextPlatform.name);
                GUI.color = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(12.0f), GUILayout.Height(12.0f)))
                {
                    var list = platform.NextPlatforms.ToList();
                    list.Remove(nextPlatform);
                    platform.NextPlatforms = list.ToArray();
                    break;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
        }
    }
}

