using UnityEngine;
using System.Collections;

namespace Runner
{
	[ExecuteInEditMode]
    public class CameraManager : ComponentManager
    {
	
		public static Transform CameraTransform {get; private set;}

        public override void Initialize()
        {
            CameraTransform = transform;
        }
	}
}
