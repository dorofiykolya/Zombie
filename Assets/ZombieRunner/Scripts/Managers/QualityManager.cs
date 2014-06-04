using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner
{
    public class QualityManager : ComponentManager
    {
        private static int currentQuality = -1;

		public override void Initialize ()
		{
			#if UNITY_IPHONE    
			switch (iPhoneSettings.generation)
			{
			case iPhoneGeneration.iPad2Gen:
			case iPhoneGeneration.iPhone4:
			case iPhoneGeneration.iPhone4S:
			case iPhoneGeneration.iPodTouch4Gen:
				QualitySettings.SetQualityLevel(2, true);
				break;
			}
			#endif
		}

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
