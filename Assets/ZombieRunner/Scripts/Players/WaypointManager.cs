using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
    public class WaypointManager : MonoBehaviour
    {
        public Transform[] list;
        public static Transform[] wayPoints;
        public static int currentWP;

        // Use this for initialization
        void Awake()
        {
            currentWP = 1;

            wayPoints = list;
        }

        public static void changeWP(bool right)
        {
            int newWp = right ? currentWP + 1 : currentWP - 1;
            newWp = Mathf.Clamp(newWp, 0, wayPoints.Length - 1);

            if (newWp == currentWP)
                return;

            currentWP = newWp;
			
			for(int i = 0; i < PlayerManager.currentList.Count; i++)
			{
            	PlayerManager.currentList[i].changeTarget(wayPoints[currentWP].transform.position, right);
			}
        }
    }
}
