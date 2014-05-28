using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner 
{
    [Serializable]
    public class MissionManager : ComponentManager
    {
        public Mission[] QueueMissions;
        public Mission[] CurrentMissions;
        public Mission[] CompletedMissions;
        public Mission[] LastMissions;

        public int Stack = 3;

        public event Action<Mission[], MissionManager> OnComplete;

        public void Dispatch(string id, float value)
        {
            List<Mission> missions = null;
            foreach (var mission in CurrentMissions)
            {
                if (mission.Id == id)
                {
                    mission.Current = value;
                    if (mission.IsCompleted == false && mission.Current >= mission.Target)
                    {
                        mission.IsCompleted = true;
                        if (missions == null)
                        {
                            missions = new List<Mission>(1) {mission};
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
                foreach (var m in missions)
                {
                    tempList.Add(m);
                }
                LastMissions = tempList.ToArray();
                if (OnComplete != null)
                {
                    OnComplete.Invoke(missions.ToArray(), this);
                }
            }
        }

        public override void Initialize()
        {
            LastMissions = new Mission[0];
            ClearStack();
        }

        public override void GameStop()
        {
            ClearStack();
        }

        private void ClearStack()
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
            Mission current;
            for (var i = 0; i < len; i++)
            {
                current = CurrentMissions[i];
                if (current.IsCompleted == false)
                {
                    CurrentMissions[index] = current;
                    index++;
                }
            }
			if (CurrentMissions.Length < Stack) 
			{
				var temp = CurrentMissions.ToList();
				temp.Capacity = Stack;
				CurrentMissions = temp.ToArray();
			}
            var tempQueue = QueueMissions.ToList();
            while (index <= Stack && tempQueue.Count > 0)
            {
                CurrentMissions[index] = tempQueue[0];
                tempQueue.RemoveAt(0);
                index++;
            }
            QueueMissions = tempQueue.ToArray();
            if (CurrentMissions.Length > index)
            {
                var temp = CurrentMissions.ToList();
                temp.Capacity = index;
                CurrentMissions = temp.ToArray();
            }
        }

        public void Load(Mission[] missions)
        {

        }
    }
}
