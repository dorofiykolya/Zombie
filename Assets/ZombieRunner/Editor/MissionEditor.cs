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
        public override void OnInspectorGUI()
        {
            var mission = GameObject.FindObjectOfType<MissionManager>();
            if(mission == null)
            {
                GUI.color = Color.red;
                GUILayout.Label("ERROR, MissionManager not found!!!");
                GUI.color = Color.white;
                return;  
            }
            /*
            var list = mission.Missions;
            GUI.color = ColorEditor.Title;
            GUILayout.Label("Missions:");
            GUI.color = Color.white;
            bool color = false;
            foreach(var currentMission in list)
            {
                color = !color;
                EditorGUILayout.Separator();
                GUILayout.BeginHorizontal();
                GUI.color = color ? Color.white : Color.grey;
                GUI.contentColor = color ? Color.white : Color.cyan;
                GUILayout.Label("Mission:");
                if(GUILayout.Button("X"))
                {
                    mission.Remove(currentMission.Id);
                    return;
                }
                GUILayout.EndHorizontal();

                var lastID = currentMission.Id;
                var id = EditorGUILayout.TextField("id:", currentMission.Id);
                if(mission[id] == null && id != lastID)
                {
                    mission.Remove(currentMission.Id);
                    currentMission.Id = id;
                    mission[id] = currentMission;
                }
                currentMission.Name = EditorGUILayout.TextField("name:", currentMission.Name);
                currentMission.Description = EditorGUILayout.TextField("description:", currentMission.Description);
                currentMission.ResetOnCompleted = EditorGUILayout.Toggle("resetOnCompleted:", currentMission.ResetOnCompleted);

                GUILayout.BeginHorizontal();
                GUILayout.Space(15.0f);
                InspectMissionLevel(currentMission);
                GUILayout.EndHorizontal();
            }

            GUI.color = Color.cyan;

            if (mission["empty"] == null)
            {
                if (GUILayout.Button("Add Mission"))
                {
                    var m = new Mission();
                    m.Id = "empty";
                    m.Name = "empty";
                    m.Description = "empty";
                    mission[m.Id] = m;
                }
            }*/
        }
        /*
        private void InspectMissionLevel(Mission mission)
        {
            GUILayout.BeginVertical();
            var levels = mission.Levels;
            MissionLevel current;
            for (var i = 0; i < levels.Length; i++ )
            {
                if(i != 0)
                {
                    EditorGUILayout.Separator();
                }
                current = levels[i];

                GUILayout.BeginHorizontal();
                GUILayout.Label("Level");
                if(GUILayout.Button("X"))
                {
                    mission.Remove(current);
                    return;
                }
                GUILayout.EndHorizontal();

                current.target = EditorGUILayout.IntField("target:", current.target);
                current.name = EditorGUILayout.TextField("name:", current.name);
                current.description = EditorGUILayout.TextField("description:", current.description);
            }
            if (GUILayout.Button("Add Level"))
            {
                mission.Add(new MissionLevel());
            }
            GUILayout.EndVertical();
        }*/
    }
}
