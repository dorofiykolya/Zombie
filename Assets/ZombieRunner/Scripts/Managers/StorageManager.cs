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
		void Awake()
		{
            //Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");

            Load ();

			if(PlayerData.playerID == "")
			{
				PlayerData.playerID = SystemInfo.deviceUniqueIdentifier;
				PlayerData.realName = "Player_" + UnityEngine.Random.Range(100000, 999999);

				PlayerPrefs.SetString("RealName", PlayerData.realName);
				PlayerPrefs.SetString("PlayerID", PlayerData.playerID);
			}
		}

        internal static void Load()
        {
            var missionManager = GameObject.FindObjectOfType<MissionManager>();

            PlayerData.CharacterId = PlayerPrefs.GetInt("CharacterId");
			PlayerData.Distance = PlayerPrefs.GetInt("Distance");
			PlayerData.SetBrains(PlayerPrefs.GetInt("Brains"));
			PlayerData.missionMulti = PlayerPrefs.GetInt("Multi");
			PlayerData.playerID = PlayerPrefs.GetString ("PlayerID");
			PlayerData.realName = PlayerPrefs.GetString ("RealName");
			PlayerData.creation = PlayerPrefs.GetInt ("Creation");
			PlayerData.tutorial = PlayerPrefs.GetInt ("Tutorial");
            var tutorial = PlayerPrefs.GetString("Zombie");
            if (tutorial == "")
            {
                tutorial = "0,0,0,0,0";
            }

            PlayerData.zombieTutorial = tutorial.Split(',');

            var missions = Deserialize(PlayerPrefs.GetString("Missions")) as MissionQueue[];
            missionManager.Load(missions);

			var powerLevels = Deserialize(PlayerPrefs.GetString("PowerLevels")) as int[];
			if(powerLevels != null)
			{
				PowerUpManager.levels = powerLevels;
			}

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
			if(PlayerData.Distance > PlayerPrefs.GetInt("Distance"))
				PlayerPrefs.SetInt("Distance", PlayerData.Distance);
			PlayerPrefs.SetInt("Brains", PlayerData.Brains);
			PlayerPrefs.SetInt("Multi", PlayerData.missionMulti);
			PlayerPrefs.SetString("Missions", Serialize(missionManager.MissionPlayerQueues));
			PlayerPrefs.SetString("CharacterLevels", Serialize (PlayerManager.levels));
			PlayerPrefs.SetString("PowerLevels", Serialize (PowerUpManager.levels));
			PlayerPrefs.SetString("RealName", PlayerData.realName);
			PlayerPrefs.SetString("PlayerID", PlayerData.playerID);
			PlayerPrefs.SetInt("Tutorial", PlayerData.tutorial);
            PlayerPrefs.SetString("Zombie", String.Join(",", PlayerData.zombieTutorial));
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
