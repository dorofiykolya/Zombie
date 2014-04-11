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
        public string Id;
        public string Name;
        public string Description;
        public string Image;
        public float Current;
        public float Target;
        public bool IsCompleted;
    }
}
