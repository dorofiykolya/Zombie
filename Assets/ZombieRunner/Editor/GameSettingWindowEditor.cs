using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;

class GameSettingWindowEditor : EditorWindow
{
    private static Vector2 sScrollView;

    private static bool sFoldoutGame;
    private static bool sFoldoutCamera;
    private static bool sFoldoutFog;
    private static bool sFoldoutRenderSettings;
    private static bool sFoldoutSettings;

    void OnGUI()
    {
        sScrollView = EditorGUILayout.BeginScrollView(sScrollView);
        sFoldoutGame = EditorGUILayout.Foldout(sFoldoutGame, "Game");
        if (sFoldoutGame)
        {
            GameSettings();
        }
        sFoldoutCamera = EditorGUILayout.Foldout(sFoldoutCamera, "Camera");
        if (sFoldoutCamera)
        {
            CameraSettings();
        }
        sFoldoutFog = EditorGUILayout.Foldout(sFoldoutFog, "Fog");
        if (sFoldoutFog)
        {
            FogSettings();
        }
        sFoldoutRenderSettings = EditorGUILayout.Foldout(sFoldoutRenderSettings, "Render Settings");
        if (sFoldoutRenderSettings)
        {
            RenderSetting();
        }
        sFoldoutSettings = EditorGUILayout.Foldout(sFoldoutSettings, "Settings");
        if (sFoldoutSettings)
        {
            Settings();
        }

        EditorGUILayout.EndScrollView();
    }

    private void Settings()
    {
        

        GUILayout.BeginHorizontal();
        GUILayout.Space(30.0f);
        GUILayout.BeginVertical();

        var settings = GameObject.FindObjectOfType<SettingManager>();
        if (settings == null)
        {
            GUI.color = Color.red;
            GUILayout.Label("ERROR, SettingManager not found!!!");
        }
        else
        {
            settings.PlatformPrefabPathSave = EditorGUILayout.TextField("Platfrom prefab path save", settings.PlatformPrefabPathSave);
            if (settings.PlatformPrefabPathSave.EndsWith("/") == false)
            {
                settings.PlatformPrefabPathSave += "/";
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void RenderSetting()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30.0f);
        GUILayout.BeginVertical();

        RenderSettings.ambientLight = EditorGUILayout.ColorField("Ambient Light", RenderSettings.ambientLight);
        RenderSettings.skybox = EditorGUILayout.ObjectField("Skybox Material", RenderSettings.skybox, typeof(Material), true) as Material;
        
        RenderSettings.haloStrength = EditorGUILayout.FloatField("Halo Strength", RenderSettings.haloStrength);
        RenderSettings.flareStrength = EditorGUILayout.FloatField("Flare Strength", RenderSettings.flareStrength);
        RenderSettings.flareFadeSpeed = EditorGUILayout.FloatField("Flare Fade Speed", RenderSettings.flareFadeSpeed);

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void FogSettings()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30.0f);
        GUILayout.BeginVertical();

        RenderSettings.fog = EditorGUILayout.Toggle("Fog", RenderSettings.fog);
        RenderSettings.fogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", RenderSettings.fogMode);
        RenderSettings.fogColor = EditorGUILayout.ColorField("Fog Color", RenderSettings.fogColor);
        RenderSettings.fogDensity = EditorGUILayout.Slider("Fog Density", RenderSettings.fogDensity, 0.0f, 1.0f / Camera.main.farClipPlane * 10.0f);
        RenderSettings.fogStartDistance = EditorGUILayout.FloatField("Fog Start Distance",
            RenderSettings.fogStartDistance);
        RenderSettings.fogEndDistance = EditorGUILayout.FloatField("Fog End Distance",
            RenderSettings.fogEndDistance);

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void CameraSettings()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30.0f);
        GUILayout.BeginVertical();

        var camera = Camera.main;
        if (camera != null)
        {
            camera.fieldOfView = EditorGUILayout.Slider("Field Of View", camera.fieldOfView, 1.0f, 179.0f);

            var rotation = camera.transform.localRotation;
            var eulerAngles = rotation.eulerAngles;
            eulerAngles.x = EditorGUILayout.Slider("Rotation", eulerAngles.x, 0.0f, 90.0f);
            rotation.eulerAngles = eulerAngles;
            camera.transform.localRotation = rotation;
            camera.farClipPlane = EditorGUILayout.Slider("Far", camera.farClipPlane, 0.1f, 2000.0f);
            camera.transform.localPosition = EditorGUILayout.Vector3Field("Position", camera.transform.localPosition);
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }


    private void GameSettings()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30.0f);
        GUILayout.BeginVertical();

        GUI.color = ColorEditor.Title;
        GUILayout.Label("Platform Generator", EditorStyles.boldLabel);
        GUI.color = Color.white;
        var location = GameObject.FindObjectOfType<Runner.LocationManager>(); 
        if (location != null)
        {
            location.SetDisposeDistance = EditorGUILayout.FloatField("Dispose Distance", location.SetDisposeDistance);
            location.SetGenerateDistance = EditorGUILayout.FloatField("Generate Distance", location.SetGenerateDistance);
            location.SetMinDisposeMultiply = EditorGUILayout.FloatField("Min Dispose Multiply", location.SetMinDisposeMultiply);
            location.SetMaxGeneratePlatforms = EditorGUILayout.IntField("Max Generate Platforms", location.SetMaxGeneratePlatforms);
        }
        else
        {
            GUILayout.Label("ERROR, LocationManager not found!!!");
        }

        GUI.color = ColorEditor.Title;
        GUILayout.Label("Player", EditorStyles.boldLabel);
        GUI.color = Color.white;
        var player = GameObject.FindObjectOfType<Runner.PlayerManager>();
        if (player != null)
        {
            player.minimumSpeed = EditorGUILayout.FloatField("Min Speed", player.minimumSpeed);
            player.speedDistanceMultiply = EditorGUILayout.FloatField("Speed Multiply", player.speedDistanceMultiply);
            player.sideScrollSpeed = EditorGUILayout.FloatField("Side Scroll Speed", player.sideScrollSpeed);
        }
        else
        {
            GUILayout.Label("ERROR, PlayerManager not found!!!");
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    [MenuItem("Runner/Game Settings")]
    public static void ShowGameSettingsWindow()
    {
        EditorWindow.GetWindow<GameSettingWindowEditor>("Game Settings", true).Show();
    }
}

