/**
 * FacebookSNSListener.cs
 * 
 * FacebookSNSListener listens to the Facebook SNS events.
 * File location: Assets/Scripts/NeatPlug/SNS/FacebookSNS/FacebookSNSListener.cs
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-facebook-sns-plugin to find information 
 * about how to integrate and use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All Rights Reserved.
 * 
 * @version 1.6.1
 * @sdk_version(android) 3.14
 * @sdk_version(ios) 3.14
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacebookSNSListener : MonoBehaviour {
	
	private bool _debug = false;	
	
	private static bool _instanceFound = false;
	
	void Awake()
	{		
		if (_instanceFound)
		{
			Destroy(gameObject);
			return;
		}
		_instanceFound = true;
		
		// The gameObject will retain...
		DontDestroyOnLoad(this);
		
		GameObject go = GameObject.Find("FacebookSNSAgent");
		if (go != null)
		{
			_debug = go.GetComponent<FacebookSNSAgent>().debugMode;
		}
		
		FacebookSNS.Instance();		
	}
	
	void OnEnable()
	{
		// Register the events.
		// Do not modify the codes below.		
		FacebookSNSAgent.OnUserLoggedIn += OnUserLoggedIn;
		FacebookSNSAgent.OnUserFailedToLogIn += OnUserFailedToLogIn;
		FacebookSNSAgent.OnUserLoggedOut += OnUserLoggedOut;
		FacebookSNSAgent.OnScorePosted += OnScorePosted;
		FacebookSNSAgent.OnFailedToPostScore += OnFailedToPostScore;
		FacebookSNSAgent.OnAchievementPosted += OnAchievementPosted;
		FacebookSNSAgent.OnFailedToPostAchievement += OnFailedToPostAchievement;
		FacebookSNSAgent.OnScreenShotPosted += OnScreenShotPosted;
		FacebookSNSAgent.OnFailedToPostScreenShot += OnFailedToPostScreenShot;		
		FacebookSNSAgent.OnImagePosted += OnImagePosted;
		FacebookSNSAgent.OnFailedToPostImage += OnFailedToPostImage;		
		FacebookSNSAgent.OnMessagePosted += OnMessagePosted;	
		FacebookSNSAgent.OnFailedToPostMessage += OnFailedToPostMessage;
		FacebookSNSAgent.OnScoresDataArrived += OnScoresDataArrived;
		FacebookSNSAgent.OnFailedToGetScoresData += OnFailedToGetScoresData;
		FacebookSNSAgent.OnUserInfoArrived += OnUserInfoArrived;
		FacebookSNSAgent.OnFailedToGetUserInfo += OnFailedToGetUserInfo;
		FacebookSNSAgent.OnUserFriendsArrived += OnUserFriendsArrived;
		FacebookSNSAgent.OnFailedToGetUserFriends += OnFailedToGetUserFriends;
		FacebookSNSAgent.OnRequestGraphResultArrived += OnRequestGraphResultArrived;
		FacebookSNSAgent.OnFailedToRequestGraph += OnFailedToRequestGraph;
		FacebookSNSAgent.OnFriendsInvited += OnFriendsInvited;
		FacebookSNSAgent.OnFailedToInviteFriends += OnFailedToInviteFriends;		
		FacebookSNSAgent.OnAppRequestSent += OnAppRequestSent;
		FacebookSNSAgent.OnFailedToSendAppRequest += OnFailedToSendAppRequest;		
		FacebookSNSAgent.OnAppRequestReceived += OnAppRequestReceived;
		FacebookSNSAgent.OnFailedToReceiveAppRequest += OnFailedToReceiveAppRequest;		
		FacebookSNSAgent.OnUserProfilePictureArrived += OnUserProfilePictureArrived;
		FacebookSNSAgent.OnFailedToGetUserProfilePicture += OnFailedToGetUserProfilePicture;
		FacebookSNSAgent.OnSSOLoginStatusArrived += OnSSOLoginStatusArrived;
		FacebookSNSAgent.OnPageLiked += OnPageLiked;
		FacebookSNSAgent.OnUserLikedPagesArrived += OnUserLikedPagesArrived;
		FacebookSNSAgent.OnFailedToGetUserLikedPages += OnFailedToGetUserLikedPages;
		FacebookSNSAgent.OnFriendPickerDismissed += OnFriendPickerDismissed;
		FacebookSNSAgent.OnFriendTaggedInImage += OnFriendTaggedInImage;
		FacebookSNSAgent.OnFailedToTagFriendInImage += OnFailedToTagFriendInImage;
		FacebookSNSAgent.OnNewReadPermissionsGranted += OnNewReadPermissionsGranted;
		FacebookSNSAgent.OnFailedToRequestNewReadPermissions += OnFailedToRequestNewReadPermissions;
		FacebookSNSAgent.OnScorePassingMessagePosted += OnScorePassingMessagePosted;
		FacebookSNSAgent.OnFailedToPostScorePassingMessage += OnFailedToPostScorePassingMessage;
		FacebookSNSAgent.OnStoryPostedWithFeedDialog += OnStoryPostedWithFeedDialog;
		FacebookSNSAgent.OnFailedToPostStoryWithFeedDialog += OnFailedToPostStoryWithFeedDialog;		
		/**
		 * ---------------------------------------------------------------------------------------------------------------
		 * @since 1.5.0
		 * ---------------------------------------------------------------------------------------------------------------
		 */
		FacebookSNSAgent.OnMessagePostedWithShareDialog += OnMessagePostedWithShareDialog;
		FacebookSNSAgent.OnFailedToPostMessageWithShareDialog += OnFailedToPostMessageWithShareDialog;
		FacebookSNSAgent.OnLinkSharedWithShareDialog += OnLinkSharedWithShareDialog;
		FacebookSNSAgent.OnFailedToShareLinkWithShareDialog += OnFailedToShareLinkWithShareDialog;
		FacebookSNSAgent.OnActionPostedWithShareDialog += OnActionPostedWithShareDialog;
		FacebookSNSAgent.OnFailedToPostActionWithShareDialog += OnFailedToPostActionWithShareDialog;
		FacebookSNSAgent.OnAchievementsDataArrived += OnAchievementsDataArrived;
		FacebookSNSAgent.OnFailedToGetAchievementsData += OnFailedToGetAchievementsData;
		FacebookSNSAgent.OnCheckPageLikedResultArrived += OnCheckPageLikedResultArrived;
		FacebookSNSAgent.OnFailedToCheckPageLiked += OnFailedToCheckPageLiked;
	}

	void OnDisable()
	{
		// Unregister the events.
		// Do not modify the codes below.		
		FacebookSNSAgent.OnUserLoggedIn -= OnUserLoggedIn;
		FacebookSNSAgent.OnUserFailedToLogIn -= OnUserFailedToLogIn;
		FacebookSNSAgent.OnUserLoggedOut -= OnUserLoggedOut;
		FacebookSNSAgent.OnScorePosted -= OnScorePosted;
		FacebookSNSAgent.OnFailedToPostScore -= OnFailedToPostScore;
		FacebookSNSAgent.OnAchievementPosted -= OnAchievementPosted;
		FacebookSNSAgent.OnFailedToPostAchievement -= OnFailedToPostAchievement;
		FacebookSNSAgent.OnScreenShotPosted -= OnScreenShotPosted;
		FacebookSNSAgent.OnFailedToPostScreenShot -= OnFailedToPostScreenShot;
		FacebookSNSAgent.OnImagePosted -= OnImagePosted;
		FacebookSNSAgent.OnFailedToPostImage -= OnFailedToPostImage;
		FacebookSNSAgent.OnMessagePosted -= OnMessagePosted;
		FacebookSNSAgent.OnFailedToPostMessage -= OnFailedToPostMessage;
		FacebookSNSAgent.OnScoresDataArrived -= OnScoresDataArrived;
		FacebookSNSAgent.OnFailedToGetScoresData -= OnFailedToGetScoresData;
		FacebookSNSAgent.OnUserInfoArrived -= OnUserInfoArrived;
		FacebookSNSAgent.OnFailedToGetUserInfo -= OnFailedToGetUserInfo;
		FacebookSNSAgent.OnUserFriendsArrived -= OnUserFriendsArrived;
		FacebookSNSAgent.OnFailedToGetUserFriends -= OnFailedToGetUserFriends;
		FacebookSNSAgent.OnRequestGraphResultArrived -= OnRequestGraphResultArrived;
		FacebookSNSAgent.OnFailedToRequestGraph -= OnFailedToRequestGraph;
		FacebookSNSAgent.OnFriendsInvited -= OnFriendsInvited;
		FacebookSNSAgent.OnFailedToInviteFriends -= OnFailedToInviteFriends;		
		FacebookSNSAgent.OnAppRequestSent -= OnAppRequestSent;
		FacebookSNSAgent.OnFailedToSendAppRequest -= OnFailedToSendAppRequest;		
		FacebookSNSAgent.OnAppRequestReceived -= OnAppRequestReceived;
		FacebookSNSAgent.OnFailedToReceiveAppRequest -= OnFailedToReceiveAppRequest;
		FacebookSNSAgent.OnUserProfilePictureArrived -= OnUserProfilePictureArrived;
		FacebookSNSAgent.OnFailedToGetUserProfilePicture -= OnFailedToGetUserProfilePicture;
		FacebookSNSAgent.OnSSOLoginStatusArrived -= OnSSOLoginStatusArrived;
		FacebookSNSAgent.OnPageLiked -= OnPageLiked;
		FacebookSNSAgent.OnUserLikedPagesArrived -= OnUserLikedPagesArrived;
		FacebookSNSAgent.OnFailedToGetUserLikedPages -= OnFailedToGetUserLikedPages;
		FacebookSNSAgent.OnFriendPickerDismissed -= OnFriendPickerDismissed;
		FacebookSNSAgent.OnFriendTaggedInImage -= OnFriendTaggedInImage;
		FacebookSNSAgent.OnFailedToTagFriendInImage -= OnFailedToTagFriendInImage;
		FacebookSNSAgent.OnNewReadPermissionsGranted -= OnNewReadPermissionsGranted;
		FacebookSNSAgent.OnFailedToRequestNewReadPermissions -= OnFailedToRequestNewReadPermissions;
		FacebookSNSAgent.OnScorePassingMessagePosted -= OnScorePassingMessagePosted;
		FacebookSNSAgent.OnFailedToPostScorePassingMessage -= OnFailedToPostScorePassingMessage;
		FacebookSNSAgent.OnStoryPostedWithFeedDialog -= OnStoryPostedWithFeedDialog;
		FacebookSNSAgent.OnFailedToPostStoryWithFeedDialog -= OnFailedToPostStoryWithFeedDialog;		
		/**
		 * ---------------------------------------------------------------------------------------------------------------
		 * @since 1.5.0
		 * ---------------------------------------------------------------------------------------------------------------
		 */
		FacebookSNSAgent.OnMessagePostedWithShareDialog -= OnMessagePostedWithShareDialog;
		FacebookSNSAgent.OnFailedToPostMessageWithShareDialog -= OnFailedToPostMessageWithShareDialog;
		FacebookSNSAgent.OnLinkSharedWithShareDialog -= OnLinkSharedWithShareDialog;
		FacebookSNSAgent.OnFailedToShareLinkWithShareDialog -= OnFailedToShareLinkWithShareDialog;
		FacebookSNSAgent.OnActionPostedWithShareDialog -= OnActionPostedWithShareDialog;
		FacebookSNSAgent.OnFailedToPostActionWithShareDialog -= OnFailedToPostActionWithShareDialog;
		FacebookSNSAgent.OnAchievementsDataArrived -= OnAchievementsDataArrived;
		FacebookSNSAgent.OnFailedToGetAchievementsData -= OnFailedToGetAchievementsData;
		FacebookSNSAgent.OnCheckPageLikedResultArrived -= OnCheckPageLikedResultArrived;
		FacebookSNSAgent.OnFailedToCheckPageLiked -= OnFailedToCheckPageLiked;
	}
	
	/**
	 * Fired when the user successfully logged in.
	 */
	void OnUserLoggedIn()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnUserLoggedIn() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when the user failed to login.
	 * 
	 * @param err
	 *          string - The error string
	 */
	void OnUserFailedToLogIn(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnUserFailedToLogIn() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when the user is logged out.
	 */
	void OnUserLoggedOut()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnUserLoggedOut() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when a score is successfully posted.
	 */
	void OnScorePosted()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnScorePosted() Fired.");
		
		/// Your code here...
	}	
	
	/**
	 * Fired when failed to post a score.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToPostScore(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostScore() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when an achievement is successfully posted.
	 */
	void OnAchievementPosted()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnAchievementPosted() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to post an achievement.
	 * 
	 *  @param err
	 *          string - The error string
	 */
	void OnFailedToPostAchievement(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostAchievement() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when a screenshot is successfully posted.
	 */
	void OnScreenShotPosted()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnScreenShotPosted() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to capture and post a screenshot.
	 */
	void OnFailedToPostScreenShot(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostScreenShot() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when an image is successfully posted.
	 * 
	 * @param photoId
	 *          string - The photo ID returned by Facebook.
	 */
	void OnImagePosted(string photoId)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnImagePosted() Fired. photoId: " + photoId);
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to post an image.
	 */
	void OnFailedToPostImage(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostImage() Fired. Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when a message is successfully posted.
	 */
	void OnMessagePosted()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnMessagePosted() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to post a message.
	 * 
	 * @param err
	 *          string - The error string
	 */
	void OnFailedToPostMessage(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostMessage() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when the scores information arrived.	 
	 * 
	 * NOTE: the scores information is required for the plugin to post new scores, 
 	 * because it will do some calculations and only post better score to facebook.
 	 * So, make sure you call PostScore() function after this event is triggered.
	 * 
	 * @param scores
	 *          List<FacebookScoreInfo> - A list of score info.
	 */
	void OnScoresDataArrived(List<FacebookScoreInfo> scores)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnScoresDataArrived() Fired.");
			
			if (scores != null)
			{
				for (int i = 0; i < scores.Count; i++)
				{
					Debug.Log ("ScoreInfo[" + i + "]: userId -> " + scores[i].userId
					           + ", userName -> " + scores[i].userName
					           + ", score -> " + scores[i].score);			
				}
			}		           
		}
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to get scores data.
	 */
	void OnFailedToGetScoresData(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToGetScoresData() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when the user info data arrived.
	 * 
	 * NOTE: The user's privacy info, birthday and location will be empty if you don't 
	 * check "Need User Birthday Info" and "Need User Location Info" on FacebookSNSAgent
	 * gameObject. 
	 * 
	 * @param userInfo
	 *         FacebookUserInfo - An object containing the user info. 
	 */
	void OnUserInfoArrived(FacebookUserInfo userInfo)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnUserInfoArrived Fired. "
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
			        + ", profilePicture -> " + userInfo.profilePicture   
			        );
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to get user info.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToGetUserInfo(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToGetUserInfo() Fired. Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when the user's friends info arrived.
	 * 
	 * NOTE: The user's privacy info, birthday and location will be empty if you don't 
	 * check "Need User Birthday Info" and "Need User Location Info" on FacebookSNSAgent
	 * gameObject. 
	 * 
	 * @param userInfo
	 *         FacebookUserInfo - An object containing the user info. 
	 */
	void OnUserFriendsArrived(List<FacebookUserInfo> friends)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnUserFriendsArrived Fired. friends.Count -> " + friends.Count);
		
			for (int i = 0; i < friends.Count; i++)
			{
				FacebookUserInfo userInfo = friends[i];				
				Debug.Log ("friends[" + i + "]: "
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
				    );
			}
		}
		
		/// Your code here...
	}	
	
	/**
	 * Fired when failed to get user's friends.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToGetUserFriends(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToGetUserFriends() Fired. Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when the result of a graph request comes back.
	 * 
	 * @param requestId
	 *           string - The ID identifying a request.                   
	 * 
	 * @param result
	 *           string - The result in a Json string.
	 */
	void OnRequestGraphResultArrived(string requestId, string result)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnRequestGraphResultArrived() Fired. requestId: " + requestId + ", result: " + result);	
	}
	
	/**
	 * Fired when a graph request failed.
	 * 
	 * @param requestId
	 *           string - The ID identifying a request.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToRequestGraph(string requestId, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToRequestGraph() Fired. requestId: " + requestId + ", Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when a friends invitatioin request successfully sent.
	 */
	void OnFriendsInvited()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFriendsInvited() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when a friends invitation request failed.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToInviteFriends(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToInviteFriends() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when an app request to recipients is successfully sent.
	 */
	void OnAppRequestSent()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnAppRequestSent() Fired.");
		
		/// Your code here...
	}
	
	/**
	 * Fired when an app request to recipients failed.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToSendAppRequest(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToSendAppRequest() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when an app request (sent by another user) is received.
	 * 
	 * An app request is received automatically after the recipient taps on the
	 * "Notifications" -> "Request" in Facebook App.
	 * 
	 * NOTE: A request will be automatcially "consumed" (Deleted) by the plugin,
	 * once this event gets called.
	 * 
	 * @param request
	 *           FacebookAppRequest - An object containing all the request info. 
	 */
	void OnAppRequestReceived(FacebookAppRequest request)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnAppRequestReceived() Fired. "
				 	+ "id -> " + request.id
				    + ", friendName -> " + request.userName		
					+ ", friendId -> " + request.userId
				    + ", message -> " + request.message
				    + ", data -> " + request.data);				
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to receive an app request.
	 * 
	 * A typical failure is the "The request was already consumed". That happens
	 * when the recipient has tapped on that request, but the request is still 
	 * visible in Facebook App for some time. (State sync delay). This kind of 
	 * error could be ignored.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToReceiveAppRequest(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToReceiveAppRequest() Fired. Error: " + err);
		
		/// Your code here...
	}		
	
	/**
	 * Fired when a user profile picture arrived.
	 * 
	 * @param userId
	 *          string - The userId specified when call GetUserProfilePicture().
	 * 
	 * @param pictureUrl
	 *          string - The url of the picture.
	 */
	void OnUserProfilePictureArrived(string userId, string pictureUrl)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnUserProfilePictureArrived() Fired. userId: " + userId + ", pictureUrl: " + pictureUrl);
		
		/// Your code here...		
	}
	
	/**
	 * Fired when a user profile picture request failed.
	 * 
	 * @param userId
	 *          string - The userId specified when call GetUserProfilePicture().
	 * 
	 * @param err
	 *          string - The error message.
	 */
	void OnFailedToGetUserProfilePicture(string userId, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToGetUserProfilePicture() Fired. userId: " + userId + ", Error: " + err);
		
		/// Your code here...		
	}	
	
	/**
	 * Fired when the SSO login status query result returned..
	 * 
	 * @param loggedIn
	 *          bool - True for logged in, false for not.
	 * 
	 * @param errorCode
	 *          int - Facebook error code.
	 * 
	 * @param subErrorCode
	 *          int - Facebook sub error code.
	 * 
	 * @param errorMessage
	 *          string - Error message. 
	 * 
	 */
	void OnSSOLoginStatusArrived(bool loggedIn, int errorCode, int subErrorCode, string errorMessage)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnSSOLoginStatusArrived() Fired. "
				 	+ "loggedIn -> " + loggedIn
				    + ", errorCode -> " + errorCode		
					+ ", subErrorCode -> " + subErrorCode
				    + ", errorMessage -> " + errorMessage);	
		
		/// Your code here...		
	}	
	
	/**
	 * Fired when a page is successfully liked by the user.
	 */
	void OnPageLiked(string pageId)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPageLiked() Fired. pageId: " + pageId);
		
		/// Your code here...		
	}
	
	/**
	 * Fired when the user's liked pages query result returned.
	 *
	 * @param likedPages
	 *           List<string> - The page IDs liked by the user.
	 */
	void OnUserLikedPagesArrived(List<string> likedPages)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnUserLikedPagesArrived Fired. likedPages.Count -> " + likedPages.Count);
		
			for (int i = 0; i < likedPages.Count; i++)
			{							
				Debug.Log ("likedPages[" + i + "]: " + likedPages[i]);
			}
		}
		
		/// Your code here...		
	}
	
	/**
	 * Fired when failed to get the user's liked pages.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToGetUserLikedPages(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToGetUserLikedPages() Fired. Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when the friend picker dismissed.
	 * 
	 * @param selectedFriends
	 *          List<string> - The selected friends.
	 */
	void OnFriendPickerDismissed(List<string> selectedFriends)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnFriendPickerDismissed Fired. selectedFriends.Count -> " + selectedFriends.Count);
		
			for (int i = 0; i < selectedFriends.Count; i++)
			{							
				Debug.Log ("selectedFriends[" + i + "]: " + selectedFriends[i]);
			}
		}
		
		/// Your code here...
	}
	
	/**
	 * Fired when the friend is successfully taged in an image.
	 * 
	 * @param photoId
	 *           string - The photoId of the image, returned from Facebook.	                
	 * 
	 * @param userId
	 *           string - The userId of the friend to be tagged.
	 * 
	 * @param x
	 *           int - Percentage from the left edge of the image.
	 * 
	 * @param y
	 *           int - Percentage from the top edge of the image.
	 */
	void OnFriendTaggedInImage(string photoId, string userId, int x, int y)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFriendTaggedInImage() Fired. "
				 	+ "photoId -> " + photoId
				    + ", userId -> " + userId		
					+ ", x -> " + x
				    + ", y -> " + y);
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to tag a friend in an image.
	 * 
	 * @param photoId
	 *           string - The photoId of the image, returned from Facebook.	                
	 * 
	 * @param userId
	 *           string - The userId of the friend to be tagged.
	 * 
	 * @param x
	 *           int - Percentage from the left edge of the image.
	 * 
	 * @param y
	 *           int - Percentage from the top edge of the image.
	 * 
	 * @param err
	 *           string - The error message.
	 */
	void OnFailedToTagFriendInImage(string photoId, string userId, int x, int y, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToTagFriendInImage() Fired. "
			        + "Error -> " + err   
				 	+ ", photoId -> " + photoId
				    + ", userId -> " + userId		
					+ ", x -> " + x
				    + ", y -> " + y);			           
		
		/// Your code here...
	}
	 
	/** 
	 * Fired when the new read permissions are successfully granted.
	 * 
	 * @param permissions
	 *            List<string> - The newly granted permissions.
	 */
	void OnNewReadPermissionsGranted(List<string> permissions)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnNewReadPermissionsGranted Fired. permissions.Count -> " + permissions.Count);
		
			for (int i = 0; i < permissions.Count; i++)
			{							
				Debug.Log ("permissions[" + i + "]: " + permissions[i]);
			}
		}
		
		/// Your code here...		
	}
	
	/**
	 * Fired when failed to request new read permissions.
	 * 
	 * @param permissions
	 *            List<string> - The newly requested permissions.
	 * 
	 * @param err
	 *         string - The error message.
	 */
	void OnFailedToRequestNewReadPermissions(List<string> permissions, string err)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnFailedToRequestNewReadPermissions Fired. Error: " + err + ", permissions.Count -> " + permissions.Count);
		
			for (int i = 0; i < permissions.Count; i++)
			{							
				Debug.Log ("permissions[" + i + "]: " + permissions[i]);
			}
		}
		
		/// Your code here...		
	}
	
	/**
	 * Fired when a score passing message is successfully posted on friend's news feed.
	 */
	void OnScorePassingMessagePosted()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnScorePassingMessagePosted() Fired.");
		
		/// Your code here...
	}	
	
	/**
	 * Fired when failed to post a score passing message.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToPostScorePassingMessage(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostScorePassingMessage() Fired. Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when a story is successfully posted via Feed Dialog.
	 * 
	 * @param storyId
	 *         string - The story ID returned by Facebook.
	 */
	void OnStoryPostedWithFeedDialog(string storyId)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnStoryPostedWithFeedDialog() Fired. storyId: " + storyId);
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to post a story via Feed Dialog.
	 * 
	 * @param err
	 *         string - The error string.
	 */
	void OnFailedToPostStoryWithFeedDialog(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostStoryWithFeedDialog() Fired. Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * ---------------------------------------------------------------------------------------------------------------
	 * @since 1.5.0
	 * ---------------------------------------------------------------------------------------------------------------
	 */
	/**
	 * Fired when a message is successfully posted via Share Dialog.
	 */	 
	void OnMessagePostedWithShareDialog()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnMessagePostedWithShareDialog() Fired");
		
		/// Your code here...		
	}
	
	/**
	 * Fired when failed to post a message via Share Dialog.
	 * 
	 * @param err
	 *         string - The error message.
	 */
	void OnFailedToPostMessageWithShareDialog(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostMessageWithShareDialog() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when a link is successfully shared via Share Dialog.
	 * 
	 * @param link
	 *         string - The link that is shared.
	 */
	void OnLinkSharedWithShareDialog(string link)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnLinkSharedWithShareDialog() Fired. link: " + link);
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to share a link via Share Dialog.
	 * 
	 * @param link
	 *         string - The link that is attempted.
	 * 
	 * @param err
	 *         string - The error message.
	 */
	void OnFailedToShareLinkWithShareDialog(string link, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToShareLinkWithShareDialog() Fired. link: " + link + ", Error: " + err);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when an Open Graph Action is successfully posted.
	 * 
	 * @param actionTYpe 
	 *          string - The Action Type.
	 * 
	 * @param propertyName
	 *          string - The property name.
	 */
	void OnActionPostedWithShareDialog(string actionType, string propertyName)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnActionPostedWithShareDialog() Fired. actionType: " + actionType + ", propertyName: " + propertyName);
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to post an Open Graph Action.
	 * 
	 * @param actionTYpe 
	 *          string - The Action Type.
	 * 
	 * @param propertyName
	 *          string - The property name.
	 * 
	 * @param err
	 *          string - The error string.
	 */
	void OnFailedToPostActionWithShareDialog(string actionType, string propertyName, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostActionWithShareDialog() Fired. actionType: " + actionType + ", propertyName: " + propertyName + ", Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when the achievements information arrived.		 
	 * 
	 * @param achievements
	 *          List<FacebookAchievementInfo> - A list of achievement info.
	 */
	void OnAchievementsDataArrived(List<FacebookAchievementInfo> achievements)
	{
		if (_debug)
		{
			Debug.Log (this.GetType().ToString() + " - OnAchievementsDataArrived() Fired.");
			
			if (achievements != null)
			{
				for (int i = 0; i < achievements.Count; i++)
				{
					Debug.Log ("AchievementInfo[" + i + "]: achievementId -> " + achievements[i].achievementId
					           + ", title -> " + achievements[i].title
					           + ", type -> " + achievements[i].type
					           + ", url -> " + achievements[i].url);			
				}
			}		           
		}
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to get achievements data.
	 * 
	 * @param err
	 *         string - The error message.
	 */
	void OnFailedToGetAchievementsData(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToGetAchievementsData() Fired. Error: " + err);
		
		/// Your code here...
	}
	
	/**
	 * Fired when the result of checking if page is liked by the user returned.
	 * 
	 * @param pageId
	 *         string - The page ID that was checked for.
	 * 
	 * @param liked
	 *         bool - True for liked, false for not liked.
	 */
	void OnCheckPageLikedResultArrived(string pageId, bool liked)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnCheckPageLikedResultArrived() Fired. pageId: " + pageId + ", liked: " + liked);
		
		/// Your code here...
	}
	
	/**
	 * Fired when failed to check if page is liked by the user.
	 * 
	 * @param pageId
	 *         string - The page ID that was checked for.
	 * 
	 * @param err
	 *         string - The error message.
	 */
	void OnFailedToCheckPageLiked(string pageId, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToCheckPageLiked() Fired. pageId: " + pageId + ", Error: " + err);
		
		/// Your code here...
	}
	
}
