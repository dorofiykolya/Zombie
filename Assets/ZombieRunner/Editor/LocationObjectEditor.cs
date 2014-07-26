using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Runner.LocationObject))]
public class LocationObjectEditor : Editor
{
    private static Shader currentAddShader;
    private static float currentAddDistance;
    private static int currentAddQuality = Math.Max(0, QualitySettings.names.Length - 1);

    public static Shader[] copyShaders;
    public static float[] copyDistances;
    public static int[] copyQuality;
    public static string copyFrom;

    public static Shader[] lastCopyShaders;
    public static float[] lastCopyDistances;
    public static int[] lastCopyQuality;
    public static string lastCopyFrom;

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
		if (target as LocationObject != null) 
		{
			var components = ((LocationObject)target).GetComponents<LocationObject>();
			if(components.Length > 1)
			{
				var lastColor = GUI.color;
				GUI.color = Color.red;
				EditorGUILayout.LabelField("HAS DUPLICATE LOCATION OBJECT");
				GUI.color = lastColor;
			}
		}
        sFoldoutLocationObject = EditorGUILayout.Foldout(sFoldoutLocationObject, "LocationObject Settings");
        GUI.color = Color.white;
        if (sFoldoutLocationObject == false)
        {
            return;
        }
        Draw(target as LocationObject, targets);
	}

    private void AddShader(Shader shader, float distance, int minQuality, UnityEngine.Object[] targets)
    {
        foreach (var t in targets)
        {
            var target = t as LocationObject;
            if (target != null)
            {
                var shaderList = target.shaderList != null ? target.shaderList.ToList() : new List<Shader>();
                var shaderDistances = target.shaderList != null ? target.shaderDistances.ToList() : new List<float>();
                var shaderQuality = target.shaderList != null ? target.shaderQualities.ToList() : new List<int>();
                shaderList.Add(shader);
                shaderDistances.Add(distance);
                shaderQuality.Add(minQuality);
                target.shaderList = shaderList.ToArray();
                target.shaderDistances = shaderDistances.ToArray();
                target.shaderQualities = shaderQuality.ToArray();
                EditorUtility.SetDirty(target.gameObject);
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
                        var q = target.shaderQualities[i];
                        if (d == distance && s == shader)
                        {
                            changed = true;
                            var shaderList = target.shaderList.ToList();
                            shaderList.RemoveAt(i);
                            var shaderDistance = target.shaderDistances.ToList();
                            shaderDistance.RemoveAt(i);
                            var shaderQuality = target.shaderQualities.ToList();
                            shaderQuality.RemoveAt(i);
                            target.shaderList = shaderList.ToArray();
                            target.shaderDistances = shaderDistance.ToArray();
                            target.shaderQualities = shaderQuality.ToArray();
                            EditorUtility.SetDirty(target.gameObject);
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
                        EditorUtility.SetDirty(target.gameObject);
                    }
                }
            }
        }
    }

    private void ChangeQuality(Shader shader, float distance, int minQuality, UnityEngine.Object[] targets)
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
                    if (s == shader && d == distance)
                    {
                        target.shaderQualities[i] = minQuality;
                        EditorUtility.SetDirty(target.gameObject);
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
                lastCopyShaders = copyShaders;
                lastCopyDistances = copyDistances;
                lastCopyQuality = copyQuality;
                lastCopyFrom = copyFrom;

                copyShaders = target.shaderList.ToArray();
                copyDistances = target.shaderDistances.ToArray();
                copyQuality = target.shaderQualities.ToArray();
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
                        currentTarget.shaderQualities = copyQuality.ToArray();
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
		if (copyShaders != null)
		{
			if (GUILayout.Button("PASTE RECURSIVE FROM: " + copyFrom))
			{
				foreach (var item in targets)
				{
					var currentTarget = item as LocationObject;
					if (currentTarget != null)
					{
						currentTarget.shaderList = copyShaders.ToArray();
						currentTarget.shaderDistances = copyDistances.ToArray();
						currentTarget.shaderQualities = copyQuality.ToArray();

						PasteRecursive(currentTarget.gameObject);
					}
				}
			}
		}
		GUI.color = Color.white;
    }

	private void PasteRecursive(GameObject go)
	{
		if (go == null)
			return;

		var currentTarget = go.GetComponent<LocationObject>();
		if (currentTarget != null)
		{
			currentTarget.shaderList = copyShaders.ToArray();
			currentTarget.shaderDistances = copyDistances.ToArray();
			currentTarget.shaderQualities = copyQuality.ToArray();
            EditorUtility.SetDirty(currentTarget.gameObject);
		}

		foreach (var g in go.GetChildren()) {
			PasteRecursive(g);
		}
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
                    target.shaderQualities = new int[0];
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
                    var shaderQuality = target.shaderQualities;
                    if (shaderList != null && shaderList.Length > 0)
                    {
                        sort(shaderDistance, shaderList, shaderQuality, 0, shaderList.Length - 1, true);
                    }
                }
            }
        }
        GUI.color = Color.white;
    }

    private bool HasCopy(LocationObject target, int index, float distance, int quality)
    {
        var i = 0;
        var shaderList = target.shaderList;
        var shaderDistance = target.shaderDistances;
        var shaderQuality = target.shaderQualities;
        if (shaderList != null && shaderDistance != null)
        {
            var len = shaderList.Length;
            for (; i < len; i++)
            {
                var s = shaderList[i];
                var d = shaderDistance[i];
                var q = shaderQuality[i];
                if (index != i)
                {
                    if (d == distance && q == quality)
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
                var shaderQuality = locationObject.shaderQualities;

                var qualityList = new int[QualitySettings.names.Length];
                for (var qi = 0; qi < qualityList.Length; qi++) qualityList[qi] = qi;
                var qualityNames = new string[QualitySettings.names.Length];
                for (var qi = 0; qi < qualityNames.Length; qi++) qualityNames[qi] = QualitySettings.names[qi];

                var i = 0;
                var len = shaderList.Length;
                for(; i < len; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();

                    var shader = shaderList[i];
                    var distance = shaderDistance[i];
                    var quality = shaderQuality[i];

                    var result = HasCopy(locationObject, i, distance, quality);
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

                    var lastQuality = quality;
                    var newQuality = EditorGUILayout.IntPopup("Min Quality", lastQuality, qualityNames, qualityList);
                    if (newQuality != lastQuality)
                    {
                        ChangeQuality(shader, distance, newQuality, targets);
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

                var qualityList = new int[QualitySettings.names.Length];
                for (var qi = 0; qi < qualityList.Length; qi++) qualityList[qi] = qi;
                var qualityNames = new string[QualitySettings.names.Length];
                for (var qi = 0; qi < qualityNames.Length; qi++) qualityNames[qi] = QualitySettings.names[qi];

                currentAddQuality = EditorGUILayout.IntPopup("Min Quality", currentAddQuality, qualityNames, qualityList);
            }
            GUILayout.EndVertical();
            if (currentAddShader != null)
            {
                if (GUILayout.Button("ADD"))
                {
                    AddShader(currentAddShader, currentAddDistance, currentAddQuality, targets);
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

    private static void sort(float[] distances, Shader[] shaders, int[] minQuality, int left, int right, bool reverse = false)
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
                int tempMinQ = minQuality[i];
                distances[i] = distances[j];
                distances[j] = tempDist;

                shaders[i] = shaders[j];
                shaders[j] = tempShader;

                minQuality[i] = minQuality[j];
                minQuality[j] = tempMinQ;
                i++;
                j--;
            }
        } while (i < j);
        if (left < j)
            sort(distances, shaders, minQuality, left, j, reverse);
        if (i < right)
            sort(distances, shaders, minQuality, i, right, reverse);
    }
}


