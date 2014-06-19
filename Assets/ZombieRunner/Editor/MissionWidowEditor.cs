using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
    public class MissionWidowEditor : EditorWindow
    {
        private Vector2 mScrollView = new Vector2();
        private static Mission copyMission;
        private int mIndex;
        private bool mFold;
        private bool mFoldCurrent;
        private bool mFoldCompleted;
        void OnGUI()
        {
            var target = FindObjectOfType<Runner.MissionManager>();
            if (target == null)
            {
                GUI.color = Color.red;
                GUILayout.Label("ERROR, MissionManager not found!!!");
                GUI.color = Color.white;
                return;
            }

            EditorGUILayout.Separator();

			for(int i = 0; i < target.visualMissions.Length; i++)
			{
				target.visualMissions[i] = EditorGUILayout.ObjectField("Visual Mission:", target.visualMissions[i], typeof(GameObject)) as GameObject;
			}

			for(int i = 0; i < target.progressMissions.Length; i++)
			{
				target.progressMissions[i] = EditorGUILayout.ObjectField("Progress Mission:", target.progressMissions[i], typeof(GameObject)) as GameObject;
			}

            EditorGUILayout.BeginHorizontal();
            
            GUI.color = Color.green;
            if (GUILayout.Button("ADD QUEUE"))
            {
                var list = target.MissionQueues != null? target.MissionQueues.ToList() : new List<MissionQueue>();
                list.Add(new MissionQueue());
                target.MissionQueues = list.ToArray();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            
            if (target.MissionQueues != null && target.MissionQueues.Length > 0)
            {
                var toolList = new string[target.MissionQueues.Length];
                for (int i = 0; i < target.MissionQueues.Length; i++)
                {
                    toolList[i] = i.ToString();
                }
                if (mIndex >= toolList.Length)
                {
                    mIndex = toolList.Length - 1;
                }
                GUI.color = Color.green;
                mIndex = GUILayout.Toolbar(mIndex, toolList);
                GUI.color = Color.white;
                Draw(mIndex, target.MissionQueues[mIndex], target);
            }
        }

        private void Draw(int indexQueue, MissionQueue target, MissionManager manager)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("QUEUE:", indexQueue.ToString(), GUILayout.ExpandWidth(true));
            EditorGUILayout.Separator();
            GUI.color = Color.red;
            if (GUILayout.Button("REMOVE QUEUE"))
            {
                var tempList = manager.MissionQueues.ToList();
                tempList.Remove(target);
                manager.MissionQueues = tempList.ToArray();
                return;
            }
            GUI.color = Color.white;

            target.Stack = EditorGUILayout.IntSlider("Stack", target.Stack, 1, 50);

            var completedCount = target.CompletedMissions != null ? target.CompletedMissions.Length : 0;
            var currentCount = target.CurrentMissions != null ? target.CurrentMissions.Length : 0;

            mFoldCompleted = EditorGUILayout.Foldout(mFoldCompleted, "Last:\t\t\t" + completedCount);
            if (mFoldCompleted)
            {
                Inspect(target.CompletedMissions);
            }
            mFoldCurrent = EditorGUILayout.Foldout(mFoldCurrent, "Current: \t" + currentCount);
            if (mFoldCurrent)
            {
                Inspect(target.CurrentMissions);
            }

            EditorGUILayout.Separator();

            mFold = EditorGUILayout.Foldout(mFold, "EDIT QUEUE");
            if (mFold)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30.0f);
                GUILayout.BeginVertical();

                mScrollView = GUILayout.BeginScrollView(mScrollView);
                GUILayout.BeginVertical();
                var index = 0;
                foreach (var mission in target.QueueMissions)
                {
                    var hasBreak = false;
                    GUILayout.BeginHorizontal();
                    if (index % 2 == 0)
                    {
                        GUI.color = Color.cyan;
                    }
                    else
                    {
                        GUI.color = Color.yellow;
                    }
                    GUILayout.Box(index.ToString(), GUILayout.Width(30), GUILayout.Height(95.0f));
                    GUI.color = Color.white;
                    GUILayout.BeginVertical();
                    if (mission.IsCompleted)
                    {
                        GUI.contentColor = Color.green;
                    }
                    mission.Id = EditorGUILayout.TextField("Id", mission.Id);
                    mission.Name = EditorGUILayout.TextField("Name", mission.Name);
                    mission.Description = EditorGUILayout.TextField("Description", mission.Description);
                    mission.Image = EditorGUILayout.TextField("Image", mission.Image);
                    mission.Target = EditorGUILayout.FloatField("Target", mission.Target);
                    EditorGUILayout.LabelField("Current", mission.Current.ToString());

                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(25.0f));
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(25.0f), GUILayout.Height(55.0f)))
                    {
                        var list = target.QueueMissions.ToList();
                        list.Remove(mission);
                        target.QueueMissions = list.ToArray();
                        hasBreak = true;
                    }
                    GUI.color = Color.green;

                    if (index != 0 && target.QueueMissions.Length > 1 && GUILayout.Button("U", GUILayout.Width(25.0f)))
                    {
                        var temp = target.QueueMissions[index];
                        target.QueueMissions[index] = target.QueueMissions[index - 1];
                        target.QueueMissions[index - 1] = temp;
                        hasBreak = true;
                    }
                    if (index != target.QueueMissions.Length - 1 && target.QueueMissions.Length > 1 && GUILayout.Button("D", GUILayout.Width(25.0f)))
                    {
                        var temp = target.QueueMissions[index];
                        target.QueueMissions[index] = target.QueueMissions[index + 1];
                        target.QueueMissions[index + 1] = temp;
                        hasBreak = true;
                    }
                    GUI.color = Color.white;
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("COPY", GUILayout.Width(50.0f)))
                    {
                        copyMission = mission.Clone();
                    }
                    if (GUILayout.Button("CLONE", GUILayout.Width(50.0f)))
                    {
                        var list = target.QueueMissions.ToList();
                        list.Insert(index + 1, mission.Clone());
                        target.QueueMissions = list.ToArray();
                        hasBreak = true;
                    }
                    if (GUILayout.Button("CLONE TO END", GUILayout.Width(100.0f)))
                    {
                        var list = target.QueueMissions.ToList();
                        list.Add(mission.Clone());
                        target.QueueMissions = list.ToArray();
                        hasBreak = true;
                    }
                    if (copyMission != null)
                    {
                        if (GUILayout.Button("CLEAR BUFFER", GUILayout.Width(100.0f)))
                        {
                            copyMission = null;
                            hasBreak = true;
                        }
                        if (copyMission != null && GUILayout.Button("PASTE: " + copyMission.Id))
                        {
                            mission.Set(copyMission);
                            hasBreak = true;
                        }
                    }
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
            AddMission(target);
        }

        private void Inspect(Mission[] list)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            GUILayout.BeginVertical();
            if (list == null || list.Length == 0)
            {
                GUILayout.Box("EMPTY", GUILayout.ExpandWidth(true));
            }
            else
            {
                var index = 0;
                foreach (var m in list)
                {
                    if (index == 0)
                    {
                        GUI.color = Color.black;
                        GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true), GUILayout.Height(5.0f));
                        GUI.color = Color.white;

                    }
                    index++;
                    if (index % 2 == 0)
                    {
                        GUI.color = Color.gray;
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }
                    if (m.IsCompleted)
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

        private void AddMission(MissionQueue manager)
        {
            GUI.color = Color.green;
            if (GUILayout.Button("ADD MISSION"))
            {
                var list = manager.QueueMissions.ToList();
                list.Add(new Mission());
                manager.QueueMissions = list.ToArray();
            }
            GUI.color = Color.white;
        }

        [MenuItem("Runner/Missions")]
        public static void ShowGameSettingsWindow()
        {
            EditorWindow.GetWindow<MissionWidowEditor>("Missions", true).Show();
        }
    }
}
