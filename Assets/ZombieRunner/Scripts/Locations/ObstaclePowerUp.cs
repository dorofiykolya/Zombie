using UnityEngine;
using System.Collections;

namespace Runner
{
	public class ObstaclePowerUp : LocationObject 
	{
		public int Id;
		public int[] prices;
		public int[] effect;

		public Runner.ObstaclePowerUp Clone()
		{
			var clone = ((Runner.ObstaclePowerUp)GameObject.Instantiate(this));
			clone.Id = this.Id;
			return clone;
		}
	}
}
