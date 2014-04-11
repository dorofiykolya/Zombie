using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Runner
{
	public class PlatformGenerator
	{
		private Vector3 vectorHelper = new Vector3(0,0,0);
		private PlatformObject _next;
        private LocationChildren _platforms;
        private LocationDisposeManager _disposedManager;
        private PlayerManager _player;

        public PlatformGenerator(LocationChildren Platforms, LocationDisposeManager DisposedManager, PlayerManager player)
        {
            this._platforms = Platforms;
            this._disposedManager = DisposedManager;
            this._player = player;
        }
		
		public void Reset()
		{
			_next = null;
		}
			
		public void Generate(float speed, Runner.PlayerController player)
		{
			Runner.PlatformObject platform = _platforms.Last;
			bool isStartPlatform = false;
			if(platform == null)
			{
				platform = LocationPlatformManager.GetRandomStartPlatform(Runner.PlayerData.PlatformType);
				isStartPlatform = true;
				if(platform == null)
				{
					ErrorManager.Show("empty start platform", "add start platforms");
					return;
				}
				PlayerData.PlatformTypeRemainingDistance = Runner.LocationPlatformManager.GetDistanceByType(platform.Type);
				_next = platform.GetNextRandom();
                _platforms.Add(platform);
			}
			float distance = platform.Distance(player);
			Vector3 size;
			int count = 0;
			while(distance <= LocationManager.GenerateDistance && count < Runner.LocationManager.MaxGeneratePlatforms)
			{
				int nextType = -1;
				if(PlayerData.PlatformTypeRemainingDistance <= 0.0f && isStartPlatform == false)
				{
					nextType = LocationPlatformManager.GetTypeByDistance(player.Distance, PlayerData.PlatformType, Random.Range(0,2) > 0);
					if(nextType == -1)
					{
						nextType = platform.Type;	
					}
					if(nextType != PlayerData.PlatformType)
					{
						_next = LocationPlatformManager.GetTransitionPlatform(PlayerData.PlatformType, nextType);
					}
					if(_next == null)
					{
						_next = LocationPlatformManager.GetRandomPlatformByTypeAndDistance(PlayerData.PlatformType, player.Distance);
					}
					PlayerData.PlatformType = nextType;
					PlayerData.PlatformTypeRemainingDistance += LocationPlatformManager.GetDistanceByType(nextType);
				}
				else
				{
					_next = _next != null? _next : LocationPlatformManager.GetRandomPlatformByTypeAndDistance(PlayerData.PlatformType, _player.Distance);
				}
				LocationPlatformManager.GetSize(out size, platform);
				vectorHelper.x = 0;
				vectorHelper.y = 0;
				vectorHelper.z = size.z - size.z / 2;
				_next.transform.position = platform.transform.position + vectorHelper;
				platform = _next;
				LocationPlatformManager.GetSize(out size, platform);
				vectorHelper.x = 0;
				vectorHelper.y = 0;
				vectorHelper.z = size.z - size.z / 2;
				platform.transform.position += vectorHelper;
				distance = platform.Distance(player);
                _platforms.Add(platform);
				_next = platform.GetNextRandom();
				count++;
			}
		}
		
		public void Move(float speed, Runner.PlayerController player)
		{
            var result = from c in _platforms.List
				where c.transform.position.z < player.transform.position.z
					select c;
			
			foreach(var current in result)
			{
				current.AllowDispose = true;
				_disposedManager.Add(current);	
			}
		}
		
	}
}

