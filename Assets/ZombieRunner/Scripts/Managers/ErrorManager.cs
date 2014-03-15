using UnityEngine;
using System.Collections;
//using UnityEditor;
namespace Runner
{
	public class ErrorManager
	{
		public static bool HasError{get;private set;}
//		private static ErrorWindow window;
		
		public static void Show(string title = "", string message = "")
		{
//			if(Application.isEditor)
//			{
//				if(window == null)
//				{
//					window = (ErrorWindow)EditorWindow.CreateInstance(typeof(ErrorWindow));	
//				}
//				window.Show();
//				window.message = message;
//				window.title = title;
				HasError = true;
//			}
		}
		
	}
	
//	public class ErrorWindow : UnityEditor.EditorWindow
//	{
//		public string message;
//		void OnGUI () 
//		{
//			if(message != null)
//			{
//				GUILayout.Label (message, EditorStyles.boldLabel);
//			}
//		}
//	}
}

