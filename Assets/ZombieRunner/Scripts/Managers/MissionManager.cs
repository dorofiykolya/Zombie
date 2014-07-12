using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner 
{
    [Serializable]
    public class MissionManager : ComponentManager
    {
        public MissionQueue[] MissionQueues;
		public MissionQueue[] MissionPlayerQueues;
		public GameObject[] visualMissions = new GameObject[3];
		public GameObject[] progressMissions = new GameObject[3];

        public MissionManager()
        {
			MissionPlayerQueues = new MissionQueue[0];
        }

        public void Dispatch(string id, float value)
        {
            List<Mission> tempMissions = null;
			foreach (var queue in MissionPlayerQueues)
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
            if (tempMissions != null && tempMissions.Count > 0)
            {
				PlayerData.missionMulti += tempMissions.Count;
				for(int i = 0; i < progressMissions.Length; i++)
				{
					progressMissions[i].SetActive(false);
				}
				progressMissions[PlayerData.missionMulti % 3].SetActive(true);

				StorageManager.Save();
            }
        }

        public override void Initialize()
        {
            ClearLastMissions();
            ClearStack();

			for(int i = 0; i < progressMissions.Length; i++)
			{
				progressMissions[i].SetActive(false);
			}
			progressMissions[PlayerData.missionMulti % 3].SetActive(true);
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
			foreach (var queue in MissionPlayerQueues)
            {
                queue.LastMissions = new Mission[0];
            }
        }

        private void ClearStack()
        {
			int index = 0;
			foreach (var queue in MissionPlayerQueues)
            {
                CheckCurrentProgress(queue);

				if (queue.CurrentMissions.Length == 0)
				{
					visualMissions[index].transform.FindChild("Label").GetComponent<UILabel>().text = "";
					visualMissions[index].transform.FindChild("Button").GetComponent<MissionAction>().mission = null;
				}
				else
				{
					visualMissions[index].transform.FindChild("Label").GetComponent<UILabel>().text = queue.CurrentMissions[0].Description + " " + queue.CurrentMissions[0].Current + "/" + queue.CurrentMissions[0].Target;
					visualMissions[index].transform.FindChild("Button").GetComponent<MissionAction>().mission = queue.CurrentMissions[0];
				}

				index++;
            }
        }

        private void CheckCurrentProgress(MissionQueue queue)
        {
			queue.ClearStack();

			if (queue.CurrentMissions.Length == 0)
				return;

            switch(queue.CurrentMissions[0].Id)
            {
                case "unlocksgtwall":
                    if (PlayerManager.levels[4] > 0)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "unlockdrwhite":
                    if (PlayerManager.levels[3] > 0)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "unlockbobby":
                    if (PlayerManager.levels[2] > 0)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "upgradeandy":
                    if (PlayerManager.levels[0] >= queue.CurrentMissions[0].Target)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "upgradejessy":
                    if (PlayerManager.levels[1] >= queue.CurrentMissions[0].Target)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "upgradebobby":
                    if (PlayerManager.levels[2] >= queue.CurrentMissions[0].Target)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "upgradedrwhite":
                    if (PlayerManager.levels[3] >= queue.CurrentMissions[0].Target)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
                case "upgradesgtwall":
                    if (PlayerManager.levels[4] >= queue.CurrentMissions[0].Target)
                        queue.CurrentMissions[0].IsCompleted = true;
                    break;
            }

            queue.ClearStack();
        }

        public void Load(MissionQueue[] missions)
        {
            if(missions == null) 
				missions = MissionQueues;
			MissionPlayerQueues = missions;
        }
    }
}
