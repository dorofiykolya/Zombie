using UnityEngine;
using System.Collections;

namespace Runner
{
	public class BrainsLabel : MonoBehaviour 
	{
		void Awake()
		{
			PlayerData.OnChanged += UpdateLabel;
		}

		private void UpdateLabel(string count)
		{
			GetComponent<UILabel> ().text = count;
		}
	}
}
