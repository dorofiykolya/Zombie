using UnityEngine;
using System.Collections;

namespace Runner
{
    public class SettingManager : ScriptableObject
    {
        public const string PrefabExtension = ".prefab";

        public string PlatformPrefabPathSave = "Assets/ZombieRunner/Prefabs/";
        public Vector3 PlatformPrefabLocalRotation = new Vector3(0.0f, 180.0f, 0.0f);
    }
}