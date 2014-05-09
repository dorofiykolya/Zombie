using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Runner;

public class ShaderQualityWindowEditor : EditorWindow
{

    private Shader currentAddShader;
    private float currentAddDistance;
    private int currentAddQuality = Math.Max(0, QualitySettings.names.Length - 1);

    private Shader[] copyShaders;
    private float[] copyDistances;
    private int[] copyQuality;
    //private bool copyCulling = true;

    private string copyFrom;

    private bool copyFold = true;
    private bool selectFold = true;
    private Runner.LocationObject[] selectList;
    private string whereName;
    private Vector2 scrollPosition;

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        copyFold = EditorGUILayout.Foldout(copyFold, "Copy Shader");
        if (copyFold)
        {
            Draw();
        }

        selectFold = EditorGUILayout.Foldout(selectFold, "Copy Shader");
        if (selectFold)
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();
            GUILayout.Box("SELECT LIST");
            DrawCopyShader();

            EditorGUILayout.Separator();
            selectList = Selection.GetFiltered(typeof(Runner.LocationObject), SelectionMode.Assets).Cast<Runner.LocationObject>().ToArray();
            GUI.color = Color.green;

            EditorGUILayout.Separator();
            if (selectList == null || selectList.Length == 0)
            {
                GUILayout.Label("empty select list");
            }
            else
            {
                foreach (var o in selectList)
                {
                    GUILayout.Label(o.name);
                }
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawClear()
    {
        GUI.color = Color.gray;
        if (GUILayout.Button("CLEAR"))
        {
            copyShaders = new Shader[0];
            copyDistances = new float[0];
            copyQuality = new int[0];
        }
        GUI.color = Color.white;
    }

    private void DrawPaste()
    {
        if (LocationObjectEditor.lastCopyShaders != null && LocationObjectEditor.lastCopyShaders.Length > 0)
        {
            GUI.color = Color.gray;
            if (GUILayout.Button("PASTE FROM: " + LocationObjectEditor.lastCopyFrom))
            {
                copyShaders = LocationObjectEditor.lastCopyShaders;
                copyDistances = LocationObjectEditor.lastCopyDistances;
                copyQuality = LocationObjectEditor.lastCopyQuality;
            }
            GUI.color = Color.white;
        }
        if (LocationObjectEditor.copyShaders != null && LocationObjectEditor.copyShaders.Length > 0)
        {
            GUI.color = Color.gray;
            if (GUILayout.Button("PASTE FROM: " + LocationObjectEditor.copyFrom))
            {
                copyShaders = LocationObjectEditor.copyShaders;
                copyDistances = LocationObjectEditor.copyDistances;
                copyQuality = LocationObjectEditor.copyQuality;
            }
            GUI.color = Color.white;
        }
    }

    private void DrawSortShaders()
    {
        GUI.color = Color.magenta;
        if (GUILayout.Button("SORT"))
        {
            if (copyShaders != null && copyShaders.Length > 0)
            {
                sort(copyDistances, copyShaders, copyQuality, 0, copyShaders.Length - 1, true);
            }
        }
        GUI.color = Color.white;
    }

    private void DoPaste(bool recursive = false, GameObject go = null)
    {
        if (selectList == null || selectList.Length == 0) return;
        foreach (var l in selectList)
        {
            if (recursive)
            {
                RecursivePaste(l.gameObject);
            }
            if (!string.IsNullOrEmpty(whereName) && whereName != l.name) continue;
            l.shaderList = copyShaders.ToArray();
            l.shaderDistances = copyDistances.ToArray();
            l.shaderQualities = copyQuality.ToArray();
        }
    }

    private void RecursivePaste(GameObject o)
    {
        if (string.IsNullOrEmpty(whereName) || whereName == o.name)
        {
            var lo = o.GetComponent<Runner.LocationObject>();
            if (lo != null)
            {
                lo.shaderList = copyShaders.ToArray();
                lo.shaderDistances = copyDistances.ToArray();
                lo.shaderQualities = copyQuality.ToArray();
            }
        }

        foreach (var g in o.GetChildren())
        {
            RecursivePaste(g);
        }
    }

    private void DoEnable(bool recursive = false)
    {
        if (selectList == null || selectList.Length == 0) return;
        foreach (var l in selectList)
        {
            if (recursive)
            {
                RecursiveEnabled(l.gameObject);
            }
            if (!string.IsNullOrEmpty(whereName) && whereName != l.name) continue;
            l.shaderCulling = true;
        }
    }

    private void RecursiveEnabled(GameObject o)
    {
        if (string.IsNullOrEmpty(whereName) || whereName == o.name)
        {
            var lo = o.GetComponent<Runner.LocationObject>();
            if (lo != null)
            {
                lo.shaderCulling = true;
            }
        }

        foreach (var g in o.GetChildren())
        {
            RecursiveEnabled(g);
        }
    }

    private void DoDisable(bool recursive = false)
    {
        if (selectList == null || selectList.Length == 0) return;
        foreach (var l in selectList)
        {
            if (recursive)
            {
                RecursiveDisabled(l.gameObject);
            }
            if (!string.IsNullOrEmpty(whereName) && whereName != l.name) continue;
            l.shaderCulling = false;
        }
    }

    private void RecursiveDisabled(GameObject o)
    {
        if (string.IsNullOrEmpty(whereName) || whereName == o.name)
        {
            var lo = o.GetComponent<Runner.LocationObject>();
            if (lo != null)
            {
                lo.shaderCulling = false;
            }
        }

        foreach (var g in o.GetChildren())
        {
            RecursiveDisabled(g);
        }
    }

    private void DrawCopyShader()
    {
        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        whereName = EditorGUILayout.TextField("WHERE NAME", whereName);
        GUI.color = Color.red;
        if (GUILayout.Button(" ", GUILayout.Width(20.0f), GUILayout.Height(12.0f)))
        {
            whereName = string.Empty;
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        GUI.color = Color.white;
        if (copyShaders != null && copyShaders.Length > 0)
        {
            if (GUILayout.Button("PASTE"))
            {
                DoPaste();
            }
        }

        if (copyShaders != null && copyShaders.Length > 0)
        {
            if (GUILayout.Button("PASTE RECURSIVE"))
            {
                DoPaste(true);
            }
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        GUI.color = Color.green;
        if (copyShaders != null && copyShaders.Length > 0)
        {
            if (GUILayout.Button("ENABLE CULLING"))
            {
                DoEnable();
            }
        }
        if (copyShaders != null && copyShaders.Length > 0)
        {
            if (GUILayout.Button("ENABLE CULLING RECURSIVE"))
            {
                DoEnable(true);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUI.color = Color.grey;
        if (copyShaders != null && copyShaders.Length > 0)
        {
            if (GUILayout.Button("DISABLE CULLING"))
            {
                DoDisable();
            }
        }
        if (copyShaders != null && copyShaders.Length > 0)
        {
            if (GUILayout.Button("DISABLE CULLING RECURSIVE"))
            {
                DoDisable(true);
            }
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
    }

    private void AddShader(Shader shader, float distance, int minQuality)
    {
        var shaderList = copyShaders != null ? copyShaders.ToList() : new List<Shader>();
        var shaderDistances = copyDistances != null ? copyDistances.ToList() : new List<float>();
        var shaderQuality = copyQuality != null ? copyQuality.ToList() : new List<int>();
        shaderList.Add(shader);
        shaderDistances.Add(distance);
        shaderQuality.Add(minQuality);
        copyShaders = shaderList.ToArray();
        copyDistances = shaderDistances.ToArray();
        copyQuality = shaderQuality.ToArray();
    }

    private void RemoveShader(Shader shader, float distance)
    {
        if (copyShaders == null) return;
        var changed = true;
        while (changed)
        {
            changed = false;
            var i = 0;
            var len = copyShaders.Length;
            for (; i < len; i++)
            {
                var s = copyShaders[i];
                var d = copyDistances[i];
                var q = copyQuality[i];
                if (d == distance && s == shader)
                {
                    changed = true;
                    var shaderList = copyShaders.ToList();
                    shaderList.RemoveAt(i);
                    var shaderDistance = copyDistances.ToList();
                    shaderDistance.RemoveAt(i);
                    var shaderQuality = copyQuality.ToList();
                    shaderQuality.RemoveAt(i);
                    copyShaders = shaderList.ToArray();
                    copyDistances = shaderDistance.ToArray();
                    copyQuality = shaderQuality.ToArray();
                    break;
                }
            }
        }
    }

    private void ChangeDistance(Shader shader, float lastDistance, float newDistance)
    {
        if (copyShaders == null) return;
        var i = 0;
        var len = copyShaders.Length;
        for (; i < len; i++)
        {
            var s = copyShaders[i];
            var d = copyDistances[i];
            if (s == shader && d == lastDistance)
            {
                d = newDistance;
                copyDistances[i] = d;
            }
        }
    }

    private void ChangeQuality(Shader shader, float distance, int minQuality)
    {
        if (copyShaders == null) return;
        var i = 0;
        var len = copyShaders.Length;
        for (; i < len; i++)
        {
            var s = copyShaders[i];
            var d = copyDistances[i];
            if (s == shader && d == distance)
            {
                copyQuality[i] = minQuality;
            }
        }
    }

    private void Draw()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10.0f);
        GUILayout.BeginVertical();

        DrawPaste();
        DrawClear();


        //GUI.color = Color.green;
        //copyCulling = EditorGUILayout.Toggle("Shader Culling", copyCulling);
        //GUI.color = Color.white;
        EditorGUILayout.Space();

        var shaderList = copyShaders;
        if (shaderList == null || shaderList.Length == 0)
        {
            GUI.color = Color.yellow;
            GUILayout.Box("empty texture, use default");
            GUI.color = Color.white;
        }
        else
        {

            DrawSortShaders();
            EditorGUILayout.Separator();

            var shaderDistance = copyDistances;
            var shaderQuality = copyQuality;

            var qualityList = new int[QualitySettings.names.Length];
            for (var qi = 0; qi < qualityList.Length; qi++) qualityList[qi] = qi;
            var qualityNames = new string[QualitySettings.names.Length];
            for (var qi = 0; qi < qualityNames.Length; qi++) qualityNames[qi] = QualitySettings.names[qi];

            var i = 0;
            var len = shaderList.Length;
            for (; i < len; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

                var shader = shaderList[i];
                var distance = shaderDistance[i];
                var quality = shaderQuality[i];

                GUI.color = Color.green;
                if (shader != null)
                {
                    GUILayout.Box(shader.name, GUILayout.ExpandWidth(true));
                }
                else
                {
                    GUILayout.Box("null", GUILayout.ExpandWidth(true));
                }
                GUI.color = Color.white;

                var lastDistance = distance;
                var newDistance = EditorGUILayout.FloatField("Distance To Camera", lastDistance);
                if (newDistance != lastDistance)
                {
                    ChangeDistance(shader, lastDistance, newDistance);
                    distance = newDistance;
                }

                var lastQuality = quality;
                var newQuality = EditorGUILayout.IntPopup("Min Quality", lastQuality, qualityNames, qualityList);
                if (newQuality != lastQuality)
                {
                    ChangeQuality(shader, distance, newQuality);
                }

                EditorGUILayout.EndVertical();
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Height(55.0f), GUILayout.Width(20.0f)) || shader == null)
                {
                    RemoveShader(shader, distance);
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
        GUI.color = Color.yellow;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

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
                AddShader(currentAddShader, currentAddDistance, currentAddQuality);
            }
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

    }



    private void sort(float[] distances, Shader[] shaders, int[] minQuality, int left, int right, bool reverse = false)
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

    [MenuItem("Runner/Shader Quality Settings")]
    public static void ShowShaderQualityWindowEditor()
    {
        EditorWindow.GetWindow<ShaderQualityWindowEditor>("Shader Quality Settings", true).Show();
    }

}

