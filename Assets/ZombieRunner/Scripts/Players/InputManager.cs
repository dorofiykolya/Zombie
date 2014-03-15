using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
    public class InputManager : MonoBehaviour
    {
        public Vector2 swipeDistance = new Vector2(40, 40);
        public float swipeSensitivty = 2;
        private Vector2 touchStartPosition;
        private bool changeUsed;
        private int patientZeroIndex = 0;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!PlayerManager.currentList[patientZeroIndex].isJumping)
                {
                    for (int i = 0; i < PlayerManager.currentList.Count; i++)
                    {
                        PlayerManager.currentList[i].doJump();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!PlayerManager.currentList[patientZeroIndex].isSliding)
                {
                    for (int i = 0; i < PlayerManager.currentList.Count; i++)
                    {
                        PlayerManager.currentList[i].doSlide();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                WaypointManager.changeWP(false);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                WaypointManager.changeWP(true);
            }
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 diff = touch.position - touchStartPosition;
                    if (diff.x == 0f)
                        diff.x = 1f;
                    float verticalPercent = Mathf.Abs(diff.y / diff.x);

                    if (verticalPercent > swipeSensitivty && Mathf.Abs(diff.y) > swipeDistance.y)
                    {
                        if (diff.y > 0)
                        {
                            if (!PlayerManager.currentList[patientZeroIndex].isJumping)
                            {
                                for (int i = 0; i < PlayerManager.currentList.Count; i++)
                                {
                                    PlayerManager.currentList[i].doJump();
                                }
                            }
                        }
                        else if (diff.y < 0)
                        {
                            for (int i = 0; i < PlayerManager.currentList.Count; i++)
                            {
                                PlayerManager.currentList[i].doSlide();
                            }
                        }
                        touchStartPosition = touch.position;
                    }
                    else if (verticalPercent < (1 / swipeSensitivty) && Mathf.Abs(diff.x) > swipeDistance.x && !changeUsed)
                    {
                        changeUsed = true;
                        WaypointManager.changeWP(diff.x > 0);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    changeUsed = false;
                }
            }
        }
    }
}
