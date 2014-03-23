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
    private static Shader currentAddShader;
    private static float currentAddDistance;

    private static Shader[] copyShaders;
    private static float[] copyDistances;
    private static string copyFrom;

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
        if (sFoldoutLocationObject == false)
        {
            return;
        }
        Draw(target as LocationObject, targets);
	}

    private void AddShader(Shader shader, float distance, UnityEngine.Object[] targets)
    {
        foreach (var t in targets)
        {
            var target = t as LocationObject;
            if (target != null)
            {
                var shaderList = target.shaderList != null ? target.shaderList.ToList() : new List<Shader>();
                var shaderDistances = target.shaderList != null ? target.shaderDistances.ToList() : new List<float>();
                shaderList.Add(shader);
                shaderDistances.Add(distance);
                target.shaderList = shaderList.ToArray();
                target.shaderDistances = shaderDistances.ToArray();
            }
        }
    }

    private void RemoveShader(Shader shader, float distance, UnityEngine.Object[] targets)
    {
        foreach (var t in targets)
        {
            var target = t as LocationObject;
            if (target != null && target.shaderList != null)
            {
                var changed = true;
                while (changed)
                {
                    changed = false;
                    var i = 0;
                    var len = target.shaderList.Length;
                    for (; i < len; i++)
                    {
                        var s = target.shaderList[i];
                        var d = target.shaderDistances[i];
                        if (d == distance && s == shader)
                        {
                            changed = true;
                            var shaderList = target.shaderList.ToList();
                            shaderList.RemoveAt(i);
                            var shaderDistance = target.shaderDistances.ToList();
                            shaderDistance.RemoveAt(i);
                            target.shaderList = shaderList.ToArray();
                            target.shaderDistances = shaderDistance.ToArray();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void ChangeDistance(Shader shader, float lastDistance, float newDistance, UnityEngine.Object[] targets)
    {
        foreach (var t in targets)
        {
            var target = t as LocationObject;
            if (target != null && target.shaderList != null)
            {
                var i = 0;
                var len = target.shaderList.Length;
                for (; i < len; i++)
                {
                    var s = target.shaderList[i];
                    var d = target.shaderDistances[i];
                    if (s == shader && d == lastDistance)
                    {
                        d = newDistance;
                        target.shaderDistances[i] = d;
                    }
                }
            }
        }
    }

    private void DrawCopyShader(LocationObject target)
    {
        GUILayout.BeginHorizontal();
        GUI.color = Color.grey;
        if (target != null)
        {
            if (target.shaderList != null && target.shaderList.Length > 0 && GUILayout.Button("COPY"))
            {
                copyShaders = target.shaderList.ToArray();
                copyDistances = target.shaderDistances.ToArray();
                copyFrom = target.name;
            }
        }
        if (copyShaders != null)
        {
            if (GUILayout.Button("PASTE FROM: " + copyFrom))
            {
                foreach (var item in targets)
                {
                    var currentTarget = item as LocationObject;
                    if (currentTarget != null)
                    {
                        currentTarget.shaderList = copyShaders.ToArray();
                        currentTarget.shaderDistances = copyDistances.ToArray();
                    }
                }
            }
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();
    }

    private void DrawClear(UnityEngine.Object[] targets)
    {
        GUI.color = Color.gray;
        if (GUILayout.Button("CLEAR"))
        {
            foreach (var t in targets)
            {
                var target = t as LocationObject;
                if (target != null)
                {
                    target.shaderList = new Shader[0];
                    target.shaderDistances = new float[0];
                }
            }
        }
        GUI.color = Color.white;
    }

    private void DrawSortShaders(UnityEngine.Object[] targets)
    {
        GUI.color = Color.magenta;
        if (GUILayout.Button("SORT"))
        {
            foreach (var t in targets)
            {
                var target = t as LocationObject;
                if (target != null)
                {
                    var shaderList = target.shaderList;
                    var shaderDistance = target.shaderDistances;
                    if (shaderList != null && shaderList.Length > 0)
                    {
                        sort(shaderDistance, shaderList, 0, shaderList.Length - 1, true);
                    }
                }
            }
        }
        GUI.color = Color.white;
    }

    private bool HasCopy(LocationObject target, int index, float distance)
    {
        var i = 0;
        var shaderList = target.shaderList;
        var shaderDistance = target.shaderDistances;
        if (shaderList != null && shaderDistance != null)
        {
            var len = shaderList.Length;
            for (; i < len; i++)
            {
                var s = shaderList[i];
                var d = shaderDistance[i];
                if (index != i)
                {
                    if (d == distance)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void Draw(LocationObject target, UnityEngine.Object[] targets)
    {
        var locationObject = target;
        if (locationObject != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical();

            DrawClear(targets);
            DrawCopyShader(locationObject);

            GUI.color = Color.green;
            locationObject.shaderCulling = EditorGUILayout.Toggle("Shader Culling", locationObject.shaderCulling);
            GUI.color = Color.white;
            EditorGUILayout.Space();
            
            var shaderList = locationObject.shaderList;
            if (shaderList == null || shaderList.Length == 0)
            {
                GUI.color = Color.yellow;
                GUILayout.Box("empty texture, use default");
                GUI.color = Color.white;
            }
            else
            {
                
                DrawSortShaders(targets);
                EditorGUILayout.Separator();

                var shaderDistance = locationObject.shaderDistances;

                var i = 0;
                var len = shaderList.Length;
                for(; i < len; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();

                    var shader = shaderList[i];
                    var distance = shaderDistance[i];

                    var result = HasCopy(locationObject, i, distance);
                    if (result)
                    {
                        GUI.color = Color.red;
                    }
                    else
                    {
                        GUI.color = Color.green;
                    }
                    if (shader != null)
                    {
                        GUILayout.Box(shader.name, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        GUILayout.Box("null", GUILayout.ExpandWidth(true));
                    }

                    
                    if (result)
                    {
                        GUI.color = Color.red;
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }
                    var lastDistance = distance;
                    var newDistance = EditorGUILayout.FloatField("Distance To Camera", lastDistance);
                    if (newDistance != lastDistance)
                    {
                        ChangeDistance(shader, lastDistance, newDistance, targets);
                        distance = newDistance;
                    }

                    EditorGUILayout.EndVertical();
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.ExpandHeight(true), GUILayout.Width(20.0f)) || shader == null)
                    {
                        RemoveShader(shader, distance, targets);
                    }
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            GUI.color = Color.yellow;
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            currentAddShader = EditorGUILayout.ObjectField("DragToAdd", currentAddShader, typeof(Shader)) as Shader;
            if (currentAddShader != null)
            {
                currentAddDistance = EditorGUILayout.FloatField("Distance", currentAddDistance);
            }
            GUILayout.EndVertical();
            if (currentAddShader != null)
            {
                if (GUILayout.Button("ADD"))
                {
                    AddShader(currentAddShader, currentAddDistance, targets);
                }
            }
            GUILayout.EndHorizontal();
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

    private static void sort(float[] distances, Shader[] shaders, int left, int right, bool reverse = false)
    {
        int i = left;
        int j = right;
        float x = distances[(int)((left + right) >> 1)];
        do
        {
            if (reverse)
            {
                while (distances[i] > x)
                    i++;
                while (distances[j] < x)
                    j--;
            }
            else
            {
                while (distances[i] < x)
                    i++;
                while (distances[j] > x)
                    j--;
            }

            if (i <= j)
            {
                float tempDist = distances[i];
                Shader tempShader = shaders[i];
                distances[i] = distances[j];
                distances[j] = tempDist;

                shaders[i] = shaders[j];
                shaders[j] = tempShader;
                i++;
                j--;
            }
        } while (i < j);
        if (left < j)
            sort(distances, shaders, left, j, reverse);
        if (i < right)
            sort(distances, shaders, i, right, reverse);
    }
}


