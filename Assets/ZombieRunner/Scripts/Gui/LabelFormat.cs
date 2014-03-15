using UnityEngine;
using System.Collections;

namespace Runner
{
	public class LabelFormat 
	{
		public static string FormatNumber(float value)
		{
			return Mathf.RoundToInt(value).ToString();
		}
	}
}