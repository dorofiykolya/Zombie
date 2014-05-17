using UnityEngine;
using System.Collections;
using System.Linq;

namespace Runner
{
    public class LocationObject : ComponentManager 
	{

		private Material material;
        private Shader lastShader;

        [SerializeField]
        public Shader[] shaderList;
        [SerializeField]
        public float[] shaderDistances;
        [SerializeField]
        public int[] shaderQualities;

        [SerializeField]
        public bool shaderCulling = true;

		protected Transform mTransform;
        protected GameObject mGameObject;
        protected Camera mLastCamera;
        protected Transform mCameraTransform;
        protected Renderer mRenderer;

		public override void Initialize ()
		{
			mTransform = transform;
		    mGameObject = gameObject;
		    mRenderer = renderer;
		}

		void Update()
		{
            if(mInitialized == false) return;

			if (material == null) 
			{
                material = mRenderer.sharedMaterial;
                if (material != null)
                {
                    lastShader = material.shader;
                }
			}
            if (mGameObject.activeSelf)
			{
				if(material != null)
				{
                    if (shaderCulling)
                    {
                        
                        var currentCamera = Camera.current;
                        if (currentCamera == null) return;
                        if (mLastCamera != currentCamera)
                        {
                            mLastCamera = currentCamera;
                            mCameraTransform = mLastCamera.transform;
                        }
                        if (shaderList != null && shaderList.Length > 0)
                        {
                            var currentQuality = QualityManager.CurrentQuality;
                            var distance = DistanceTo(mCameraTransform);//mTransform.position.Distance(camera.transform.position);
                            Shader selectedShader = null;
                            int selectedQuality = 0;
                            float selectedDistance = 0.0f;
                            var i = 0;
                            var len = shaderList.Length;
                            for (; i < len; i++)
                            {
                                //if(name == "probirka")
                                //{
                                //    selectedShader = selectedShader;
                                //}
                                var s = shaderList[i];
                                var d = shaderDistances[i];
                                var q = shaderQualities[i];
                                if (selectedShader == null || q <= currentQuality)
                                {
									if (selectedShader == null || q >= selectedQuality && ((distance >= d) || selectedDistance >= d))
                                    {
                                        selectedShader = s;
                                        selectedQuality = q;
                                        selectedDistance = d;
                                    }
                                    //break;
                                }
                            }
                            if (selectedShader != null && lastShader != selectedShader)
                            {
                                lastShader = selectedShader;
                                material.shader = selectedShader;
                            }
                        }
                    }

					material.SetVector("_NearCurve", Runner.EffectManager.Near);
					material.SetFloat("_Dist", Runner.EffectManager.Dist);
					material.SetVector("_FarCurve", Runner.EffectManager.Far);
				}
			}
		}

		void OnBecameInvisible() 
		{
			enabled = false;
		}

		protected virtual float DistanceTo(Transform aTransform)
		{
            return mTransform.position.Distance(aTransform.position);
		}
	}
}