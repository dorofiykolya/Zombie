using UnityEngine;
using System.Collections;

namespace Runner
{
    public class StateManager : ComponentManager
	{
		public delegate void ExitState(State current, State exit);
		public delegate void EnterState(State enter, State exit);
		
		private State currentState;
		private State previousState = State.NONE;
		public State Current {get {return currentState;} set {SetState(value);}}
		public State Previous {get {return previousState;}}
		public event ExitState OnExitState;
		public event EnterState OnEnterState;
		public event System.Action<State> OnChanged;
		
		public State state;
		
		private void SetState(State value)
		{
			if(value == currentState)
			{
				return;
			}
			if(OnExitState != null)
			{
				OnExitState(currentState, value);
			}
			if(OnEnterState != null)
			{
				OnEnterState(value, currentState);	
			}
			previousState = currentState;
			currentState = value;
			if(OnChanged != null)
			{
				OnChanged(value);	
			}
		}

        public override void Initialize()
        {
            Current = State.LOAD;
			GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
			GUIPanelManager.Get(PanelType.MainMenu).Show();
			GUIPanelManager.currentPanel = PanelType.MainMenu;
        }

		public override void GameRestart ()
		{
			Initialize ();
		}

		public override void GameStop ()
		{
			Current = State.LOSE;
			GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
			GUIPanelManager.Get(PanelType.Lose).Show();
			GUIPanelManager.currentPanel = PanelType.Lose;
		}
        
		void OnGUI ()
		{
			if (currentState == State.LOAD && GUIPanelManager.currentPanel == PanelType.MainMenu) 
			{
				if (GUI.Button (new Rect (0, Screen.height * .2f, Screen.width, Screen.height * .6f), "", GUIStyle.none)) 
				{
					Player.isStop = false;
					SetState (State.GAME);
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.GameMenu).Show();
					GUIPanelManager.currentPanel = PanelType.GameMenu;
					Game.GameStart();
				}
			}
		}
	}
}
