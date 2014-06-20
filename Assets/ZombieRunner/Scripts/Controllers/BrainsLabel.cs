using UnityEngine;
using System.Collections;

namespace Runner
{
	public class BrainsLabel : MonoBehaviour 
	{
		void Awake()
		{
			PlayerData.OnChanged += UpdateLabel;
			GetComponent<UILabel> ().text = PlayerData.Brains.ToString();
		}

		private void UpdateLabel(string count)
		{
			GetComponent<UILabel> ().text = count;
		}
	}
}
