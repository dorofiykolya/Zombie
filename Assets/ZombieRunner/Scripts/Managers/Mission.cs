using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner
{
    [Serializable]
    public class Mission
    {
		[SerializeField]
        public MissionLevel[] Levels = new MissionLevel[0];

        public event Action<Mission, MissionLevel[]> OnCompleted;

        public string Id;
        public string Name;
        public string Description;
        public int Current;
        public bool ResetOnCompleted;

        public MissionLevel[] Update(int current)
        {
            var result = new List<MissionLevel>();
            foreach (var level in Levels)
            {
                if(level.completed == false && level.target <= current)
                {
                    if(ResetOnCompleted)
                    {
                        current -= level.target;
                    }
                    level.completed = true;
                    result.Add(level);
                }
            }
            this.Current = current;
            var array = result.ToArray();
            if(array.Length > 0)
            {
                OnCompleted.Invoke(this, array);
            }
            return array;
        } 

        public void Add(MissionLevel level)
        {
            var levelList = Levels.ToList();
            level.level = levelList.Count;
            levelList.Add(level);
            Levels = levelList.ToArray();
        }

        public void Remove(MissionLevel level)
        {
            var levelList = Levels.ToList();
            levelList.Remove(level);
            Levels = levelList.ToArray();
            var index = 0;
            foreach(var l in Levels)
            {
                l.level = index;
                index++;
            }
        }

        public Mission Clone()
        {
            var m = new Mission();
            m.Id = Id;
            m.Levels = Levels;
            m.Name = Name;
            m.Current = Current;
            m.Description = Description;
            return m;
        }
    }
}
