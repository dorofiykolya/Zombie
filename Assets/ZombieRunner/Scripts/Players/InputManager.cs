using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner
{
	public enum SwipeDirection 
	{
		Null = 0,
		Duck = 1,
		Jump = 2,
		Right = 3,
		Left = 4
	}

    public class InputManager : MonoBehaviour
    {
		private SwipeDirection sSwipeDirection;
		
		//distance calculation
		private float fInitialX;
		private float fInitialY;
		private float fFinalX;
		private float fFinalY;
		
		private float inputX;
		private float inputY;
		private float slope;
		private float fDistance;
		private float iTouchStateFlag;
		
		void Start ()
		{
			fInitialX = 0.0f;
			fInitialY = 0.0f;
			fFinalX = 0.0f;
			fFinalY = 0.0f;
			
			inputX = 0.0f;
			inputY = 0.0f;
			
			iTouchStateFlag = 0;
			sSwipeDirection = SwipeDirection.Null;
		}
		
		void Update()
		{
			if(StateManager.Current == State.LOSE || StateManager.Current == State.LOAD)
			{
				return;
			}
			//ARROWS
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (!PlayerManager.currentList[0].bInAir)
				{
					for (int i = 0; i < PlayerManager.currentList.Count; i++)
					{
						PlayerManager.currentList[i].doJump();
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (!PlayerManager.currentList[0].bInDuck)
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
			//TOUCH
			if (iTouchStateFlag == 0 && Input.GetMouseButtonDown(0))
			{		
				fInitialX = Input.mousePosition.x;
				fInitialY = Input.mousePosition.y;
				
				sSwipeDirection = SwipeDirection.Null;
				iTouchStateFlag = 1;
			}
			if (iTouchStateFlag == 1)
			{
				fFinalX = Input.mousePosition.x;
				fFinalY = Input.mousePosition.y;
				
				sSwipeDirection = swipeDirection();
				if (sSwipeDirection != SwipeDirection.Null)
				{
					iTouchStateFlag = 2;

					switch (sSwipeDirection)
					{
						case SwipeDirection.Jump:
							if (!PlayerManager.currentList[0].bInAir)
							{
								for (int i = 0; i < PlayerManager.currentList.Count; i++)
								{
									PlayerManager.currentList[i].doJump();
								}
							}
							break;
						case SwipeDirection.Duck:
							if (!PlayerManager.currentList[0].bInDuck)
							{
								for (int i = 0; i < PlayerManager.currentList.Count; i++)
								{
									PlayerManager.currentList[i].doSlide();
								}
							}
						break;
						case SwipeDirection.Left:
							WaypointManager.changeWP(false);
							break;
						case SwipeDirection.Right:
							WaypointManager.changeWP(true);
							break;
					}
				}
			}
			if (iTouchStateFlag == 2 || Input.GetMouseButtonUp(0))
			{
				iTouchStateFlag = 0;
			}
		}
	
		private SwipeDirection swipeDirection()
		{
			inputX = fFinalX - fInitialX;
			inputY = fFinalY - fInitialY;
			slope = inputY / inputX;
			
			fDistance = Mathf.Sqrt( Mathf.Pow((fFinalY-fInitialY), 2) + Mathf.Pow((fFinalX-fInitialX), 2) );
			
			if (fDistance <= (Screen.width / 15))
				return SwipeDirection.Null;
			
			if (inputX >= 0 && inputY > 0 && slope > 1)
			{		
				return SwipeDirection.Jump;
			}
			else if (inputX <= 0 && inputY > 0 && slope < -1)
			{
				return SwipeDirection.Jump;
			}
			else if (inputX > 0 && inputY >= 0 && slope < 1 && slope >= 0)
			{
				return SwipeDirection.Right;
			}
			else if (inputX > 0 && inputY <= 0 && slope > -1 && slope <= 0)
			{
				return SwipeDirection.Right;
			}
			else if (inputX < 0 && inputY >= 0 && slope > -1 && slope <= 0)
			{
				return SwipeDirection.Left;
			}
			else if (inputX < 0 && inputY <= 0 && slope >= 0 && slope < 1)
			{
				return SwipeDirection.Left;
			}
			else if (inputX >= 0 && inputY < 0 && slope < -1)
			{
				return SwipeDirection.Duck;
			}
			else if (inputX <= 0 && inputY < 0 && slope > 1)
			{
				return SwipeDirection.Duck;
			}
			
			return SwipeDirection.Null;	
		}
    }
}
