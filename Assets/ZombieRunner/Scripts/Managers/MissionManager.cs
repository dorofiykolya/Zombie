using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner 
{
    [Serializable]
    public class MissionManager : MonoBehaviour
    {
        private Dictionary<string, Mission> missions = new Dictionary<string, Mission>();
		[SerializeField]
        private Mission[] missionList;
		[SerializeField]
        private bool invalidated;

        void Awake()
        {
            if(missionList != null)
            {
                missions.Clear();
                foreach(var m in missionList)
                {
                    missions.Add(m.Id, m);
                }
            }
        }

        public Mission this[string id]
        {
            get
            {
                Mission result = null;
                missions.TryGetValue(id, out result);
                return result;
            }
            set
            {
                if (missions.ContainsKey(id))
                    throw new ArgumentException("missions.ContainsKey(id) id:" + id);

                missions.Add(id, value);
                invalidated = true;
            }
        }

        public Mission[] Missions
        {
            get
            {
                validate();
                return missionList;
            }
        }

        private void validate()
        {
            if(invalidated || missionList == null)
            {
                missionList = missions.Values.ToArray();
                invalidated = false;
            }
        }

        public void Remove(string Id)
        {
            missions.Remove(Id);
            invalidated = true;
        }

        public void Load(Mission[] missions)
        {
            if (missions != null)
            {
                foreach (var mission in missions)
                {
                    var thisMission = this[mission.Id];
                    thisMission.Levels = mission.Levels;
                    thisMission.Current = mission.Current;
                    thisMission.Description = mission.Description;
                    thisMission.Name = mission.Name;
                }
            }
            invalidated = true;
        }
    }
}
