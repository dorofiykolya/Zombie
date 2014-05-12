using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
    public class WaypointManager : ComponentManager
    {
        public Transform[] wayPoints;
        public int currentWP;
		public int prevWP;

		public override void GameStop ()
		{
			currentWP = 1;
			prevWP = 1;
		}

        public void changeWP(bool right)
        {
            int newWp = right ? currentWP + 1 : currentWP - 1;
            newWp = Mathf.Clamp(newWp, 0, wayPoints.Length - 1);

            if (newWp == currentWP)
                return;

			prevWP = currentWP;
            currentWP = newWp;

            for (int i = 0; i < Player.currentList.Count; i++)
			{
                Player.currentList[i].changeTarget(wayPoints[currentWP].transform.position, right);
			}
        }
    }
}
