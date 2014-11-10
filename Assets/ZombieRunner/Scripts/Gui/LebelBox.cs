using UnityEngine;
using System.Collections;

namespace Runner
{
    public class LebelBox : MonoBehaviour 
    {
    	// Use this for initialization
        IEnumerator Start () 
        {
            if (PlayerData.currentLevels == null)
                yield return null;

            gameObject.GetComponentInChildren<UILabel>().text = gameObject.name;

            animation.Stop();

            var stars = gameObject.GetComponentsInChildren<TweenScale>();

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].transform.localScale = Vector3.zero;
                stars[i].enabled = false;
            }

            int level = int.Parse(name.Split(' ')[1]);

            //collider.enabled = false;

            if (!PlayerData.currentLevels [level - 1].IsCompleted && level != 1)
                return false;

            collider.enabled = true;
            
            for (int i = 0; i < stars.Length; i++)
            {
                if(i == 0) stars[i].GetComponent<UISprite>().spriteName = PlayerData.currentLevels [level - 1].Current >= PlayerData.currentLevels [level - 1].Target1 ? "star_yellow" : "star_empty";
                else if(i == 1) stars[i].GetComponent<UISprite>().spriteName = PlayerData.currentLevels [level - 1].Current >= PlayerData.currentLevels [level - 1].Target2 ? "star_yellow" : "star_empty";
                else if(i == 2) stars[i].GetComponent<UISprite>().spriteName = PlayerData.currentLevels [level - 1].Current >= PlayerData.currentLevels [level - 1].Target3 ? "star_yellow" : "star_empty";
                stars[i].from = Vector3.zero;
                stars[i].to = new Vector3(0.01f, -0.01f, 0.01f);
                stars[i].delay = (level + i) / 2f + 1f;
                stars[i].duration = 0.5f;
                stars[i].ResetToBeginning();
                stars[i].enabled = true;
            }

            yield return new WaitForSeconds(level / 2f);
            animation.Play("Take 001");
    	}

        void OnEnable()
        {
            StartCoroutine(Start());
        }

        void OnClick()
        {
            int level = int.Parse(name.Split(' ')[1]);
            Debug.Log(PlayerData.tutorial);
            LevelsManager.currentLevel = "task " + level;
            
            var manager = GameObject.FindObjectOfType<Manager>();
            
            manager.Player.isStop = false;
            manager.States.Current = State.GAME;
            GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
            GUIPanelManager.Get(PanelType.GameMenu).Show();
            GUIPanelManager.currentPanel = PanelType.GameMenu;
            manager.Game.GameStart();
        }
    }
}
