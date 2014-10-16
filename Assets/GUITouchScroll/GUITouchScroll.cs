using UnityEngine;
using System.Collections;

public class GUITouchScroll : MonoBehaviour 
{
    // Internal variables for managing touches and drags
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded = 0f;
	
    private Vector3 scrollPosition = new Vector3(0f, 4.5f, 0f);

	public float inertiaDuration = 0.75f;

    public GameObject platforms;

    void Update()
    {
        platforms.transform.position = scrollPosition;

		if (Input.touchCount != 1)
		{
			if ( scrollVelocity != 0.0f )
			{
				// slow down over time
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
                scrollPosition.y = Mathf.Max(0.75f, Mathf.Min(4.5f, scrollPosition.y + ((1.5f / Screen.height) * (frameVelocity * Time.deltaTime))));

				// after N seconds, we've stopped
				if (t >= 1.0f) scrollVelocity = 0.0f;
			}
			return;
		}
		
		Touch touch = Input.touches[0];

		if (touch.phase == TouchPhase.Began)
		{
			scrollVelocity = 0.0f;
		}
		else if (touch.phase == TouchPhase.Moved)
		{
			// dragging
            scrollPosition.y = Mathf.Max(0.75f, Mathf.Min(4.5f, scrollPosition.y + ((1.5f / Screen.height) * touch.deltaPosition.y)));

            if (Mathf.Abs(touch.deltaPosition.y) >= 10) 
                scrollVelocity = (int)(touch.deltaPosition.y / touch.deltaTime);
            
            timeTouchPhaseEnded = Time.time;
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			// impart momentum, using last delta as the starting velocity
			// ignore delta < 10; precision issues can cause ultra-high velocity
			if (Mathf.Abs(touch.deltaPosition.y) >= Screen.height * 0.05f) 
				scrollVelocity = (int)(touch.deltaPosition.y / touch.deltaTime);
				
			timeTouchPhaseEnded = Time.time;
		}
	}

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(0, 0, 300, 300), scrollPosition.y.ToString());
    }
}
 