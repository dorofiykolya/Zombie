using UnityEngine;
using System;
using System.Collections;

namespace Runner
{
    public class LevelsManager : ComponentManager 
    {
        [Serializable]
        public class Level
        {
            public string Id;
            public string Name;
            public string DescriptionRussian;
            public string DescriptionEnglish;
            public float Current;
            public float Target1;
            public float Target2;
            public float Target3;
            public bool IsCompleted;
        }

        public Level[] Levels;

        public UISprite[] Stars;
        public UILabel[] Labels;
        public UISprite bar;

        public UISprite[] WindowSprite;
        public UILabel[] WindowText;
        public UILabel WindowDesc;

        public static string currentLevel;

        private float currentProgress;

        public override void GameStart()
        {
            GameRestart();
        }

        public override void GameStop()
        {
            bool isComplete = false;
            foreach (var level in PlayerData.currentLevels)
            {
                if(isComplete)
                {
                    isComplete = false;
                    level.IsCompleted = true;
                }

                if(level.Current >= level.Target1)
                {
                    isComplete = true;
                }
            }

            StorageManager.Save();
            GamePause();
        }

        public override void GameRestart()
        {
            currentProgress = 0;

            foreach (var level in PlayerData.currentLevels)
            {
                if(level.Id == currentLevel)
                {
                    bar.width = (int)((level.Current / level.Target3) * 228f);

                    Stars[0].enabled = level.Current >= level.Target1;
                    Stars[1].enabled = level.Current >= level.Target2;
                    Stars[2].enabled = level.Current >= level.Target3;

                    Labels[0].text = level.Target1.ToString();
                    Labels[1].text = level.Target2.ToString();
                    Labels[2].text = level.Target3.ToString();
                    
                    return;
                }
            }
        }

        public override void GamePause()
        {
            foreach (var level in PlayerData.currentLevels)
            {
                if (level.Id == currentLevel)
                {
                    WindowText [0].text = level.Target1.ToString();
                    WindowText [1].text = level.Target2.ToString();
                    WindowText [2].text = level.Target3.ToString();

                    WindowSprite[0].spriteName = level.Current >= level.Target1 ? "toggle_arrow" : "star_yellow";
                    WindowSprite[1].spriteName = level.Current >= level.Target2 ? "toggle_arrow" : "star_yellow";
                    WindowSprite[2].spriteName = level.Current >= level.Target3 ? "toggle_arrow" : "star_yellow";

                    WindowDesc.text = Localization.language == "Russian" ? level.DescriptionRussian : level.DescriptionEnglish;
                }
            }
        }

        public void Dispatch(string id, float value)
        {
            foreach (var level in PlayerData.currentLevels)
            {
                if(level.Id == id && id == currentLevel)
                {
                    currentProgress += value;

                    if(currentProgress > level.Current)
                        level.Current += value;

                    bar.width = Mathf.Min(228, (int)((level.Current / level.Target3) * 228f));

                    Stars[0].enabled = level.Current >= level.Target1;
                    Stars[1].enabled = level.Current >= level.Target2;
                    Stars[2].enabled = level.Current >= level.Target3;

                    return;
                }
            }
        }
    }
}
