using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
    public class WaypointManager : ComponentManager
    {
        public Transform[] list;
        public static Transform[] wayPoints;
        public static int currentWP;

        public override void Initialize()
        {
            currentWP = 1;
            wayPoints = list;
        }

        public void changeWP(bool right)
        {
            int newWp = right ? currentWP + 1 : currentWP - 1;
            newWp = Mathf.Clamp(newWp, 0, wayPoints.Length - 1);

            if (newWp == currentWP)
                return;

            currentWP = newWp;

            for (int i = 0; i < Player.currentList.Count; i++)
			{
                Player.currentList[i].changeTarget(wayPoints[currentWP].transform.position, right);
			}
        }
    }
}
