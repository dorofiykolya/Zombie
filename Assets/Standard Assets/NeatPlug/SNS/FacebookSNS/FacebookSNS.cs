/**
 * FacebookSNS.cs
 * 
 * A Singleton class encapsulating public access methods for Facebook SNS processes.
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-facebook-sns-plugin to find information how 
 * to use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All rights reserved.
 * 
 * @version 1.6.1
 * @sdk_version(android) 3.14
 * @sdk_version(ios) 3.14
 *
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FacebookSNS  {
	
	#region Fields		
		
	private class FacebookSNSNativeHelper : IFacebookSNSNativeHelper {
		
#if UNITY_ANDROID	
		private AndroidJavaObject _plugin = null;
#endif		
		public FacebookSNSNativeHelper()
		{
			
		}
		
		public void CreateInstance(string className, string instanceMethod)
		{	
#if UNITY_ANDROID			
			AndroidJavaClass jClass = new AndroidJavaClass(className);
			_plugin = jClass.CallStatic<AndroidJavaObject>(instanceMethod);	
#endif			
		}
		
		public void Call(string methodName, params object[] args)
		{
#if UNITY_ANDROID			
			_plugin.Call(methodName, args);	
#endif
		}
		
		public void Call(string methodName, string signature, object arg)
		{
#if UNITY_ANDROID			
			var method = AndroidJNI.GetMethodID(_plugin.GetRawClass(), methodName, signature);			
			AndroidJNI.CallObjectMethod(_plugin.GetRawObject(), method, AndroidJNIHelper.CreateJNIArgArray(new object[] {arg}));
#endif			
		}
		
		public ReturnType Call<ReturnType> (string methodName, params object[] args)
		{
#if UNITY_ANDROID			
			return _plugin.Call<ReturnType>(methodName, args);
#else
			return default(ReturnType);
#endif			
		}		
	
	};	
	
	private static FacebookSNS _instance = null;
	
	#endregion
	
	#region Functions
	
	/**
	 * Constructor.
	 */
	private FacebookSNS()
	{	
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().SetNativeHelper(new FacebookSNSNativeHelper());
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance();
#endif
	}
	
	/**
	 * Instance method.
	 */
	public static FacebookSNS Instance()
	{		
		if (_instance == null) 
		{
			_instance = new FacebookSNS();
		}
		
		return _instance;
	}	
	
	/**
	 * Initialize the environment.
	 * 
	 * @param string
	 *            appId - The AppID got from facebook dev site.
	 * 
	 * @param string
	 *            appSecret - The AppSecret got from facebook dev site.
	 *            NOTE: setting appSecret in code is risky. For more information, please visit:
	 * 	          http://www.neatplug.com/integration-guide-unity3d-facebook-sns-plugin
	 *            
	 * @param bool
	 *            autoFetchUserFriends - If set to true, user's friends will be fetched automatically.
	 * 
	 * @param bool
	 *            autoFetchUserLikedPages - If set to true, user's liked pages will be fetched automatically.
	 *            
	 * @param bool
	 *            needUserEmail - If require user's email address. 
	 * 
	 * @param bool 
	 *            needUserDoBInfo - If require user's DoB info.
	 *            
	 * @param bool
	 *            needUserLocationInfo - If require user's location info.
	 * 
	 * @param bool
	 *            needUserLikesInfo - If require user's likes info.
	 * 
	 * @param bool
	 *            autoLoadScores - Automatically load user scores after login.
	 * 
	 * @param bool
	 *            scorePolicyLowerBetter - If set to true, score is the lower the better,
	 *                                     otherwise score policty is the higher the better
	 *                                     (default), which means that only a higher score 
	 *                                     will be posted.
	 * 
	 * @param string
	 *            serverReceiveScoreURL - The URL to receive the score on an external server.
	 *                                    This is intended to increase security, because you don't
	 *                                    need to supply AppSecret if you do posting score on your 
	 *                                    own server, AppSecret will no longer exist in your 
	 *                                    client binary then.
	 * 
	 * @param string
	 *            serverReceiveAchievementURL - The URL to receive the achievementURL on an external server.
	 *                                    This is intended to increase security, because you don't
	 *                                    need to supply AppSecret if you do posting achievement on your 
	 *                                    own server, AppSecret will no longer exist in your 
	 *                                    client binary then.
	 * 	
	 */
	public void Init(string appId, string appSecret, bool autoFetchUserFriends, bool autoFetchUserLikedPages, 
	                   bool needUserEmail, bool needUserDoBInfo, bool needUserLocationInfo, bool needUserLikesInfo,
	                   bool autoLoadScores, bool scorePolicyLowerBetter, bool debugMode,
	                   string serverReceiveScoreURL, string serverReceiveAchievementURL)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().Init(appId, appSecret, autoFetchUserFriends, autoFetchUserLikedPages, 
		                                   needUserEmail, needUserDoBInfo, needUserLocationInfo, needUserLikesInfo,
		                                   autoLoadScores, scorePolicyLowerBetter, debugMode,
		                                   serverReceiveScoreURL, serverReceiveAchievementURL);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().Init(appId, appSecret, autoFetchUserFriends, autoFetchUserLikedPages, 
		                                   needUserEmail, needUserDoBInfo, needUserLocationInfo, needUserLikesInfo,
		                                   autoLoadScores, scorePolicyLowerBetter, debugMode,
		                                   serverReceiveScoreURL, serverReceiveAchievementURL);
