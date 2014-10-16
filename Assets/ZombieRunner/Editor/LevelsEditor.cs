using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
    [CustomEditor(typeof(LevelsManager))]
    [CanEditMultipleObjects]
    public class LevelsEditor : Editor 
    {
        private bool mFoldCurrent;
        private bool mFoldCompleted;
        
        public override void OnInspectorGUI()
        {
            var manager = GameObject.FindObjectOfType<LevelsManager>();
            if(manager == null)
            {
                GUI.color = Color.red;
                GUILayout.Label("ERROR, LevelsManager not found!!!");
                GUI.color = Color.white;
                return;  
            }
            
            GUI.color = Color.green;
            if (GUILayout.Button("EDITOR"))
            {
                EditorWindow.GetWindow<LevelsWindowEditor>("Levels", true).Show();
                return;
            }
        }
    }
}
