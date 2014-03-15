using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Runner
{
	public class LocationManager : MonoBehaviour 
	{
		//public
		public Runner.PlatformObject[] startPlatforms;
		public Runner.PlatformObject[] platforms;
        public Runner.PlatformObject[] transitionPlatforms;
		public Runner.PlatformInfo[] platformsInfo;
		
		public float SetDisposeDistance = 10.0f;
		public float SetMinDisposeMultiply = 1.0f;
		public float SetGenerateDistance = 100.0f;
		public int SetMaxGeneratePlatforms = 20;
		public float SetMinQualityShader = 600.0f;
		
		private static GameObject GlobalPlatformContainer;
		public static GameObject PlatformContainer{get;private set;}
		public static GameObject DisposedPlatformContainer{get;private set;}
		
		public static float DisposeDistance{get;private set;}
		public static float MinDisposeMultiply{get;private set;}
		public static float GenerateDistance{get;private set;}
		public static int MaxGeneratePlatforms{get;private set;}
		
		public static Runner.LocationManager Instance {get;private set;}
		public static LocationChildren Platforms{get;private set;}
		
		//private
		
		//methods
		#region public methods
		
		
		
		#endregion public methods
		#region privateMethods
		
		private void Awake()
		{
			Instance = this;
			GlobalPlatformContainer = new GameObject("Platforms");
			PlatformContainer = new GameObject("PlatformContainer");
			DisposedPlatformContainer = new GameObject("DisposedPlatformContainer");
			GlobalPlatformContainer.AddChild(PlatformContainer);
			GlobalPlatformContainer.AddChild(DisposedPlatformContainer);
		}
		
		private void Start () 
		{
			Instance = this;
			Platforms = new LocationChildren();
			LocationPlatformManager.ParsePlatforms(platforms, startPlatforms, transitionPlatforms);
			LocationPlatformManager.ParseInfo(platformsInfo);
			
			PlayerData.PlatformType = 0;
			
			LoadPlayerData();
			
			DisposeDistance = SetDisposeDistance;
			MinDisposeMultiply = SetMinDisposeMultiply;
			GenerateDistance = SetGenerateDistance;
			MaxGeneratePlatforms = SetMaxGeneratePlatforms;
		}
		
		public static void Restart ()
		{
			if(Instance == null)
			{
				return;	
			}
			var manager = Instance;
			PlayerManager.Restart();
			Platforms.RemoveAll();
			LocationDisposeManager.RemoveAll();
			PlatformGenerator.Reset();
		}
		
		private void Update () 
		{
			if(ErrorManager.HasError)
			{
				return;	
			}
			
			DisposeDistance = SetDisposeDistance;
            MinDisposeMultiply = SetMinDisposeMultiply;
			GenerateDistance = SetGenerateDistance;
			MaxGeneratePlatforms = SetMaxGeneratePlatforms;
			
			Runner.PlayerController player = Runner.PlayerManager.Player;
			if(player == null)
			{
				ErrorManager.Show("ERROR","player == null");
				return;
			}
			float moveSpeed = Runner.PlayerManager.Speed * Time.deltaTime;
			Runner.PlayerManager.Move(moveSpeed);
			PlatformGenerator.Generate(moveSpeed, player);
			Platforms.Move(moveSpeed, player, SetMinQualityShader);
			LocationDisposeManager.Update(player);
			
		}
		
		private void LoadPlayerData()
		{
			if(PlayerPrefs.HasKey("CharacterId") == false)
			{
				return;	
			}
			PlayerData.CharacterId = PlayerPrefs.GetInt("CharacterId");
			PlayerData.PlatformType = PlayerPrefs.GetInt("PlatformMode");
			PlayerData.PlatformTypeRemainingDistance = PlayerPrefs.GetFloat("PlatformTypeRemainingDistance");
		}
		
		private void SavePlayerData()
		{
			PlayerPrefs.SetInt("CharacterId", PlayerData.CharacterId);
			PlayerPrefs.SetInt("PlatformMode", PlayerData.PlatformType);
			PlayerPrefs.SetFloat("PlatformTypeRemainingDistance", PlayerData.PlatformTypeRemainingDistance);
		}

		#endregion privateMethods
	}
}