#endif		
	}
	
	/**
	 * Login the user.	
	 * 
	 * This will show up a facebook login box if there's no cached SSO facebook session, or
	 * simply load the session from cache without user interaction.
	 * 
	 * You may need to call this function if you want the user to login whenever you want, 
	 * instead of logging in at the time when posting a score, message or screenshot.
	 * 
	 * A usecase is that you let the user login every time your game starts (If the user has
	 * not yet logged in yet, neither through your App nor facebook's official App).
	 * 
	 */
	public void Login()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().Login();		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().Login();
#endif			
	}
	
	/**
	 * Log out the user.
	 */
	public void Logout()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().Logout();		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().Logout();
#endif			
	}	
	
	/**
	 * Post a score for the user to your App.
	 * 
	 * Only the higher score will be posted when score policy is set to "The_Higher_The_Better", 
	 * or only the lower score will be posted when score policy is set to "The_Lower_The_Better".
	 * 
	 * @param score
	 *          int - The score to be posted.
	 * 
	 */
	public void PostScore(int score)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostScore(score);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostScore(score);
#endif			
	}	
	
	/**
	 * Get the scores information about the user and his/her friends who
	 * have played your game.
	 * 
	 * Scores information would be useful if you want to build a leaderboard
	 * among the user and his/her friends.
	 * 
	 * Generally you don't have to call this method explicitly if you have
	 * "Auto Load User Scores" set to true on FacebookSNSAgent gameObject.
	 * If you really need to call this manually, e.g. for refreshing the 
	 * leaderboard, make sure this function gets called after 
	 * FacebookSNSListener::OnUserInfoArrived() event triggered, 
	 * since some user information will be needed to make this call.	 
	 */	
	public void GetScores()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().GetScores();		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().GetScores();
#endif	
	}
	
	/**
	 * Post an achievement for the user to your App.	 
	 *
	 * @param achivementUrl
	 *          string - The URL of specified achievement page.
	 *                   For more information about how to set up an achievement, please refer to
	 *                   Facebook dev site: https://developers.facebook.com/docs/howtos/achievements/
	 */
	public void PostAchievement(string achievementUrl)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostAchievement(achievementUrl);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostAchievement(achievementUrl);
#endif
	}	

	/**
	 * Capture and post the screenshot of your App on the user's wall.
	 * 
	 * NOTE: Screenshot capturing is having issue on Android with Unity3.4.x, 
	 * you may want to upgrade to Unity4 to get it work. That is a Unity bug.
	 * 	
	 */
	public void PostScreenShot()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostScreenShot(null);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostScreenShot(null);
#endif
	}
	
	/**
	 * Capture and post the screenshot of your App on the user's wall.
	 * 
	 * NOTE: Screenshot capturing is having issue on Android with Unity3.4.x, 
	 * you may want to upgrade to Unity3.5 or Unity4 to get it work. That is a Unity bug.
	 * 
	 * @param description
	 *            string - Description for the screenshot.
	 * 	
	 * 
	 */
	public void PostScreenShot(string description)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostScreenShot(description);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostScreenShot(description);
