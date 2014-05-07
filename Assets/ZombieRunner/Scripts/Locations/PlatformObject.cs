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
		public float MinimumDistance = 0;
		public int Type = 0;
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
			return LocationPlatformManager.PopPlatform(NextPlatforms[Random.Range(0, NextPlatforms.Length)]);
		}
		
		public void SetStartTransform()
		{
			transform.position = Vector3.zero;
		}
		
		public void Move(Vector3 move)
		{
			transform.position += move;	
		}
		
		public float Distance(Runner.PlayerController player)
		{
			return player.transform.position.Distance(transform.position);	
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
			clone.gameObject.SetActive(false);
			return clone;
		}

		override protected float DistanceToCamera(Camera camera)
		{
			return Mathf.Max(0.0f, base.DistanceToCamera(camera) - (this.Size.z / 2.0f));
		}
	}

    public enum PlatformMode
    {
        Platform,
        Transition
    }
}

