using UnityEngine;
using System.Collections;

public class ResolutionLayout : MonoBehaviour 
{
    private const float PERCENT = 0.666f;

	// Use this for initialization
	void Awake () 
    {
        var layout = (Screen.width / (float)Screen.height) / PERCENT;
        
        transform.localScale = new Vector3(layout, layout, layout);
	}
}