#endif
	}	
	
	/**
	 * Post an image on the user's wall.
	 * 
	 * @param imgData
	 *           byte[] - The byte array of the image data.	
	 * 	
	 */
	public void PostImage(byte[]imgData)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostImage(imgData, null);	
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostImage(imgData, null);
#endif
	}		
	
	/**
	 * Post an image on the user's wall.
	 * 
	 * @param imgData
	 *           byte[] - The byte array of the image data.
	 * 
	 * @param description
	 *           string - The description of the image.
	 * 	
	 */
	public void PostImage(byte[]imgData, string description)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostImage(imgData, description);	
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostImage(imgData, description);
#endif
	}		
	
	/**
	 * Post a message on the user's feed, with or without a link.
	 * 
	 * @param message
	 *           string - The message to be posted.
	 * 
	 * @param link
	 *           string - The link to be posted.
	 */
	public void PostMessage(string message, string link)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostMessage(message, link, null, null, null);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostMessage(message, link, null, null, null);	
#endif
	}
	
	/**
	 * Post a message on the user's feed, with fully exposed parameters.
	 * 
	 * This function is useful if you want to share a link with detailed information,
	 * generally you can use this to post for promotional purpose.
	 * 
	 * @param message
	 *          string - The message.
	 * 
	 * @param link
	 *          string - The link.
	 * 
	 * @param linkName
	 *          string - The name of the link.
	 * 
	 * @param pictureURL
	 *          string - The url of the picture.
	 * 
	 * @param description
	 *          string - The description.
	 */
	public void PostMessage(string message, string link, string linkName, 
		                    string pictureURL, string description)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostMessage(message, link, linkName, pictureURL, description);		
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostMessage(message, link, linkName, pictureURL, description);
#endif
	}	
	
	/**
	 * Make a graph request.
	 * 
	 * @param requestId
	 *            string - The ID for identifying a request.(Arbitrary unique string)
	 *                     The requestId will be passed back to your App via event
	 *                     FacebookSNSListener::OnRequestGraphResultArrived().
	 * 
	 * @param httpMethod
	 *            string - HTTP method of the request.
	 * 
	 * @param graphPath
	 *            string - The graph path to be requested.
	 * 
	 * @param graphParams
	 *            Dictionary<string, string> - The parameteres attached for the request.
	 * 
	 */
	public void RequestGraph(string requestId, string httpMethod, string graphPath, 
	                         Dictionary<string, string> graphParams)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().RequestGraph(requestId, httpMethod, graphPath, graphParams);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().RequestGraph(requestId, httpMethod, graphPath, graphParams);
#endif
	}
	
	/**
	 * Get the user's access token.
	 * 
	 * Generally you don't need the access token unless you want
	 * to make direct graph calls that may need it.
	 */
	public string GetUserAccessToken()
	{
		string result = null;
#if UNITY_ANDROID		
		result = FacebookSNSAndroid.Instance().GetUserAccessToken();
#endif	
#if UNITY_IPHONE
		result = FacebookSNSIOS.Instance().GetUserAccessToken();
#endif		
		return result;
	}
	
	/**
	 * Get the App's access token.
	 * 
	 * Generally you don't need the access token unless you want
	 * to make direct graph calls that may need it.
	 */
	public string GetAppAccessToken()
	{
		string result = null;
#if UNITY_ANDROID		
		result = FacebookSNSAndroid.Instance().GetAppAccessToken();
#endif	
#if UNITY_IPHONE
		result = FacebookSNSIOS.Instance().GetAppAccessToken();
#endif	
		return result;
	}
	
	/**
	 * Invite friends to App.
	 * 
	 * @param friendIds
	 *           List<string> - The list of friend IDs. If null supplied,
	 *                          the invitation will be sent to all friends
	 *                          of the user.
	 * 
	 * @param title
	 *           string - Titile to show on the invitation request dialog.
	 * 
	 * @param message
	 *           string - Message to show in the invitation request detail.
	 * 
	 * @param frictionless
	 *           bool - True for frictionless enabled, false for not.
	 *                  Enabling frictionless mode for a invitation request
	 *                  is intended to send the request directly to recipients
	 *                  without any user interactions.
	 */	
	public void InviteFriends(List<string> friendIds, string title, string message, 
	                          bool frictionless)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().InviteFriends(friendIds, title, message, frictionless);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().InviteFriends(friendIds, title, message, frictionless);
