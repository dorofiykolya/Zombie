using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner 
{
    [Serializable]
    public class MissionManager : ComponentManager
    {
        public MissionQueue[] MissionQueues;
		public GameObject[] visualMissions = new GameObject[3];

        public event Action<Mission[], MissionManager> OnComplete;

        public MissionManager()
        {
            MissionQueues = new MissionQueue[0];
        }

        public void Dispatch(string id, float value)
        {
            List<Mission> tempMissions = null;
            foreach (var queue in MissionQueues)
            {
                var result = queue.Dispatch(id, value);
                if (result != null)
                {
                    if (tempMissions == null)
                    {
                        tempMissions = new List<Mission>(result);
                    }
                    else
                    {
                        tempMissions.AddRange(result);
                    }
                }
            }
            if (OnComplete != null && tempMissions != null && tempMissions.Count > 0)
            {
                OnComplete.Invoke(tempMissions.ToArray(), this);
            }
        }

        public override void Initialize()
        {
            ClearLastMissions();
            ClearStack();
        }

        public override void GameStop()
        {
            ClearStack();
        }

		public override void GamePause ()
		{
			ClearStack ();
		}

        private void ClearLastMissions()
        {
            foreach (var queue in MissionQueues)
            {
                queue.LastMissions = new Mission[0];
            }
        }

        private void ClearStack()
        {
			int index = 0;
            foreach (var queue in MissionQueues)
            {
                queue.ClearStack();
				visualMissions[index].transform.FindChild("Label").GetComponent<UILabel>().text = queue.CurrentMissions[0].Description + " " + queue.CurrentMissions[0].Current + "/" + queue.CurrentMissions[0].Target;
				index++;
            }
        }

        public void Load(MissionQueue[] missions)
        {
            if(missions == null) return;
            MissionQueues = missions;
        }
    }
}
