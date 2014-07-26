using UnityEngine;
using System.Collections;
using Runner;

namespace Runner
{
	[AddComponentMenu("Runner/Platform")]
	public class PlatformObject : LocationObject
	{
	    public PlatformMode Mode;
		public int Level = 0;
		public int MinimumDistance;
		public int Type;
	    public int TypeTo = 0;
		public bool InPlatformList;
		public int Id {get;set;}
		
		public Runner.PlatformObject[] NextPlatforms;
		public Vector3 Size{get;set;}
		public bool AllowDispose{get;set;}
		public bool IsStartPlatform{get;set;}
		
		public PlatformObject GetNextRandom ()
		{
			if(NextPlatforms == null || NextPlatforms.Length == 0)
			{
				return null;
			}
			return Location.PlatformsManager.PopPlatform(NextPlatforms[Random.Range(0, NextPlatforms.Length)]);
		}
		
		public void SetStartTransform()
		{
            if (mTransform == null)
            {
                mTransform = transform;
            }
            mTransform.position = Vector3.zero;
		}
		
		public void Move(Vector3 move)
		{
            if (mTransform == null)
            {
                mTransform = transform;
            }
            mTransform.position += move;	
		}
		
		public float Distance(Runner.PlayerController player)
		{
		    if (mTransform == null)
		    {
                mTransform = transform;   
		    }
            return player.transform.position.Distance(mTransform.position);
		}
		
		public Runner.PlatformObject Clone()
		{
			var clone = ((Runner.PlatformObject)GameObject.Instantiate(this));
			clone.Id = this.Id;
			clone.NextPlatforms = this.NextPlatforms;
			clone.IsStartPlatform = this.IsStartPlatform;
			clone.Level = this.Level;
			clone.Type = this.Type;
			clone.MinimumDistance = this.MinimumDistance;
			clone.AllowDispose = false;
			clone.Size = this.Size;
			//clone.gameObject.SetActive(false);
			return clone;
		}

		override protected float DistanceTo(Transform aTransform)
		{
            float result = mTransform.position.Distance(aTransform.position) - (this.Size.z / 2.0f);
		    return result > 0.0f ? result : 0.0f;
		}
	}

    public enum PlatformMode
    {
        Platform,
        Transition
    }
}

