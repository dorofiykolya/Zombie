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
				UpBonus(child);
			}
		}


		private static void GemRigidbody(GameObject gameObject) 
		{
			if (gameObject.name == "probirka") 
			{
				gameObject.AddComponent("LocationObject");
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				GemRigidbody(child.gameObject);
			}
		}

		private static void GemTagChange(GameObject gameObject)
		{
			if (gameObject.name == "gem") 
			{
				gameObject.layer = LayerMask.NameToLayer("Default"); 
			}

			foreach (Transform child in gameObject.transform) 
			{
				GemTagChange(child.gameObject);
			}
		}

		private static void UpBonus(GameObject gameObject) 
		{
			if (gameObject.CompareTag("Currency")) 
			{
				if(gameObject.transform.position.x > -4 && gameObject.transform.position.x < 4)
				{
					gameObject.transform.position = new Vector3(0,
					                                            gameObject.transform.position.y,
					                                            gameObject.transform.position.z);
				}
				else if(gameObject.transform.position.x >= 4)
				{
					gameObject.transform.position = new Vector3(8,
					                                            gameObject.transform.position.y,
					                                            gameObject.transform.position.z);
				}
				else if(gameObject.transform.position.x <= -4)
				{
					gameObject.transform.position = new Vector3(-8,
					                                            gameObject.transform.position.y,
					                                            gameObject.transform.position.z);
				}
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				UpBonus(child.gameObject);
			}
		}

		private static void FindMissingMaterial(GameObject gameObject) 
		{
			if(gameObject.renderer != null)
			{
				if (gameObject.renderer.sharedMaterial == null) 
				{
					gameObject.renderer.sharedMaterial = mat;
				}
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				FindMissingMaterial(child.gameObject);
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
