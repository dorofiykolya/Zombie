using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Runner
{
    [Serializable]
	public class StorageManager : ScriptableObject
    {
        private static fastJSON.JSON json = fastJSON.JSON.Instance;
		void Awake()
		{
			Load ();
		}

        internal static void Load()
        {
            if (PlayerPrefs.HasKey("CharacterId") == false)
            {
                return;
            }

            var missionManager = GameObject.FindObjectOfType<MissionManager>();

            PlayerData.CharacterId = PlayerPrefs.GetInt("CharacterId");
			PlayerData.Distance = PlayerPrefs.GetFloat("Distance");
			PlayerData.Brains = PlayerPrefs.GetInt("Brains");
            var missions = Deserialize(PlayerPrefs.GetString("Missions")) as MissionQueue[];
            missionManager.Load(missions);
			var characterLevels = Deserialize(PlayerPrefs.GetString("CharacterLevels")) as int[];
			if(characterLevels == null)
			{
				PlayerManager.levels = PlayerManager.defaultLevels;
			}
			else
			{
				PlayerManager.levels = characterLevels;
			}
        }

        internal static void Save()
        {
            var missionManager = GameObject.FindObjectOfType<MissionManager>();

            PlayerPrefs.SetInt("CharacterId", PlayerData.CharacterId);
			PlayerPrefs.SetFloat("Distance", PlayerData.Distance);
			PlayerPrefs.SetInt("Brains", PlayerData.Brains);
            PlayerPrefs.SetString("Missions", Serialize(missionManager.MissionQueues));
			PlayerPrefs.SetString("CharacterLevels", Serialize (PlayerManager.levels));
        }

        private static string Serialize(object obj)
        {
            var binary = new BinaryFormatter();
            var memory = new MemoryStream();
            binary.Serialize(memory, obj);
            return Convert.ToBase64String(memory.GetBuffer());
        }

        private static object Deserialize(string data)
        {
            try
            {
                var binary = new BinaryFormatter();
                var memory = new MemoryStream(Convert.FromBase64String(data));
                return binary.Deserialize(memory);
            }
            catch(Exception)
            {

            }
            return null;
        }
    }
}
