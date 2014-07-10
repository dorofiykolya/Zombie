using UnityEngine;
using System.Collections;

namespace Runner
{
    public class EffectManager : ComponentManager
    {

        public Material[] materials;
		public Matrix4x4 projectionMatrix;
		public bool useMatrix = true;
		
		public bool SetDynamic;
		public float SetDistanceDelim = 1000f;
		public Vector2 SetDynamicFarMultiply = new Vector2(50, -50);
		//public Vector2 SetDynamicNearMultiply = new Vector2(5000,5000);
		
		public float SetDist = 0.1f;
		public Vector4 SetNear = new Vector4(5000,5000,5000);
		public Vector4 SetFar = Vector4.zero;
		
		private Camera currentCamera;
		
		public static float Dist{get;private set;}
		public static Vector4 Near{get;private set;}
		public static Vector4 Far{get;private set;}
		
		public static bool Dynamic{get;private set;}

		float deltaTime = 0.0f;
		float fps = 0.0f;
		
        public override void Initialize()
        {
            Dist = SetDist;
            Near = SetNear;
            Far = SetFar;
            Dynamic = SetDynamic;
            currentCamera = Camera.main;
            if (currentCamera != null)
            {
                projectionMatrix = currentCamera.projectionMatrix;
            }
        }

		void OnGUI()
		{
			GUI.color = Color.black;
			GUI.Label (new Rect (Screen.width / 2, 0, 100, 50), fps.ToString() + " " + UnityEngine.QualitySettings.GetQualityLevel());
		}
		
		// Update is called once per frame
		void Update ()
		{
			deltaTime += Time.deltaTime;
			deltaTime /= 2.0f;
			fps = 1.0f/deltaTime;

			if(SetDynamic)
			{
				//SetNear.Set(Mathf.Abs(Mathf.Sin(PlayerManager.Distance / SetDistanceDelim) * SetDynamicNearMultiply.x), Mathf.Abs(Mathf.Sin(PlayerManager.Distance / SetDistanceDelim)) * SetDynamicNearMultiply.y, 0 ,0);
				SetFar.Set(Mathf.Sin(Player.Distance / SetDistanceDelim) * SetDynamicFarMultiply.x, Mathf.Sin(Player.Distance / SetDistanceDelim) * SetDynamicFarMultiply.y, 0 ,0);
			}
			
			Dist = SetDist;
			Near = SetNear;
			Far = SetFar;
			Dynamic = SetDynamic;


            foreach (var material in materials)
            {
                if (material != null)
                {
                    material.SetVector("_NearCurve", SetNear);
                    material.SetFloat("_Dist", SetDist);
                    material.SetVector("_FarCurve", SetFar);   
                }
            }
			
			if(ErrorManager.HasError)
			{
				return;
			}
			
			if(currentCamera != null && useMatrix)
			{
				//matrix.m32 = Mathf.Sin((float)Time.frameCount / 30.0f);
				projectionMatrix.m10 = Mathf.Sin((float)Time.frameCount / 25.0f) / 3;
				currentCamera.projectionMatrix = projectionMatrix;	
			}
		}
	}
}

