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

        public Mission Clone()
        {
            var m = new Mission();
            m.Id = Id;
            m.Name = Name;
            m.Description = Description;
            m.Image = Image;
            m.Target = Target;
            return m;
        }

        public void Set(Mission value)
        {
            Id = value.Id;
            Name = value.Name;
            Description = value.Description;
            Image = value.Image;
            Target = value.Target;
        }
    }
}