#endif
	}
	
	/**
	 * Send an App request to specified recipients.
	 * 
	 * @param recipientIds
	 *           List<string> - The list of recipient IDs.
	 *                          Leave this param null to send app request to all
	 *                          friends of the user.
	 * 
	 * @param title
	 *           string - Titile to show on the app request dialog.
	 * 
	 * @param message
	 *           string - Message to show in the app request detail.
	 * 
	 * @param data
	 *           Dictionary<string, string> - Key-value pairs for the extra params 
	 *                                       attached to the request. This can be used 
	 *                                       to carry the contextual data of your game.
	 *                                       e.g. a challenge score.
	 * 
	 * @param frictionless
	 *           bool - True for frictionless enabled, false for not.
	 *                  Enabling frictionless mode for an app request
	 *                  is intended to send the request directly to recipients
	 *                  without any user interactions.
	 */
	public void SendAppRequest(List<string> recipientIds, string title, string message, 
		                       Dictionary<string, string> data, bool frictionless)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().SendAppRequest(recipientIds, title, message, data, frictionless);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().SendAppRequest(recipientIds, title, message, data, frictionless);
#endif
	}
	
	/**
	 * Get the profile picture of the specified user.
	 * 
	 * This function requires a userId, can be either the user's ID or his / her friend's ID.
	 * You will need to call this function in OnUserInfoArrived() to get current user's
	 * profile picture, or call it in OnUserFriendsArrived() to get some friend's profile
	 * picture, because the userId can be obtained there.
	 * 
	 * The picture url will be sent to you via FacebookSNSListener::OnUserProfilePictureArrived().
	 * 
	 * @param userId
	 *          string - The ID of the user.	
	 */
	public void GetUserProfilePicture(string userId)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().GetUserProfilePicture(userId, 0, 0);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().GetUserProfilePicture(userId, 0, 0);
#endif
	}
	
	/**
	 * Get the user's friends info.
	 * 
	 * The friends info will be sent to your App via FacebookSNSListener::OnUserFriendsArrived().
	 * 
	 * Generally you don't have to call this method explicitly if you have
	 * "Auto Fetch User Friends" set to true on FacebookSNSAgent gameObject.
	 * If you really need to call this manually, make sure this function gets 
	 * called after FacebookSNSListener::OnUserLoggedIn() event triggered, 
	 * since a valid session will be needed to make this call.	 
	 */
	public void GetUserFriends()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().GetUserFriends();
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().GetUserFriends();
#endif	
	}
	
	/**
	 * Get the user's liked pages.
	 * 
	 * The pages info will be sent to your App via FacebookSNSListener::OnUserLikedPagesArrived().
	 * 
	 * Generally you don't have to call this method explicitly if you have
	 * "Auto Fetch User Liked Pages" set to true on FacebookSNSAgent gameObject.
	 * If you really need to call this manually, make sure this function gets 
	 * called after FacebookSNSListener::OnUserLoggedIn() event triggered, 
	 * since a valid session will be needed to make this call.	
	 */
	public void GetUserLikedPages()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().GetUserLikedPages();
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().GetUserLikedPages();
#endif	
	}
	
	/**
	 * Detect the SSO Login status.
	 * 
	 * This is very useful to get the user's login status from Facebook App or the browser.
	 * Getting the "external" login status is important since there might be session permission
	 * change or user's password change outside that invalidated the current cached session.
	 * your App may want to know this external login status to render a "Connect to Facbook"
	 * button in case the user's connection to facebook is broken, instead of calling Login() 
	 * that will bring up a login UI at the point.
	 * 
	 * The status result will be sent to your App via FacebookSNSListener::OnSSOLoginStatusArrived().
	 *
	 */
	public void DetectSSOLoginStatus() 
	{			
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().DetectSSOLoginStatus();
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().DetectSSOLoginStatus();
#endif		
	}	
	
	/**
	 * Have the user like a page.
	 * 
	 * You App will get notified once the page is successfully liked by the user, via 
	 * FacebookSNSListener::OnPageLiked() event.
	 * 
	 * @param pageId
	 *          string - The page ID to be liked.
	 */
	public void LikePage(String pageId)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().LikePage(pageId);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().LikePage(pageId);
