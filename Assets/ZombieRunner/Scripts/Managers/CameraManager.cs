using UnityEngine;
using System.Collections;

namespace Runner
{
    public class CameraManager : ComponentManager
    {
	
		public static Transform CameraTransform {get; private set;}

        public override void Initialize()
        {
            CameraTransform = transform;
        }
	}
}
