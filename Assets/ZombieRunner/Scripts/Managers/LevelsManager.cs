using UnityEngine;
using System;
using System.Collections;

namespace Runner
{
    public class LevelsManager : MonoBehaviour 
    {
        [Serializable]
        public class Level
        {
            public string Id;
            public string Name;
            public string DescriptionRussian;
            public string DescriptionEnglish;
            public float Current;
            public float Target1;
            public float Target2;
            public float Target3;
            public bool IsCompleted;
        }

        public Level[] Levels;
    }
}
