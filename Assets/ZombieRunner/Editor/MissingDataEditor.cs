using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
	[CustomEditor(typeof(MonoBehaviour))]
	public class MissingDataEditor : Editor 
	{
		private static Material mat;

		[MenuItem( "Window/Find Missing Data In Prefabs", priority = 1 )]
		public static void FindMissingMaterialInPrefabs()
		{
			var progressTime = Environment.TickCount;
			
			#region Load all assets in project before searching

			mat = AssetDatabase.LoadAssetAtPath("Assets/Assets/probirka_glass.mat", typeof(Material)) as Material;
			Debug.Log(mat);
			
			var allAssetPaths = AssetDatabase.GetAllAssetPaths();
			for( int i = 0; i < allAssetPaths.Length; i++ )
			{
				
				if( Environment.TickCount - progressTime > 250 )
				{
					progressTime = Environment.TickCount;
					EditorUtility.DisplayProgressBar( "Find Missing Data", "Searching prefabs", (float)i / (float)allAssetPaths.Length );
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
				ObstacleScale(child);
			}
		}

		private static void ObstacleScale(GameObject gameObject) 
		{
			if (gameObject.name == "Human1")
			{
				gameObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				ObstacleScale(child.gameObject);
			}
		}

		private static bool isPrefab( GameObject item )
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