#endif	
	}	
	
	/**
	 * Get the profile picture of the specififed user.
	 * 
	 * This function requires a userId, can be either the user's ID or his / her friend's ID.
	 * You will need to call this function in OnUserInfoArrived() to get current user's
	 * profile picture, or call it in OnUserFriendsArrived() to get some friend's profile
	 * picture, because the userId can be obtained there.
	 * 
	 * The picture url will be sent to you via FacebookSNSListener::OnUserProfilePictureArrived().
	 * 
	 * @param userId
	 *          string - The ID of the user.
	 * 
	 * @param width
	 *          int - The width of the picture (in pixels).
	 *                NOTE: facebook will return the closest valid size picture.
	 * 
	 * @param height
	 *          int - The height of the picture (in pixels).
	 *                NOTE: facebook will return the closest valid size picture.
	 */
	public void GetUserProfilePicture(string userId, int width, int height)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().GetUserProfilePicture(userId, width, height);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().GetUserProfilePicture(userId, width, height);
#endif
	}	
	
	/**
	 * Show a friend picker dialog.
	 * 
	 * The result will be sent to your App via FacebookSNSListener::OnFriendPickerDismissed().
	 * 
	 * @param multiSelect
	 *           bool - True for multiSelect, false for singleSelect.
	 */
	public void ShowFriendPicker(bool multiSelect)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().ShowFriendPicker(multiSelect, 0, FacebookFriendFilter.ALL);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().ShowFriendPicker(multiSelect, 0, FacebookFriendFilter.ALL);
#endif	
	}
	
	/**
	 * Show a friend picker dialog.
	 * 
	 * The result will be sent to your App via FacebookSNSListener::OnFriendPickerDismissed().
	 * 
	 * @param multiSelect
	 *           bool - True for multiSelect, false for singleSelect.
	 * 
	 * @param maxSelected
	 *           int - The max number of friends that can be selected in the picker.
	 */
	public void ShowFriendPicker(bool multiSelect, int maxSelected)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().ShowFriendPicker(multiSelect, maxSelected, FacebookFriendFilter.ALL);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().ShowFriendPicker(multiSelect, maxSelected, FacebookFriendFilter.ALL);
#endif	
	}
	
	/**
	 * Show a friend picker dialog.
	 * 
	 * The result will be sent to your App via FacebookSNSListener::OnFriendPickerDismissed().
	 * 
	 * @param multiSelect
	 *           bool - True for multiSelect, false for singleSelect.
	 * 
	 * @param maxSelected
	 *           int - The max number of friends that can be selected in the picker.
	 * 
	 * @param filter
	 *           FacebookFriendFilter - The filter for showing filtered friends in the picker.
	 */
	public void ShowFriendPicker(bool multiSelect, int maxSelected, FacebookFriendFilter filter)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().ShowFriendPicker(multiSelect, maxSelected, filter);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().ShowFriendPicker(multiSelect, maxSelected, filter);
#endif	
	}
	
	/**
	 * Tag a friend in an image.
	 * 
	 * To tag a friend in the image, first you need to post an image to facebook by calling
	 * PostImage(), and then get the returned facebook photoID in 
	 * FacebookSNSListener::OnImagePosted().
	 * 
	 * @param photoId
	 *          String - The photoId from Facebook.	                 
	 *          
	 * @param userId
	 *          String - The friend's userId to be tagged.
	 *          
	 * @param x
	 *         int - Percentage from the left edge of the image.
	 *  
	 * @param y
	 *         int - Percentage from the top edge of the image.
	 *    
	 */
	public void TagFriendInImage(string photoId, string userId, int x, int y)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().TagFriendInImage(photoId, userId, x, y);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().TagFriendInImage(photoId, userId, x, y);
#endif
	}
	
	/**
	 * Request new read permissions.
	 *
	 * Your App will get notified via FacebookSNSListener::OnNewReadPermissionsGranted() if 
	 * the permissions are successfully granted, or 
	 * FacebookSNSListener::OnFailedToRequestNewReadPermissions when failed to grant permissions.
	 * 
	 * @param permissions
	 *           string[] - The permissions to be requested.
	 */
	public void RequestNewReadPermissions(string[] permissions)
	{	
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().RequestNewReadPermissions(permissions);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().RequestNewReadPermissions(permissions);
#endif		
	}
	
	/**
	 * Post a score passing message on the user's feed.
	 * 
	 * @param pictureURL
	 *            string - The picture URL of your App icon.
	 * 
	 * @param link
	 *            string - Your App download link.
	 * 
	 * @param linkName
	 *            string - The link name.
	 *                     An example link name:
	 *                     Come play the game!
	 * 
	 * @param message
	 *            string - The message, can contains variables.
	 *                     Variables: $fname - Friend's name, $mname - My name,
	 *                                $fscore - Friend's score, $mscore - My score.
	 *                     An example message:
	 *                     player $mname, beats friend $fname's best score in app xx.
	 * 
	 * @param description
	 *            string - The description, can contains variables.
	 *                     Variables: $fname - Friend's name, $mname - My name,
	 *                                $fscore - Friend's score, $mscore - My score. 
	 *                     An example description:
	 *                     $mname : $mscore, $fname : $fscore.
	 */
	public void PostScorePassingMessage(string pictureURL, string link, string linkName,
			string message, string description)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostScorePassingMessage(pictureURL, link, linkName, 
		                                                      message, description);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostScorePassingMessage(pictureURL, link, linkName, 
		                                                  message, description);
