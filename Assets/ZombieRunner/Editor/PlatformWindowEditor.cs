using System.Security.Cryptography;
using Runner;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Linq;

public class PlatformWindowEditor : EditorWindow
{
    private static Vector2 sScrollView = new Vector2();
    private static bool sFoldoutSelected;
    private static bool sFoldoutPlatforms;
    private static bool sFoldoutStartPlatforms;
    private static bool sFoldoutTransitionPlatforms;
    private Runner.LocationManager target;

    void OnGUI()
    {
        target = GameObject.FindObjectOfType<Runner.LocationManager>();
        if (target == null)
        {
            GUI.color = Color.red;
            GUILayout.Label("ERROR, LocationManager not found!!!");
            GUI.color = Color.white;
            return;
        }

        sScrollView = EditorGUILayout.BeginScrollView(sScrollView);

        GUI.color = ColorEditor.Title;
        GUILayout.Label("Select prefabs to add to the list");
        GUI.color = Color.white;

        var selection = Selection.GetFiltered(typeof(Runner.PlatformObject), SelectionMode.Assets);

        sFoldoutSelected = EditorGUILayout.Foldout(sFoldoutSelected, "Selected Objects");
        if (sFoldoutSelected)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();

            GUI.color = ColorEditor.Title;
            var contains = false;
            foreach (var s in selection)
            {
                SelectedListPlaceholder(s);
                contains = true;
            }
            if (contains == false)
            {
                GUILayout.Label("no selected objects");
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();

        GUI.color = ColorEditor.Title;
        GUILayout.Label("Lists");
        GUI.color = Color.white;

        sFoldoutPlatforms = EditorGUILayout.Foldout(sFoldoutPlatforms, "Platforms");
        if (sFoldoutPlatforms)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();

            GUI.color = ColorEditor.Title;
            var contains = false;
            foreach (var s in target.platforms)
            {
                SelectedListPlaceholder(s);
                contains = true;
            }
            if (contains == false)
            {
                GUILayout.Label("no platforms");
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        sFoldoutTransitionPlatforms = EditorGUILayout.Foldout(sFoldoutTransitionPlatforms, "Transition Platforms");
        if (sFoldoutTransitionPlatforms)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();

            GUI.color = ColorEditor.Title;
            var contains = false;
            foreach (var s in target.transitionPlatforms)
            {
                SelectedListPlaceholder(s);
                contains = true;
            }
            if (contains == false)
            {
                GUILayout.Label("no transition platforms");
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        sFoldoutStartPlatforms = EditorGUILayout.Foldout(sFoldoutStartPlatforms, "Start Platforms");
        if (sFoldoutStartPlatforms)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();

            GUI.color = ColorEditor.Title;
            var contains = false;
            foreach (var s in target.startPlatforms)
            {
                SelectedListPlaceholder(s);
                contains = true;
            }
            if (contains == false)
            {
                GUILayout.Label("no start platforms");
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (target.platforms.Any(s => s == null) || target.startPlatforms.Any(s => s == null) ||
            target.transitionPlatforms.Any(s => s == null))
        {
            GUI.color = ColorEditor.RgbToColor(255, 50, 50);
            GUILayout.Label("There are errors, correct please!", EditorStyles.boldLabel);
        }

        GUI.color = Color.cyan;
        if (GUILayout.Button("Auto Correct"))
        {
            var platforms = from p in target.platforms
                where p != null && p.Mode == PlatformMode.Platform
                select p;
            target.platforms = platforms.ToArray();

            var start = from s in target.startPlatforms
                where s != null && s.Mode == PlatformMode.Platform
                select s;
            target.startPlatforms = start.ToArray();

            var trans = from t in target.transitionPlatforms
                where t != null && t.Mode == PlatformMode.Transition
                select t;
            target.transitionPlatforms = trans.ToArray();

        }
        GUI.color = Color.white;

        EditorGUILayout.EndScrollView();
    }

    private void SelectedListPlaceholder(UnityEngine.Object current)
    {
        if(current == null) return;
        
        GUILayout.BeginHorizontal();
        GUI.color = Color.cyan;
        if (GUILayout.Button(new GUIContent("s", "select object"), GUILayout.Width(20)))
        {
            Selection.activeObject = current;
        }
        GUI.color = ColorEditor.Title;
        GUILayout.Label(current.ToString(), GUILayout.Width(260.0f));
        var inPlatforms = Contains(current, target.platforms);
        var inStartPlatforms = Contains(current, target.startPlatforms);
        var inTransitions = Contains(current, target.transitionPlatforms);
        if (inPlatforms)
        {
            GUI.color = ColorEditor.RgbToColor(250, 188, 0);
            GUILayout.Label(" [In  Platfroms]", GUILayout.ExpandWidth(true));
        }
        if (inStartPlatforms)
        {
            GUI.color = ColorEditor.RgbToColor(250, 125, 0);
            GUILayout.Label(" [In    Start    ]", GUILayout.ExpandWidth(true));
        }
        if (inTransitions)
        {
            GUI.color = ColorEditor.RgbToColor(200, 250, 0);
            GUILayout.Label(" [In Transition]", GUILayout.ExpandWidth(true));
        }
        if (inPlatforms || inStartPlatforms || inTransitions)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Remove"))
            {
                if (inPlatforms)
                {
                    var platformList = target.platforms.ToList();
                    platformList.Remove((Runner.PlatformObject)current);
                    target.platforms = platformList.ToArray();
                }
                else if (inStartPlatforms)
                {
                    var startList = target.startPlatforms.ToList();
                    startList.Remove((Runner.PlatformObject)current);
                    target.startPlatforms = startList.ToArray();
                }
                else if (inTransitions)
                {
                    var transitionList = target.transitionPlatforms.ToList();
                    transitionList.Remove((Runner.PlatformObject)current);
                    target.transitionPlatforms = transitionList.ToArray();
                }
            }
        }
        else
        {
            if (current is Runner.PlatformObject && ((Runner.PlatformObject)current).Mode == PlatformMode.Transition)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Add To Transition"))
                {
                    var transitionList = target.transitionPlatforms.ToList();
                    transitionList.Add((Runner.PlatformObject)current);
                    target.transitionPlatforms = transitionList.ToArray();
                }
            }
            else
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Add To Platfroms"))
                {
                    var platformList = target.platforms.ToList();
                    platformList.Add((Runner.PlatformObject)current);
                    target.platforms = platformList.ToArray();
                }
                GUI.color = Color.cyan;
                if (GUILayout.Button("Add To Start"))
                {
                    var startList = target.startPlatforms.ToList();
                    startList.Add((Runner.PlatformObject)current);
                    target.startPlatforms = startList.ToArray();
                }
            }
        }

        GUILayout.EndHorizontal();
    }

    private static bool Contains(UnityEngine.Object obj, UnityEngine.Object[] array)
    {
        var e = array.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current == obj)
            {
                return true;
            }
        }
        return false;
    }

    [MenuItem("Runner/Platform Editor")]
    static public void OpenPlatformWindow()
    {
        EditorWindow.GetWindow<PlatformWindowEditor>("Platform", true);
    }
}

