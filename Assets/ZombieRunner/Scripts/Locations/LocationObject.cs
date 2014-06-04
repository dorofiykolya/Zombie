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

		void OnBecameVisible() 
		{
			enabled = true;
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