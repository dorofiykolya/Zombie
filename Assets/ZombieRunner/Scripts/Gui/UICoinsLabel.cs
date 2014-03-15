using UnityEngine;
using System.Collections;

namespace Runner
{
	public class UICoinsLabel : MonoBehaviour {

		public UILabel Label;

		void Update () 
		{
			if(Label == null) return;
			Label.text = "0";
		}
	}
}