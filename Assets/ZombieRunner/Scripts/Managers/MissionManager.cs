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

        }

        public override void Initialize()
        {

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
			
        }

        private void ClearStack()
        {
			
        }

        private void CheckCurrentProgress(MissionQueue queue)
        {
		
        }

        public void Load(MissionQueue[] missions)
        {

        }
    }
}
