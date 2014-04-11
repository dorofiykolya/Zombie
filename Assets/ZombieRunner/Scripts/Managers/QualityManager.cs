using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runner
{
    public class QualityManager : ComponentManager
    {
        private static int currentQuality = -1;

        public static int CurrentQuality
        {
            get
            {
                if (currentQuality == -1)
                {
                    currentQuality = UnityEngine.QualitySettings.GetQualityLevel();
                }
                return currentQuality;
            }
        }

        void Update()
        {
            currentQuality = UnityEngine.QualitySettings.GetQualityLevel();
        }
    }
}
