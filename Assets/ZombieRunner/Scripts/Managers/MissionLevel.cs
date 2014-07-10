using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runner
{
    [Serializable]
    public class MissionLevel
    {
        public int target;
        public bool completed;
        public int level;
        public string name;
        public string description;
    }
}
