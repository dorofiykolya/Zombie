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
        public ShaderInfo[] shaders;
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
                        if (shaders != null && shaders.Length > 0 && camera != null)
                        {
                            var distance = transform.position.Distance(camera.transform.position);
                            Shader current = null;
                            foreach (var shaderInfo in shaders)
                            {
                                if (distance >= shaderInfo.Distance)
                                {
                                    current = shaderInfo.Shader;
                                    break;
                                }
                            }
                            if (current == null)
                            {
                                current = shaders[shaders.Length - 1].Shader;
                            }
                            if (lastShader != current)
                            {
                                lastShader = current;
                            }
                            material.shader = current;
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