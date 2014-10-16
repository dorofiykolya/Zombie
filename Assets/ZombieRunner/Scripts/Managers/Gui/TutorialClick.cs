using UnityEngine;
using System.Collections;

namespace Runner
{
    public class TutorialClick : MonoBehaviour 
    {
        void OnClick()
        {
            Time.timeScale = 1;
            TutorialAction.hideTutorial();
        }
    }
}
