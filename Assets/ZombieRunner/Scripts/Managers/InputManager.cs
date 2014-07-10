using UnityEngine;
using System.Collections;

namespace Runners
{
	
//	public class TouchEvent
//	{
//		public Vector2 position;
//		public Vector2 previousPosition;
//		public float deltaTime;
//		
//		public TouchEvent Clone()
//		{
//			return new TouchEvent(){position=position,previousPosition=previousPosition,deltaTime=deltaTime};
//		}
//	}

	public enum MouseButton
	{
		LEFT = 0,
		RIGHT = 1,
		MIDDLE = 2
	}
	
	public class InputManager : MonoBehaviour {
		
		private static InputManager instance;
		public static InputManager Instance	{get{return instance;}}
		
		public static event System.Action<Vector2> OnSlideLeft;
		public static event System.Action<Vector2> OnSlideRight;
		public static event System.Action<Vector2> OnSlideUp;
		public static event System.Action<Vector2> OnSlideDown;
		
		public static event System.Action<Vector2> OnDown;
		public static event System.Action<Vector2> OnUp;
		public static event System.Action<Vector2> OnMove;
		
		private Vector2 touchPosition;
		private Vector3 mousePosition;
		//private float mouseDownTime;
		
		public bool touchEnabled = true;
		public bool mouseEnabled = true;
		public float accelerationSensitivity = 0.25f;
		
		public Vector2 slide = new Vector2(40, 40);
		public float sensitivity = 2;
		public float mouseSlideTime = 0.5f;
		
		public Vector2 Position
		{
			get
			{
				if(touchPosition != null)
				{
					return touchPosition;	
				}
				return new Vector2(mousePosition.x, mousePosition.y);
			}
		}
		
		public bool IsAccelerationRight
		{
			get{ return Input.acceleration.x > accelerationSensitivity;}
		}
		public bool IsAccelerationLeft
		{
			get{return Input.acceleration.x < -accelerationSensitivity;}	
		}
		
		void Awake()
		{
			instance = this;	
		}
		
		// Use this for initialization
		void Start ()
		{
			OnSlideLeft += arg => Debug.Log("left");
			OnSlideRight += arg => Debug.Log("right");
			OnSlideUp += arg => Debug.Log("up");
			OnSlideDown += arg => Debug.Log("down");
		}
		
		// Update is called once per frame
		void Update () {
			if(enabled)
			{
				if(touchEnabled && Input.touchCount == 1)
				{
					Touch touch;
					touch = Input.touches[0];
					switch(touch.phase)
					{
						case TouchPhase.Began:
						{
							touchPosition = touch.position;
							DispathDown(ref touchPosition);
							break;
						}
						case TouchPhase.Moved:
						{
							var position = touch.position;
							DispathMove(ref position);
							var dif = position - touchPosition;
							var verticalPercent = Mathf.Abs(dif.y / dif.x);
							if (verticalPercent > sensitivity && Mathf.Abs(dif.y) > slide.y) 
							{
								if (dif.y > 0) 
								{
									DispathSlideUp(ref position);
								}
								else if (dif.y < 0) 
								{
									DispathSlideDown(ref position);
								}
								touchPosition = position;
							} 
							else if ( verticalPercent < (1 / sensitivity) && Mathf.Abs(dif.x) > slide.x) 
							{
		                        if(dif.x > 0)
								{
									DispathSlideRight(ref position);
								}
								else
								{
									DispathSlideLeft(ref position);
								}
							}
							break;
						}
						case TouchPhase.Ended:
						case TouchPhase.Canceled:
						{
							var position = touch.position;
							DispathUp(ref position);
							break;
						}
						case TouchPhase.Stationary:
							break;
					}
				}
				else if(mouseEnabled)
				{
					if(Input.GetMouseButtonDown((int)MouseButton.LEFT))
					{
						//mouseDownTime = Time.time;
						Vector3 position = Input.mousePosition;
						mousePosition = position;
						DispathDown(ref position);
					}
					else if(Input.GetMouseButton((int)MouseButton.LEFT))
					{
						Vector3 position = Input.mousePosition;
						DispathMove(ref position);
						var dif = position - mousePosition;
						var verticalPercent = Mathf.Abs(dif.y / dif.x);
						if (verticalPercent > sensitivity && Mathf.Abs(dif.y) > slide.y) 
						{
							if (dif.y > 0) 
							{
								DispathSlideUp(ref position);
							}
							else if (dif.y < 0) 
							{
								DispathSlideDown(ref position);
							}
							touchPosition = position;
						} 
						else if ( verticalPercent < (1 / sensitivity) && Mathf.Abs(dif.x) > slide.x) 
						{
	                        if(dif.x > 0)
							{
								DispathSlideRight(ref position);
							}
							else
							{
								DispathSlideLeft(ref position);
							}
						}
					}
					else if(Input.GetMouseButtonUp((int)MouseButton.LEFT))
					{
						Vector3 position = Input.mousePosition;
						DispathUp(ref position);
					}
				}
			}
		}
		private void DispathMove(ref Vector2 v){if(OnMove!=null)OnMove(v);}
		private void DispathMove(ref Vector3 v){if(OnMove!=null)OnMove(new Vector2(v.x,v.y));}
		
		private void DispathUp(ref Vector2 v){if(OnUp!=null)OnUp(v);}
		private void DispathUp(ref Vector3 v){if(OnUp!=null)OnUp(new Vector2(v.x,v.y));}
		
		private void DispathDown(ref Vector2 v){if(OnDown!=null)OnDown(v);}
		private void DispathDown(ref Vector3 v){if(OnDown!=null)OnDown(new Vector2(v.x,v.y));}
		
		private void DispathSlideUp(ref Vector2 v){if(OnSlideUp!=null)OnSlideUp(v);}
		private void DispathSlideUp(ref Vector3 v){if(OnSlideUp!=null)OnSlideUp(new Vector2(v.x,v.y));}
		
		private void DispathSlideDown(ref Vector2 v){if(OnSlideDown!=null)OnSlideDown(v);}
		private void DispathSlideDown(ref Vector3 v){if(OnSlideDown!=null)OnSlideDown(new Vector2(v.x,v.y));}
		
		private void DispathSlideLeft(ref Vector2 v){if(OnSlideLeft!=null)OnSlideLeft(v);}
		private void DispathSlideLeft(ref Vector3 v){if(OnSlideLeft!=null)OnSlideLeft(new Vector2(v.x,v.y));}
		
		private void DispathSlideRight(ref Vector2 v){if(OnSlideRight!=null)OnSlideRight(v);}
		private void DispathSlideRight(ref Vector3 v){if(OnSlideRight!=null)OnSlideRight(new Vector2(v.x,v.y));}
	}
}
