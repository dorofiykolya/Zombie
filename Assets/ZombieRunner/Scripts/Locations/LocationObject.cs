using UnityEngine;
using System.Collections;

namespace Runner
{
	public class LocationObject : MonoBehaviour 
	{
		private static bool needInitialize = true;

		private static Shader BASE;
		private static Shader DIFFUSE;
		private static Shader DIFFUSE_REFLECTION;
		private static Shader SPECULAR_REFLECTION;
		private static Shader DIFFUSE_SPECULAR_REFLECTION;
		private static Shader DIFFUSE_WORLDLIGHT;

		private Shader defaultShader = null;
		private bool isBase = false;
		private bool invalidated = true;
		private Material material = null;

		public bool baseShader
		{
			get{ return isBase;}
			set{
				if(value != isBase)
				{
					isBase = value;
					invalidated = true;
				}
			}
		}

		void initialize()
		{
			BASE = Shader.Find ("Curve/Base Curve");
			DIFFUSE = Shader.Find ("Curve/Diffuse Curve");
			DIFFUSE_REFLECTION = Shader.Find ("Curve/Diffuse Curve Reflective");
			SPECULAR_REFLECTION = Shader.Find ("Curve/Specular Curve Reflective");
			DIFFUSE_SPECULAR_REFLECTION = Shader.Find ("Curve/Diffuse Specular Curve Reflective");
			DIFFUSE_WORLDLIGHT = Shader.Find ("Curve/Diffuse Curve WorldLight");
			needInitialize = false;
		}

		void validate()
		{
			if(isBase)
			{
				if(defaultShader == null)
				{
					defaultShader = material.shader;
				}
				material.shader = DIFFUSE;
			}
			else if(defaultShader != null)
			{
				material.shader = defaultShader;
			}

			var components = transform.GetComponentsInChildren<LocationObject> ();
			foreach (var location in components) 
			{
				location.baseShader = isBase;
			}

			invalidated = false;
		}

		void Update()
		{
			if (needInitialize) 
			{
				initialize ();
			}
			if (material == null) 
			{
				material = renderer.material;
			}
			if (invalidated)
			{
				validate();
			}
			if(gameObject.activeSelf)
			{
				if(material != null)
				{
					material.SetVector("_NearCurve", Runner.EffectManager.Near);
					material.SetFloat("_Dist", Runner.EffectManager.Dist);
					material.SetVector("_FarCurve", Runner.EffectManager.Far);
				}
			}
		}
	}
}