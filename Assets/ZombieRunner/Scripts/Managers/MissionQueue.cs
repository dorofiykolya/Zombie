using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner
{
    [Serializable]
    public class MissionQueue
    {
        public Mission[] QueueMissions;
        public Mission[] CurrentMissions;
        public Mission[] CompletedMissions;
        public Mission[] LastMissions;

        public int Stack = 3;

        public MissionQueue()
        {
            QueueMissions = new Mission[0];
            CurrentMissions = new Mission[0];
            CompletedMissions = new Mission[0];
            LastMissions = new Mission[0];
        }

        public IEnumerable<Mission> Dispatch(string id, float value)
        {
            List<Mission> missions = null;
            foreach (var mission in CurrentMissions)
            {
                if (mission.Id == id)
                {
					if(id.Contains("run"))
					{
						mission.Current = value;
					}
					else
					{
						mission.Current += value;
					}
                    
                    if (mission.IsCompleted == false && mission.Current >= mission.Target)
                    {
                        mission.IsCompleted = true;
                        if (missions == null)
                        {
                            missions = new List<Mission>(1) { mission };
                        }
                        else
                        {
                            missions.Add(mission);
                        }
                    }
                }
            }
            if (missions != null)
            {
                var tempList = new List<Mission>(LastMissions);
                tempList.AddRange(missions);
                LastMissions = tempList.ToArray();
            }
            return missions;
        }

        internal void ClearStack()
        {
            var tempCompleted = new List<Mission>(CompletedMissions);
            foreach (var m in CurrentMissions)
            {
                if (m.IsCompleted)
                {
                    tempCompleted.Add(m);
                }
            }
            CompletedMissions = tempCompleted.ToArray();

            var index = 0;
            var len = CurrentMissions.Length;
            for (var i = 0; i < len; i++)
            {
                Mission current = CurrentMissions[i];
                if (current.IsCompleted == false)
                {
                    CurrentMissions[index] = current;
                    index++;
                }
            }
            if (CurrentMissions.Length < Stack)
            {
				Array.Resize(ref CurrentMissions, Stack);
            }
            var tempQueue = QueueMissions.ToList();

            while (index < Stack && tempQueue.Count > 0)
            {
                CurrentMissions[index] = tempQueue[0];
                tempQueue.RemoveAt(0);
                index++;
            }
            QueueMissions = tempQueue.ToArray();
            if (CurrentMissions.Length > index)
            {
				Array.Resize(ref CurrentMissions, index);
            }
        }
    }
}
