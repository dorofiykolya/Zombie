using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Runner
{
	public class LeaderboardsController : MonoBehaviour 
	{
		private static float prevGetTime;
        private static float prevFriendsTime;
        private static float prevRegionTime;

		public List<LeaderboardSlot> slots;

		private void StartGetLeaders()
		{
			if(prevGetTime < Time.timeSinceLevelLoad)
			{
				prevGetTime = Time.timeSinceLevelLoad + 10;
				GetLeaders ();
			}
		}

        public void StartGetFriends()
        {
            if(prevFriendsTime < Time.timeSinceLevelLoad)
            {
                prevFriendsTime = Time.timeSinceLevelLoad + 10;
                GetFriends ();
            }
        }

        public void StartGetRegion()
        {
            if(prevRegionTime < Time.timeSinceLevelLoad)
            {
                prevRegionTime = Time.timeSinceLevelLoad + 10;
                GetRegion ();
            }
        }

		void OnEnable()
		{
			if (PlayerData.playerID == null)
				return;

			if(PlayerData.creation <= 0)
				StartCoroutine (CreatePlayer ());

            if (name == "Friend")
            {
                return;
            }

            if (name == "Region")
            {
                CheckRegion();
            }
            else
            {
                StartGetLeaders();
            }
		}

        private void CheckRegion()
        {
            if (PlayerData.region != "")
            {
                StartGetRegion();
            }
            else if (Input.location.isEnabledByUser)
            {
                getGeographicalCoordinates();
            }
        }

		private IEnumerator CreatePlayer()
		{
            string query = string.Format("http://red-clove.com.ua/runner/runner_create.php?id={0}&score={1}&realname={2}&facebook={3}&image={4}&region={5}", 
                                         PlayerData.playerID, PlayerData.Distance, PlayerData.realName, PlayerData.facebook, PlayerData.image, PlayerData.region);
			WWW www = new WWW(query);
			yield return www;
			if(www.text == "1")
			{
				PlayerData.creation = 1;
				PlayerPrefs.SetInt("Creation", PlayerData.creation);
			}
		}

        private IEnumerator LoadQuery(string query)
        {
            query = query.Replace(" ", "%20");
            WWW www = new WWW(query);
            yield return www;
            var array = www.text.Split(new string[]{"playerPosition"}, System.StringSplitOptions.None);
            var jsonleaders = SimpleJSON.JSON.Parse (array[0]).AsArray;
            var jsonplayer = SimpleJSON.JSON.Parse (array[1]).AsArray;
            
            for(int i = 0; i < jsonleaders.Count; i++)
            {
                slots[i].score.text = jsonleaders[i]["score"];
                slots[i].playername.text = jsonleaders[i]["realname"];
                if(jsonleaders[i]["image"] != null)
                    StartCoroutine(slots[i].Download (jsonleaders[i]["image"]));
                
                if(PlayerData.realName.Equals(jsonleaders[i]["realname"]))
                    slots[i].playername.color = new Color(246, 255, 0);
                else
                    slots[i].playername.color = Color.white;
                slots[i].gameObject.SetActive(true);
            }
            
            if(int.Parse(jsonplayer[0]["COUNT(*)"]) > 10)
            {
                slots[10].score.text = PlayerData.Distance.ToString();
                slots[10].playername.text = PlayerData.realName;
                slots[10].playername.color = new Color(246, 255, 0);
                slots[10].place.text = jsonplayer[0]["COUNT(*)"];
                slots[10].gameObject.SetActive(true);
            }
        }

        private void GetRegion()
        {
            string query = string.Format("http://red-clove.com.ua/runner/runner_region.php?id={0}&score={1}&region={2}", 
                                         PlayerData.playerID, PlayerData.Distance, PlayerData.region);
            StartCoroutine(LoadQuery(query));
        }

        private void GetFriends()
        {
            string query = string.Format("http://red-clove.com.ua/runner/runner_friends.php?id={0}&score={1}&realname={2}&facebook={3}&image={4}&friends={5}", 
                                         PlayerData.playerID, PlayerData.Distance, PlayerData.realName, PlayerData.facebook, PlayerData.image, PlayerData.friends);
            StartCoroutine(LoadQuery(query));
        }

        private void GetLeaders()
		{
            string query = string.Format("http://red-clove.com.ua/runner/runner_score.php?id={0}&score={1}&realname={2}&facebook={3}&image={4}", 
                                         PlayerData.playerID, PlayerData.Distance, PlayerData.realName, PlayerData.facebook, PlayerData.image);
            StartCoroutine(LoadQuery(query));
		}

        public void getGeographicalCoordinates()
        {
            StartCoroutine(getGeographicalCoordinatesCoroutine());
        }

        private IEnumerator getGeographicalCoordinatesCoroutine()
        {
            Input.location.Start();

            int maximumWait = 20;
            while(Input.location.status == LocationServiceStatus.Initializing && maximumWait > 0)
            {
                yield return new WaitForSeconds(1);
                maximumWait--;
            }

            if(maximumWait < 1 || Input.location.status == LocationServiceStatus.Failed)
            {
                Input.location.Stop();
                yield break;
            }

            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;

            Input.location.Stop();
            WWW www = new WWW("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=true");
            yield return www;
            if(www.error != null) yield break;
            XmlDocument reverseGeocodeResult = new XmlDocument();
            reverseGeocodeResult.LoadXml(www.text);
            if(reverseGeocodeResult.GetElementsByTagName("status").Item(0).ChildNodes.Item(0).Value != "OK") yield break;
            string countryCode = null;
            bool countryFound = false;
            foreach(XmlNode eachAdressComponent in reverseGeocodeResult.GetElementsByTagName("result").Item(0).ChildNodes)
            {
                if(eachAdressComponent.Name == "address_component")
                {
                    foreach(XmlNode eachAddressAttribute in eachAdressComponent.ChildNodes)
                    {
                        if(eachAddressAttribute.Name == "short_name") countryCode = eachAddressAttribute.FirstChild.Value;
                        if(eachAddressAttribute.Name == "type" && eachAddressAttribute.FirstChild.Value == "country")
                            countryFound = true;
                    }
                    if(countryFound) break;
                }
            }
        }
	}
}
