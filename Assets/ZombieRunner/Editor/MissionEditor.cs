using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
    [CustomEditor(typeof(MissionManager))]
    [CanEditMultipleObjects]
    class MissionEditor : Editor
    {
        private bool mFoldLast;
        private bool mFoldCurrent;
        private bool mFoldCompleted;

        public override void OnInspectorGUI()
        {
            var manager = GameObject.FindObjectOfType<MissionManager>();
            if(manager == null)
            {
                GUI.color = Color.red;
                GUILayout.Label("ERROR, MissionManager not found!!!");
                GUI.color = Color.white;
                return;  
            }

            GUI.color = Color.green;
            if(GUILayout.Button("EDITOR"))
            {
                EditorWindow.GetWindow<MissionWidowEditor>("Missions", true).Show();
                return;
            }
           /* GUI.color = Color.white;
            manager.Stack = EditorGUILayout.IntSlider("Stack", manager.Stack, 1, 50);

            var queueCount = manager.QueueMissions != null ? manager.QueueMissions.Length : 0;

            EditorGUILayout.LabelField("Missions", queueCount.ToString());

            var completedCount = manager.CompletedMissions != null ? manager.CompletedMissions.Length : 0;
            var currentCount = manager.CurrentMissions != null ? manager.CurrentMissions.Length : 0;
            var lastCount = manager.LastMissions != null ? manager.LastMissions.Length : 0;

            mFoldCompleted = EditorGUILayout.Foldout(mFoldCompleted, "Last:\t\t\t" + completedCount);
            if (mFoldCompleted)
            {
                Inspect(manager.CompletedMissions);
            }
            mFoldCurrent = EditorGUILayout.Foldout(mFoldCurrent, "Current: \t" + currentCount);
            if(mFoldCurrent)
            {
                Inspect(manager.CurrentMissions);
            }
            mFoldLast = EditorGUILayout.Foldout(mFoldLast, "Last:\t\t\t" + lastCount);
            if (mFoldLast)
            {
                Inspect(manager.LastMissions);
            }*/
        }

        private void Inspect(Mission[] list)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();
            if(list == null || list.Length == 0)
            {
                GUILayout.Box("EMPTY", GUILayout.ExpandWidth(true));
            }
            else
            {
                var index = 0;
                foreach(var m in list)
                {
                    if(index == 0)
                    {
                        GUI.color = Color.black;
                        GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true), GUILayout.Height(5.0f));
                        GUI.color = Color.white;
                        
                    }
                    index++;
                    if(index % 2 == 0)
                    {
                        GUI.color = Color.gray;
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }
                    if(m.IsCompleted)
                    {
                        GUI.color = Color.green;
                    }
                    GUILayout.Label("Id:\t\t" + m.Id);
                    GUILayout.Label("Name:\t" + m.Name);
                    GUILayout.Label("Current/Target:\t" + m.Current + "/" + m.Target);
                    GUI.color = Color.black;
                    GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true), GUILayout.Height(5.0f));
                    GUI.color = Color.white;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}
