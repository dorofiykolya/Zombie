/**
 * FacebookSNSTest.cs
 * 
 * A Test script for demonstrating the usage of FacebookSNS Plugin.
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-facebook-sns-plugin to find information 
 * about how to integrate and use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All Rights Reserved. 
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacebookSNSTest : MonoBehaviour {	
	
	private float _buttonWidth =  (Mathf.Min(Screen.height, Screen.width) - 40f) / 3f;	
	private float _buttonHeight = 60f;	
	
	void OnGUI() {	
		
		if (UnityEngine.GUI.Button (new Rect(10f, 10f, _buttonWidth, _buttonHeight), "Post Score")) {		
			FacebookSNS.Instance().PostScore(100);
		}
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 10f, _buttonWidth, _buttonHeight), "Post Message")) {		
			FacebookSNS.Instance().PostMessage("NeatPlug Unity3d Plugins. (MSG " + Random.value.ToString() + ")", 
			                                   "http://www.neatplug.com", 
			                                   "NeatPlug Plugins", 
			                                   "http://www.neatplug.com/img/neatplug-app-icon.png",
			                                   "NeatPlug Unity3d Plugins empower mobile Apps with native operating system features and extended capabilities from third-party services and software.");
		}		
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 10f, _buttonWidth, _buttonHeight), "Post ScreenShot")) {		
			FacebookSNS.Instance().PostScreenShot("ScreenShot Description");
		}
		
		if (UnityEngine.GUI.Button (new Rect(10f, 80f, _buttonWidth, _buttonHeight), "Detect SSO Login Status")) {			 
			FacebookSNS.Instance().DetectSSOLoginStatus();
		}
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 80f, _buttonWidth, _buttonHeight), "Login")) {			
			FacebookSNS.Instance().Login();	
		}	
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 80f, _buttonWidth, _buttonHeight), "Logout")) {			 
			FacebookSNS.Instance().Logout();		
		}		
		
		if (UnityEngine.GUI.Button (new Rect(10f, 150f, _buttonWidth, _buttonHeight), "Invite Friends")) {			
			FacebookSNS.Instance().InviteFriends(null, "Invite Friends", "Come play this awesome game!", false);
		}		
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 150f, _buttonWidth, _buttonHeight), "Send App Request")) {		
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("Key0", "Val0");
			parameters.Add("Key1", "Val1");
			FacebookSNS.Instance().SendAppRequest(null, "App Request", "I've got 10,000 points, can you beat it?", parameters, true);
		}		
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 150f, _buttonWidth, _buttonHeight), "Like a Page")) {	
			FacebookSNS.Instance().LikePage("292542090861944");
		}		
		
		if (UnityEngine.GUI.Button (new Rect(10f, 220f, _buttonWidth, _buttonHeight), "Show Friend Picker\n(Show All, No Filter)")) {			
			FacebookSNS.Instance().ShowFriendPicker(true);
		}
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 220f, _buttonWidth, _buttonHeight), "Show Friend Picker\n(Your App Installed)")) {			 
			FacebookSNS.Instance().ShowFriendPicker(true, 0, FacebookFriendFilter.APP_INSTALLED);
		}
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 220f, _buttonWidth, _buttonHeight), "Show Friend Picker\n(Your App Not Installed)")) {			 
			FacebookSNS.Instance().ShowFriendPicker(true, 0, FacebookFriendFilter.APP_NOT_INSTALLED);
		}	
		
		if (UnityEngine.GUI.Button (new Rect(10f, 290f, _buttonWidth, _buttonHeight), "Post Passing Message")) {		
			FacebookSNS.Instance().PostScorePassingMessage("http://www.neatplug.com/img/neatplug-app-icon.png",
			                                               "http://www.neatplug.com",
			                                               "Come play this game!",
			                                               "player $mname, beats friend $fname's best score in app NeatPlug.",
			                                               "$mname : $mscore, $fname : $fscore.");
		}
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 290f, _buttonWidth, _buttonHeight), "Post Story on Friend's wall\n(With Feed Dialog)")) {		
			FacebookSNS.Instance().PostStoryWithFeedDialog(null, 
			                                               "Come play this game!", 
			                                               "This is really an awesome game", 
			                                               "Description here...",
			                                               "http://www.neatplug.com",
			                                               "http://www.neatplug.com/img/neatplug-app-icon.png",
			                                               null);	
		}
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 290f, _buttonWidth, _buttonHeight), "Post Message via Share Dialog\n(Only availbale on iOS)")) {			 
			FacebookSNS.Instance().PostMessageWithShareDialog();
		}
		
		if (UnityEngine.GUI.Button (new Rect(10f, 360f, _buttonWidth, _buttonHeight), "Share Link via Share Dialog\n(Only availbale on iOS)")) {
			FacebookSNS.Instance().ShareLinkWithShareDialog("http://www.neatplug.com");
		}
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 360f, _buttonWidth, _buttonHeight), "Post Action via Share Dialog\n(Only availbale on iOS)")) {		
			FacebookSNS.Instance().PostActionWithShareDialog("actionType", "propertyName", "propertyValue", null);
		}
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 360f, _buttonWidth, _buttonHeight), "Get Achievements")) {		
			FacebookSNS.Instance().GetAchievements();
		}
		
		if (UnityEngine.GUI.Button (new Rect(10f, 430f, _buttonWidth, _buttonHeight), "Check If Page Liked")) {		
			FacebookSNS.Instance().CheckIfPageLiked("292542090861944");
		}
		
		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 430f, _buttonWidth, _buttonHeight), "Query Leaderboard\nData")) {	
			
			bool includeInactiveFriends = false;
			int limit = 100;
			int pageRows = 20;
			
			List<List<FacebookUserInfo>> pages = FacebookSNS.Instance().QueryLeaderboardData(includeInactiveFriends, limit, pageRows);		
			if (pages != null && pages.Count > 0)
			{
				for (int p = 0; p < pages.Count; p++)
				{				
					List<FacebookUserInfo> friends = pages[p];
					if (friends != null && friends.Count > 0)
					{
						for (int i = 0; i < friends.Count; i++)
						{
							FacebookUserInfo userInfo = friends[i];				
							Debug.Log ("Result of QueryLeaderboardData(" + includeInactiveFriends + ", " + limit + ", " + pageRows + "): pages[" + p + "].friends[" + i + "]: "
							    + "id -> " + userInfo.id 
							    + ", userName -> " + userInfo.userName		
								+ ", firstName -> " + userInfo.firstName
							    + ", middleName -> " + userInfo.middleName
							    + ", lastName -> " + userInfo.lastName
							    + ", name -> " + userInfo.name			           
							    + ", gender -> " + userInfo.gender 
							    + ", email -> " + userInfo.email       
							    + ", locale -> " + userInfo.locale
							    + ", birthday -> " + userInfo.birthday
							    + ", link -> " + userInfo.link
							    + ", country -> " + userInfo.country
							    + ", state -> " + userInfo.state
							    + ", city -> " + userInfo.city 
							    + ", installed -> " + userInfo.installed	
							    + ", profilePicture -> " + userInfo.profilePicture   
							    + ", score -> " + userInfo.score        
							    );
						}
					}					
				}
			}
		}
		
		if (UnityEngine.GUI.Button (new Rect(30f + 2f * _buttonWidth, 430f, _buttonWidth, _buttonHeight), "Query Friends\n(gender = 'male')")) {
			
			string selection = "gender = 'male'";
			List<FacebookUserInfo> friends = FacebookSNS.Instance().QueryFriends(selection);
			if (friends != null)
			{
				for (int i = 0; i < friends.Count; i++)
				{
					FacebookUserInfo userInfo = friends[i];				
					Debug.Log ("Result of QueryFriends(\"" + selection + "\"): friends[" + i + "]: "
					    + "id -> " + userInfo.id 
					    + ", userName -> " + userInfo.userName		
						+ ", firstName -> " + userInfo.firstName
					    + ", middleName -> " + userInfo.middleName
					    + ", lastName -> " + userInfo.lastName
					    + ", name -> " + userInfo.name			           
					    + ", gender -> " + userInfo.gender 
					    + ", email -> " + userInfo.email       
					    + ", locale -> " + userInfo.locale
					    + ", birthday -> " + userInfo.birthday
					    + ", link -> " + userInfo.link
					    + ", country -> " + userInfo.country
					    + ", state -> " + userInfo.state
					    + ", city -> " + userInfo.city 
					    + ", installed -> " + userInfo.installed	
					    + ", profilePicture -> " + userInfo.profilePicture  
					    + ", score -> " + userInfo.score
					    );
				}
			}
		}		
				
		if (UnityEngine.GUI.Button (new Rect(10f, 500f, _buttonWidth, _buttonHeight), "Show Leaderboard")) {		
			FacebookSNS.Instance().ShowLeaderboard(50);
		}

		if (UnityEngine.GUI.Button (new Rect(20f + _buttonWidth, 500f, _buttonWidth, _buttonHeight), "Get Permissions")) {
			List<string> permissions = FacebookSNS.Instance().GetPermissions();
			if (permissions != null)
			{
				for (int i = 0; i < permissions.Count; i++)
				{
					Debug.Log ("Result of GetPermissions(): permissions[" + i + "]: " + permissions[i]);
				}
			}
		}

	}	
	
	// Quit test app when back button is tapped.
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{					
			Application.Quit();
		}	
	}
}
