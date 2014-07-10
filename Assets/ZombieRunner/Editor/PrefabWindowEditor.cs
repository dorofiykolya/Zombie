using System;
using System.Linq;
using Runner;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

public class PlatformPrefabWindowEditor : EditorWindow
{
    private static Vector2 sScrollView;
    private static bool sFoldoutSelected = true;

    public PlatformPrefabWindowEditor()
    {

    }

    void Update()
    {
        
    }

    void OnGUI()
    {
        var settings = GameObject.FindObjectOfType<SettingManager>();
        if (settings == null)
        {
            GUI.color = Color.red;
            GUILayout.Label("ERROR, SettingManager not found!!!");
            GUI.color = Color.white;
            return;
        }

        GUI.color = Color.white;
        settings.PlatformPrefabLocalRotation = EditorGUILayout.Vector3Field("Local Rotation On Create", settings.PlatformPrefabLocalRotation);
        EditorGUILayout.Separator();

        sScrollView = EditorGUILayout.BeginScrollView(sScrollView);

        var selection = Selection.GetFiltered(typeof(MeshFilter), SelectionMode.Assets);

        //
        sFoldoutSelected = EditorGUILayout.Foldout(sFoldoutSelected, "SelectedObjects");
        if (sFoldoutSelected)
        {
            GUI.color = new Color(85.0f / 255.0f, 170.0f / 255.0f, 255.0f / 255.0f);
            foreach (var selected in selection)
            {
                if (PrefabUtility.GetPrefabParent(selected) == null && PrefabUtility.GetPrefabObject(selected) != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    GUI.color = Color.cyan;
                    if (GUILayout.Button(new GUIContent("s", "select object"), GUILayout.Width(20)))
                    {
                        Selection.activeObject = selected;
                    }
                    GUI.color = ColorEditor.Title;
                    GUILayout.Label(selected.ToString() + " [" + PrefabUtility.GetPrefabType(selected).ToString() + "]");
                    GUI.color = Color.green;
                    if (GUILayout.Button("create platform prefab"))
                    {
                        var gameObject = ((MeshFilter)selected).gameObject;

                        var localPath = settings.PlatformPrefabPathSave + selected.name + SettingManager.PrefabExtension;
                        if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)) != null)
                        {
                            if (EditorUtility.DisplayDialog("Are you sure?",
                                "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
                            {
                                CreateNew(gameObject, localPath, settings.PlatformPrefabLocalRotation);
                            }
                        }
                        else
                        {
                            CreateNew(gameObject, localPath, settings.PlatformPrefabLocalRotation);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Runner/Prefab/Platform Prefab Editor")]
    private static void OpenEditor()
    {
        EditorWindow.GetWindow<PlatformPrefabWindowEditor>("Platform Prefab Editor", true).Show();
    }


    [MenuItem("Runner/Prefab/Create Platform Prefab From Selected")]
    public static void CreatePrefab()
    {
        var settings = GameObject.FindObjectOfType<SettingManager>();
        if (settings == null)
        {
            EditorUtility.DisplayDialog("ERROR", "SettingManager not found!!!", "close");
            return;
        }
        var selection = Selection.GetFiltered(typeof(MeshFilter), SelectionMode.Assets);

        foreach (var selected in selection)
        {
            if (PrefabUtility.GetPrefabParent(selected) == null && PrefabUtility.GetPrefabObject(selected) != null)
            {
                var gameObject = ((MeshFilter)selected).gameObject;

                var localPath = settings.PlatformPrefabPathSave + selected.name + SettingManager.PrefabExtension;
                if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)) != null)
                {
                    if (EditorUtility.DisplayDialog("Are you sure?",
                        "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
                    {
                        CreateNew(gameObject, localPath, settings.PlatformPrefabLocalRotation);
                    }
                }
                else
                {
                    CreateNew(gameObject, localPath, settings.PlatformPrefabLocalRotation);
                }
            }
        }
    }

    [MenuItem("Runner/Prefab/Create Platform Prefab From Selected", true)]
    public static bool ValidateCreatePrefab()
    {
        var settings = GameObject.FindObjectOfType<SettingManager>();
        if (settings == null)
        {
            return false;
        }
        var selected = Selection.GetFiltered(typeof(MeshFilter), SelectionMode.Assets);
        return selected != null && selected.Length > 0;
    }

    [MenuItem("Runner/Prefab/Clone PlatformObject Prefab From Selected")]
    public static void ClonePlatformPrefab()
    {
        var settings = GameObject.FindObjectOfType<SettingManager>();
        if (settings == null)
        {
            EditorUtility.DisplayDialog("ERROR", "SettingManager not found!!!", "close");
            return;
        }
        var selected = Selection.GetFiltered(typeof(PlatformObject), SelectionMode.Assets);
        if (selected != null && selected.Length > 0)
        {
            foreach (var s in selected)
            {
                if (PrefabUtility.GetPrefabParent(s) == null && PrefabUtility.GetPrefabObject(s) != null && PrefabUtility.GetPrefabType(s) == PrefabType.Prefab)
                {
                    CreateNew(((PlatformObject)s).gameObject, settings.PlatformPrefabPathSave + s.name + "(Clone)" + SettingManager.PrefabExtension, new Vector3(float.NaN, float.NaN, float.NaN));
                }
            }
        }
    }

    [MenuItem("Runner/Prefab/Clone PlatformObject Prefab From Selected", true)]
    public static bool ValidatePlatformClonePrefab()
    {
        var settings = GameObject.FindObjectOfType<SettingManager>();
        if (settings == null)
        {
            return false;
        }
        var selected = Selection.GetFiltered(typeof(PlatformObject), SelectionMode.Assets);
        if (selected != null && selected.Length > 0)
        {
            return selected.Any(s => PrefabUtility.GetPrefabParent(s) == null && PrefabUtility.GetPrefabObject(s) != null && PrefabUtility.GetPrefabType(s) == PrefabType.Prefab);
        }
        return false;
    }

    private static void CreateNew(GameObject obj, string localPath, Vector3 rotation)
    {
        obj = (GameObject)GameObject.Instantiate(obj);

        var rigit = obj.GetComponent<Rigidbody>();
        if (rigit == null)
        {
            obj.AddComponent<Rigidbody>();
            rigit = obj.GetComponent<Rigidbody>();
        }
        if (rigit != null)
        {
            rigit.useGravity = false;
            rigit.constraints = RigidbodyConstraints.FreezeAll;
        }

        var collider = obj.GetComponent<BoxCollider>();
        if (collider == null)
        {
            obj.AddComponent<BoxCollider>();
            collider = obj.GetComponent<BoxCollider>();
        }
        if (collider != null)
        {
            collider.isTrigger = false;
            var v3 = collider.center;
            if (v3.y > 0)
            {
                v3.y = -v3.y / 2.0f;
                collider.center = v3;
            }
        }
        var mrender = obj.GetComponent<MeshRenderer>();
        if (mrender == null)
        {
            obj.AddComponent<MeshRenderer>();
            mrender = obj.GetComponent<MeshRenderer>();
        }
        if (mrender != null)
        {
            mrender.castShadows = false;
            mrender.receiveShadows = false;
        }
        obj.transform.position = Vector3.zero;
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (float.IsNaN(rotation.x) == false && float.IsNaN(rotation.y) == false && float.IsNaN(rotation.z) == false)
        {
            var r = obj.transform.localRotation;
            r.eulerAngles = rotation;
            obj.transform.localRotation = r;
        }
        else
        {
            obj.transform.rotation = Quaternion.identity;
        }

        var platform = obj.GetComponent<Runner.PlatformObject>();
        if (platform == null)
        {
            obj.AddComponent<PlatformObject>();
        }

        var prefab = PrefabUtility.CreateEmptyPrefab(localPath);
        PrefabUtility.ReplacePrefab(obj, prefab);
        GameObject.DestroyImmediate(obj);
    }
}

