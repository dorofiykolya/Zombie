using UnityEngine;
using System.Collections;

public class LevelAction : MonoBehaviour 
{
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {       
            Ray ray = Camera.allCameras[1].ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 300);

            if(hit.transform != null)
            {
                int level = int.Parse(hit.transform.name.Split(' ')[1]);
            }
        }
    }
}
