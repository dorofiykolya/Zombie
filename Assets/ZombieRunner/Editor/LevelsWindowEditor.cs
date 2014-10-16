using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
    public class LevelsWindowEditor : EditorWindow 
    {
        private Vector2 mScrollView = new Vector2();
        private int mIndex;
        private bool mFold;
        private bool mFoldCurrent;
        private bool mFoldCompleted;

        void OnGUI()
        {
            var target = FindObjectOfType<Runner.LevelsManager>();
            if (target == null)
            {
                GUI.color = Color.red;
                GUILayout.Label("ERROR, LevelsManager not found!!!");
                GUI.color = Color.white;
                return;
            }

            EditorGUILayout.BeginHorizontal();
            
            GUI.color = Color.green;
            if (target.Levels == null)
            {
                var list = new List<LevelsManager.Level>();
                list.Add(new LevelsManager.Level());
                target.Levels = list.ToArray();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            
            if (target.Levels != null && target.Levels.Length > 0)
            {
                GUI.color = Color.white;
                Draw(target);
            }
        }
        
        private void Draw(LevelsManager manager)
        {
            GUI.color = Color.white;
            
            mFold = EditorGUILayout.Foldout(mFold, "EDIT LEVELS");
            if (mFold)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30.0f);
                GUILayout.BeginVertical();
                
                mScrollView = GUILayout.BeginScrollView(mScrollView);
                GUILayout.BeginVertical();
                var index = 0;
                foreach (var mission in manager.Levels)
                {
                    var hasBreak = false;
                    GUILayout.BeginHorizontal();
                    if (index % 2 == 0)
                    {
                        GUI.color = Color.cyan;
                    } else
                    {
                        GUI.color = Color.yellow;
                    }
                    GUILayout.Box(index.ToString(), GUILayout.Width(30), GUILayout.Height(95.0f));
                    GUI.color = Color.white;
                    GUILayout.BeginVertical();
                    mission.Id = EditorGUILayout.TextField("Id", mission.Id);
                    mission.Name = EditorGUILayout.TextField("Name", mission.Name);
                    mission.DescriptionRussian = EditorGUILayout.TextField("Description Russian", mission.DescriptionRussian);
                    mission.DescriptionEnglish = EditorGUILayout.TextField("Description English", mission.DescriptionEnglish);
                    mission.Target1 = EditorGUILayout.FloatField("Star 1", mission.Target1);
                    mission.Target2 = EditorGUILayout.FloatField("Star 2", mission.Target2);
                    mission.Target3 = EditorGUILayout.FloatField("Star 3", mission.Target3);
                    EditorGUILayout.LabelField("Current", mission.Current.ToString());
                    
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical(GUILayout.Width(25.0f));
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(25.0f), GUILayout.Height(55.0f)))
                    {
                        var list = manager.Levels.ToList();
                        list.Remove(mission);
                        manager.Levels = list.ToArray();
                        hasBreak = true;
                    }
                    GUI.color = Color.green;
                    
                    if (index != 0 && manager.Levels.Length > 1 && GUILayout.Button("U", GUILayout.Width(25.0f)))
                    {
                        var temp = manager.Levels[index];
                        manager.Levels[index] = manager.Levels[index - 1];
                        manager.Levels[index - 1] = temp;
                        hasBreak = true;
                    }
                    if (index != manager.Levels.Length - 1 && manager.Levels.Length > 1 && GUILayout.Button("D", GUILayout.Width(25.0f)))
                    {
                        var temp = manager.Levels [index];
                        manager.Levels [index] = manager.Levels [index + 1];
                        manager.Levels [index + 1] = temp;
                        hasBreak = true;
                    }
                    GUI.color = Color.white;
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    
                    GUI.color = Color.black;
                    GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true));
                    GUI.color = Color.white;
                    index++;
                    if (hasBreak)
                    {
                        break;
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            AddMission(manager);
        }

        private void AddMission(LevelsManager manager)
        {
            GUI.color = Color.green;
            if (GUILayout.Button("ADD LEVEL"))
            {
                var list = manager.Levels.ToList();
                list.Add(new LevelsManager.Level());
                manager.Levels = list.ToArray();
            }
            GUI.color = Color.white;
        }
        
        [MenuItem("Runner/Levels")]
        public static void ShowGameSettingsWindow()
        {
            EditorWindow.GetWindow<LevelsWindowEditor>("Levels", true).Show();
        }
    }
}