#endif		
	}
	
	/**
	 * Post a story with a Feed Dialog.
	 * 
	 * This call will open a Facebook native Feed Dialog for the user to input / change the content of
	 * the story that is about to be posted. The story can be posted on the user's own wall or any friend's
	 * wall. Currently this is the only approach to post on friend's timeline since Facebook has rejected
	 * directly posting to friend's wall using Graph API without user interaction.
	 * 
	 * @param friendId
	 *         string - The friend ID, (or leave it null to post on your own wall) of which you want the 
	 *                  story to be posted on the wall.
	 * 
	 * @param name
	 *         string - The name of the story.
	 * 
	 * @param caption
	 *         string - The caption of the story.
	 * 
	 * @param description
	 *         string - The description of the story.
	 * 
	 * @param link
	 *         string - A link you want to share.
	 * 
	 * @param pictureURL
	 *         string - A URL of a picture, that you want to show in the story.
	 * 
	 * @param sourceURL
	 *         string - A URL of a media file, (mp3, swf, etc...) that you want to show in the story.
	 *                  NOTE: If you provide both pictureURL and sourceURL, the pictureURL will be ignored.
	 * 
	 */
	public void PostStoryWithFeedDialog(string friendId, string name, string caption, string description, 
		                                string link, string pictureURL, string sourceURL)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().PostStoryWithFeedDialog(friendId, name, caption, description, 
		                                                      link, pictureURL, sourceURL);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostStoryWithFeedDialog(friendId, name, caption, description, 
		                                                      link, pictureURL, sourceURL);
#endif		
	}
	
	/**
	 * Post a message with the Share Dialog.
	 * 
	 * NOTE: This feature is available on iOS only.
	 */
	public void PostMessageWithShareDialog()
	{
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostMessageWithShareDialog();
#endif		
	}
	
	/**
	 * Share a link with the Share Dialog.
	 * 
	 * NOTE: This feature is available on iOS only.
	 * 
	 * @param link
	 *         string - The link to be shared.
	 */
	public void ShareLinkWithShareDialog(string link)
	{
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().ShareLinkWithShareDialog(link);
#endif		
	}	
	
	/**
	 * Post an action with the Share Dialog.
	 * 
	 * NOTE: This feature is available on iOS only.
	 * 
	 * @param actionType
	 *            string - The action type.
	 * 
	 * @param propertyName
	 *            string - The action property name.
	 * 
	 * @param propertyValue
	 *            string - The action property value.
	 * 
	 * @param imgBytes
	 *            byte[] - The image data.
	 */
	public void PostActionWithShareDialog(string actionType, string propertyName, string propertyValue, byte[] imgBytes)
	{
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().PostActionWithShareDialog(actionType, propertyName, propertyValue, imgBytes);
#endif			
	}	
	
	/**
	 * Get the achievements data for the user.
	 */
	public void GetAchievements()
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().GetAchievements();
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().GetAchievements();
#endif		
	}
	
	/**
	 * Check if a page is liked by the user.
	 * 
	 * The result will be returned via FacebookSNSListener::OnCheckPageLikedResultArrived().
	 * 
	 * @param pageId
	 *           string - The ID of the page.
	 */
	public void CheckIfPageLiked(string pageId)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().CheckIfPageLiked(pageId);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().CheckIfPageLiked(pageId);
