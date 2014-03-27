using UnityEngine;
using System.Collections;

namespace Runner
{
	public class StateManager : MonoBehaviour
	{
		public delegate void ExitState(State current, State exit);
		public delegate void EnterState(State enter, State exit);
		
		private static StateManager instance;
		private static State currentState;
		private static State previousState = State.NONE;
		public static State Current {get {return currentState;} set {SetState(value);}}
		public static State Previous {get {return previousState;}}
		public static event ExitState OnExitState;
		public static event EnterState OnEnterState;
		public static event System.Action<State> OnChanged;
		
		public State state;
		
		private static void SetState(State value)
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
			instance.state = value;
			if(OnChanged != null)
			{
				OnChanged(value);	
			}
		}
		void Awake()
		{
			instance = this;
		}
		// Use this for initialization
		void Start ()
		{

		}
		
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnGUI ()
		{
			if (currentState == State.LOAD && GUIPanelManager.currentPanel == PanelType.MainMenu) 
			{
				if (GUI.Button (new Rect (0, Screen.height * .2f, Screen.width, Screen.height * .6f), "", GUIStyle.none)) 
				{
					TimerManager.Play ();
					SetState (State.GAME);
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.GameMenu).Show();
					GUIPanelManager.currentPanel = PanelType.GameMenu;
					Manager.Restart();
				}
			}
		}
	}
}
