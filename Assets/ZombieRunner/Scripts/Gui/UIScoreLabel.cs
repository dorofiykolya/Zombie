using UnityEngine;
using System.Collections;

namespace Runner
{
	public class UIScoreLabel : ComponentManager {

		public UILabel Label;

		void Update () 
		{
			if(Label == null) return;
			Label.text = LabelFormat.FormatNumber(Player.Distance);
		}
	}
}