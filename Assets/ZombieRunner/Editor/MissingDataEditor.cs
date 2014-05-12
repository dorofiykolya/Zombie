﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
	public class MissingDataEditor : EditorWindow 
	{
		private Animation death;
		private string name;

		void OnGUI()
		{
			death = EditorGUILayout.ObjectField("Death", death, typeof(Animation)) as Animation;
			name = EditorGUILayout.TextField("Name", name);

			if (death != null)
			{
				if (GUILayout.Button("ADD"))
				{
					AddAnimation();
				}
			}
		}

		private void AddAnimation()
		{
			var progressTime = Environment.TickCount;
			
			#region Load all assets in project before searching
			
			var allAssetPaths = AssetDatabase.GetAllAssetPaths();
			for( int i = 0; i < allAssetPaths.Length; i++ )
			{
				
				if( Environment.TickCount - progressTime > 250 )
				{
					progressTime = Environment.TickCount;
					EditorUtility.DisplayProgressBar( "Prefabs Adjustment", "Searching prefabs", (float)i / (float)allAssetPaths.Length );
				}
				
				AssetDatabase.LoadMainAssetAtPath( allAssetPaths[ i ] );
				
			}
			
			EditorUtility.ClearProgressBar();
			
			#endregion
			
			var prefabs = Resources
				.FindObjectsOfTypeAll( typeof( GameObject ) )
					.Cast<GameObject>()
					.Where( x => x.transform.parent == null && isPrefab( x ) )
					.OrderBy( x => x.name )
					.ToList();
			
			foreach(GameObject child in prefabs)
			{
				Adjustment(child);
			}
		}

		[MenuItem("Runner/Prefabs Adjustment")]
		public static void ShowShaderQualityWindowEditor()
		{
			EditorWindow.GetWindow<MissingDataEditor>("Prefabs Adjustment", true).Show();
		}

		private void Adjustment(GameObject gameObject) 
		{
			if (gameObject.CompareTag("Human"))
			{
				gameObject.collider.isTrigger = false;
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				Adjustment(child.gameObject);
			}
		}

		private bool isPrefab( GameObject item )
		{
			if( item == null )
				return false;
			
			return
				item != null &&
					PrefabUtility.GetPrefabParent( item ) == null &&
					PrefabUtility.GetPrefabObject( item ) != null;
		}
	}
}
