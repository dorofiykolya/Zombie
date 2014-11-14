using UnityEngine;
using System.Collections;

namespace Runner
{
	public class LeaderboardSlot : MonoBehaviour 
	{
		public UILabel playername;
		public UILabel place;
		public UILabel score;
        public UITexture image;

        public IEnumerator Download(string url)
        {
            url = url.Replace("melkhior", "&");

            var www = new WWW(url);
            yield return www;
            image.mainTexture = www.texture as Texture;
        }
	}
}
