using UnityEngine;
using System.Collections;

namespace Runner
{
    public class LevelAction : MonoBehaviour 
    {
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {       
                Ray ray = Camera.allCameras[1].ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 300, 1 << 19);

                if(hit.transform != null)
                {
                    int level = int.Parse(hit.transform.name.Split(' ')[1]);
                    LevelsManager.currentLevel = "task " + level;

                    var manager = GameObject.FindObjectOfType<Manager>();


                    manager.Player.isStop = false;
                    manager.States.Current = State.GAME;
                    GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
                    GUIPanelManager.Get(PanelType.GameMenu).Show();
                    GUIPanelManager.currentPanel = PanelType.GameMenu;
                    manager.Game.GameStart();

                    gameObject.SetActive(false);
                }
            }
        }
    }
}
