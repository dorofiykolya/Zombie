﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Runner
{
    public class LocationManager : ComponentManager 
	{
		public Runner.PlatformObject[] startPlatforms;
		public Runner.PlatformObject[] platforms;
        public Runner.PlatformObject[] transitionPlatforms;
		public Runner.PlatformInfo[] platformsInfo;
		
		public float SetDisposeDistance = 10.0f;
		public float SetMinDisposeMultiply = 1.0f;
		public float SetGenerateDistance = 100.0f;
		public int SetMaxGeneratePlatforms = 20;
		
		private GameObject GlobalPlatformContainer;
		public GameObject PlatformContainer{get;private set;}
		public GameObject DisposedPlatformContainer{get;private set;}
		
		public static float DisposeDistance{get;private set;}
		public static float MinDisposeMultiply{get;private set;}
		public static float GenerateDistance{get;private set;}
		public static int MaxGeneratePlatforms{get;private set;}
		
		public LocationChildren Platforms{get;private set;}
        public LocationDisposeManager DisposedManager { get; private set; }
        public PlatformGenerator Generator { get; private set; }
		
        public override void Initialize()
        {
            GlobalPlatformContainer = new GameObject("Platforms");
            PlatformContainer = new GameObject("PlatformContainer");
            DisposedPlatformContainer = new GameObject("DisposedPlatformContainer");
            GlobalPlatformContainer.AddChild(PlatformContainer);
            GlobalPlatformContainer.AddChild(DisposedPlatformContainer);

            Platforms = new LocationChildren(PlatformContainer, DisposedPlatformContainer);
            DisposedManager = new LocationDisposeManager(Platforms);
            Generator = new PlatformGenerator(Platforms, DisposedManager, Player);
            LocationPlatformManager.ParsePlatforms(platforms, startPlatforms, transitionPlatforms);
            LocationPlatformManager.ParseInfo(platformsInfo);

            PlayerData.PlatformType = 0;

            DisposeDistance = SetDisposeDistance;
            MinDisposeMultiply = SetMinDisposeMultiply;
            GenerateDistance = SetGenerateDistance;
            MaxGeneratePlatforms = SetMaxGeneratePlatforms;
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
			
			Runner.PlayerController player = Player.Current;
			if(player == null)
			{
				ErrorManager.Show("ERROR","player == null");
				return;
			}
			float moveSpeed = Player.Speed * Time.deltaTime;
			Player.Move(moveSpeed);
			Generator.Generate(moveSpeed, player);
			Platforms.Move(moveSpeed, player);
			DisposedManager.Update(player);
			
		}

        public override void GameStart()
        {
            //PlayerManager.Restart();
            Platforms.RemoveAll();
            DisposedManager.RemoveAll();
            Generator.Reset();
        }
	}
}

