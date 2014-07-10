using UnityEngine;
using System.Collections;

namespace Runner
{
	public class UIScoreMultiplierLabel : MonoBehaviour {

		public UILabel Label;

		void Update () 
		{
			if(Label == null) return;
			Label.text = "x1";
		}
	}
}