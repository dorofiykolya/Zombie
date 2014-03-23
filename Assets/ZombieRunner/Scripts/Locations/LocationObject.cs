using UnityEngine;
using System.Collections;
using System.Linq;

namespace Runner
{
	public class LocationObject : MonoBehaviour 
	{
		private Material material;
        private Shader lastShader;

        [SerializeField]
        public Shader[] shaderList;
        [SerializeField]
        public float[] shaderDistances;

        [SerializeField]
        public bool shaderCulling = true;

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
                        var camera = Camera.current;
                        if (shaderList != null && shaderList.Length > 0 && camera != null)
                        {
                            var distance = transform.position.Distance(camera.transform.position);
                            Shader current = null;
                            var i = 0;
                            var len = shaderList.Length;
                            for (; i < len; i++)
                            {
                                var s = shaderList[i];
                                var d = shaderDistances[i];
                                if (distance >= d)
                                {
                                    current = s;
                                    break;
                                }
                            }
                            if (current == null)
                            {
                                current = shaderList[shaderList.Length - 1];
                            }
                            if (lastShader != current)
                            {
                                lastShader = current;
                                if (current != null)
                                {
                                    material.shader = current;
                                }
                            }
                        }
                    }

					material.SetVector("_NearCurve", Runner.EffectManager.Near);
					material.SetFloat("_Dist", Runner.EffectManager.Dist);
					material.SetVector("_FarCurve", Runner.EffectManager.Far);
				}
			}
		}
	}
}