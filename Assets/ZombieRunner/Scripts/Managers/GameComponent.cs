using System;
namespace Runner
{
	public class GameComponent : IComponentManager
	{
		public GameComponent ()
		{
			Manager.Instance.Register(this);
		}

		public virtual void GameStart()
		{
			
		}
		
		public virtual void GameStop()
		{
			
		}
		
		public virtual void GamePause()
		{
			
		}
		
		public virtual void GameResume()
		{
			
		}
		
		public virtual void GameRestart()
		{
			
		}
	}
}