#endif		
	}
	
	/**
	 * Query friends with conditions for selection (SQL Syntax).
	 * 
	 * The supported fields of the friend records are listed below:
	 * id, username, first_name, middle_name, last_name, name, gender, locale, birthday, 
	 * link, country, state, city, email, installed, picture, score.
	 * 
	 * Please note that some fields may be empty because of the protection of friends' 
	 * sensitive information.
	 * 
	 * @param selection
	 *             string - The "WHERE" subclause (without "WHERE") of a SQL query. For example:
	 *                   "gender = 'male' and installed = 1" 
	 *                   (This filters all male friends who have installed your App)
	 * 
	 * @return List<FacebookUserInfo> - The query result, a list of friends.
	 */                   
	public List<FacebookUserInfo> QueryFriends(string selection)
	{
		List<FacebookUserInfo> result = new List<FacebookUserInfo>();
#if UNITY_ANDROID		
		result = FacebookSNSAndroid.Instance().QueryFriends(selection);
#endif	
#if UNITY_IPHONE
		result = FacebookSNSIOS.Instance().QueryFriends(selection);
#endif	
		return result;
	}
	
	/**
	 * Query the data that is needed for building a leaderboard.
	 * 
	 * @param includeInactiveFriends
	 *             bool - True for including inactive friends, false for excluding them.
	 *                    (Inactive friends are those have been removed from the user's friend list)
	 * 
	 * @param limit
	 *             int - The maximum number of total rows to be retrieved. If it is set to 0, stands for 
	 *                   no limit.
	 * 
	 * @param pageRows
	 *             int - The maximum number of rows in a single page, 0 stands for no limit.
	 * 
	 * @return List<List<FacebookUserInfo>> - A list of pages of friends that have been sorted by the 
	 *                                        score order specified by "Score Policy" property of the 
	 *                                        FacebookSNSAgent gameObject.
	 * 
	 * The supported fields of the returned friends are listed below:
	 * id, username, first_name, middle_name, last_name, name, gender, locale, birthday, 
	 * link, country, state, city, email, installed, picture, score.	            
	 */                   
	public List<List<FacebookUserInfo>> QueryLeaderboardData(bool includeInactiveFriends, int limit, int pageRows)
	{
		List<List<FacebookUserInfo>> result = new List<List<FacebookUserInfo>>();
#if UNITY_ANDROID		
		result = FacebookSNSAndroid.Instance().QueryLeaderboardData(includeInactiveFriends, limit, pageRows);
#endif	
#if UNITY_IPHONE
		result = FacebookSNSIOS.Instance().QueryLeaderboardData(includeInactiveFriends, limit, pageRows);
#endif	
		return result;
	}
	
	/**
	 * Show the leaderboard.
	 * 
	 * This is a convenient way to display a leaderboard. 
	 * You don't need to handle any leaderboard related events, just
	 * a single call will show the built-in native-style leaderboard
	 * to the user.
	 * 
	 * @param limit
	 *           int - The max number of rows in the leaderboard.
	 *                 (0 for no limit).
	 */
	public void ShowLeaderboard(int limit)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().ShowLeaderboard(limit, null);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().ShowLeaderboard(limit, null);
#endif		
	}
	
	/**
	 * Show the leaderboard.
	 * 
	 * This is a convenient way to display a leaderboard. 
	 * You don't need to handle any leaderboard related events, just
	 * a single call will show the built-in native-style leaderboard
	 * to the user.
	 * 
	 * @param limit
	 *           int - The max number of rows in the leaderboard.
	 *                 (0 for no limit).
	 * 
	 * @param title
	 *           string - The title of the leaderboard. If pass a null
	 *                    or empty string, a default value "Leaderboard"
	 *                    will be used.
	 */
	public void ShowLeaderboard(int limit, string title)
	{
#if UNITY_ANDROID		
		FacebookSNSAndroid.Instance().ShowLeaderboard(limit, title);
#endif	
#if UNITY_IPHONE
		FacebookSNSIOS.Instance().ShowLeaderboard(limit, title);
#endif		
	}

	/**
	 * Get the granted permissions in current session.
	 * 
	 * @return List<string> - A list of permissions that have been granted in current session.
	 */                   
	public List<string> GetPermissions()
	{
		List<string> result = new List<string>();
#if UNITY_ANDROID		
		result = FacebookSNSAndroid.Instance().GetPermissions();
#endif	
#if UNITY_IPHONE
		result = FacebookSNSIOS.Instance().GetPermissions();
#endif	
		return result;
	}
	
	#endregion
}
