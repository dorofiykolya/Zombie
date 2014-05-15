using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
    public class WaypointManager : ComponentManager
    {
        public Transform[] wayPoints;
        public int currentWP;
		public int transitWP;

		public override void GameStop ()
		{
			transitWP = currentWP;
			currentWP = 1;
		}

        public void changeWP(bool right)
        {
            int newWp = right ? currentWP + 1 : currentWP - 1;
            newWp = Mathf.Clamp(newWp, 0, wayPoints.Length - 1);

            if (newWp == currentWP)
                return;

			transitWP = currentWP;
            currentWP = newWp;

            for (int i = 0; i < Player.currentList.Count; i++)
			{
                Player.currentList[i].changeTarget(wayPoints[currentWP].transform.position, right);
			}
        }
    }
}
