using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Runner
{
    public class MissionWidowEditor : EditorWindow
    {
        [MenuItem("Runner/Missions")]
        public static void ShowGameSettingsWindow()
        {
            EditorWindow.GetWindow<MissionWidowEditor>("Missions", true).Show();
        }
    }
}
