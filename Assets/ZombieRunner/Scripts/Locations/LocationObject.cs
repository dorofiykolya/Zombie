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

		public override void Initialize ()
		{
			mTransform = transform;
		}

		void Update()
		{
			if (material == null) 
			{
				material = renderer.material;
                if (material != null)
                {
                    lastShader = material.shader;
                }
			}
			if(gameObject.activeSelf)
			{
				if(material != null)
				{
                    if (shaderCulling)
                    {
                        var currentQuality = QualityManager.CurrentQuality;
                        var camera = Camera.current;
                        if (shaderList != null && shaderList.Length > 0 && camera != null)
                        {
							var distance = DistanceToCamera(camera);//mTransform.position.Distance(camera.transform.position);
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

		protected virtual float DistanceToCamera(Camera camera)
		{
			return transform.position.Distance(camera.transform.position);
		}
	}
}