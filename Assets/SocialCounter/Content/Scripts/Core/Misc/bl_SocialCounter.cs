////////////////////////////////////////////////////////////////////////////
// bl_SocialCounter
//
//
//                    Lovatto Studio 2016
////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class bl_SocialCounter : MonoBehaviour
{
    [Header("Global")]
    [Range(10,9999)] public float UpdateEach = 60 * 5;//in seconds
    public bool HideOnNonInternet = true;
    [Header("Facebook")]
    public  string Facebook_ID = "";
    [Header("Youtube")]
    public string ChannelID;
    public string YoutubeKey;
    [Header("Twitter")]
    public string Twitter_User;
    [Header("Google Plus")]
    public string GooglePlus_User;
    public string GooglePlus_Key;
    [Header("Instagram")]
    public string Instagram_UserID;
    public string Instagram_AcessToken;
    [Header("Github")]
    public string Github_User;

    [Header("Cache")]
    public YoutubeInfo YouTube;
    public FacebookJson Facebook;
    public TwitterInfo Twitter;
    public GooglePlusJson GooglePlus;
    public InstagramJson Instagram;
    public GithubJson Github;

    private List<bl_SocialCounterDisplay> FacebookDisplay = new List<bl_SocialCounterDisplay>();
    private List<bl_SocialCounterDisplay> YoutubeDisplay = new List<bl_SocialCounterDisplay>();
    private List<bl_SocialCounterDisplay> GooglePlusDisplay = new List<bl_SocialCounterDisplay>();
    private List<bl_SocialCounterDisplay> TwitterDisplay = new List<bl_SocialCounterDisplay>();
    private List<bl_SocialCounterDisplay> InstagramDisplay = new List<bl_SocialCounterDisplay>();
    private List<bl_SocialCounterDisplay> GithubDisplay = new List<bl_SocialCounterDisplay>();

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //check if have internet connection avaible
        if (HideOnNonInternet && !CheckForInternetConnection())
        {
            HideAll();
            return;
        }

        GetStatics();
        StartUpdate();
    }

    /// <summary>
    /// Call this for start to update the statics
    /// </summary>
    public void StartUpdate()
    {
        InvokeRepeating("GetStatics", 1, UpdateEach);
    }

    /// <summary>
    /// Call this for stop updates the statics
    /// </summary>
    public void CancelUpdate()
    {
        CancelInvoke();
    }

    /// <summary>
    /// Get statics
    /// </summary>
    void GetStatics()
    {
        StartCoroutine(GetFacebookCount());
        StartCoroutine(GetYoutubeCount());
        StartCoroutine(GetTwitterCount());
        StartCoroutine(GetGooglePlusCount());
        StartCoroutine(GetInstagramCount());
        StartCoroutine(GetGithubCount());
    }

    /// <summary>
    /// 
    /// </summary>
    void HideAll()
    {
        foreach(bl_SocialCounterDisplay d in FacebookDisplay) { d.Hide(); }
        foreach (bl_SocialCounterDisplay d in YoutubeDisplay) { d.Hide(); }
        foreach (bl_SocialCounterDisplay d in GooglePlusDisplay) { d.Hide(); }
        foreach (bl_SocialCounterDisplay d in TwitterDisplay) { d.Hide(); }
        foreach (bl_SocialCounterDisplay d in InstagramDisplay) { d.Hide(); }
        foreach (bl_SocialCounterDisplay d in GithubDisplay) { d.Hide(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sd"></param>
    public void RegisterDisplay(bl_SocialCounterDisplay sd)
    {
        switch (sd.m_Social)
        {
            case bl_SocialCounterDisplay.Social.Facebook:
                FacebookDisplay.Add(sd);
                break;
            case bl_SocialCounterDisplay.Social.GooglePlus:
                GooglePlusDisplay.Add(sd);
                break;
            case bl_SocialCounterDisplay.Social.Instagram:
                InstagramDisplay.Add(sd);
                break;
            case bl_SocialCounterDisplay.Social.Twitter:
                TwitterDisplay.Add(sd);
                break;
            case bl_SocialCounterDisplay.Social.Youtube:
                YoutubeDisplay.Add(sd);
                break;
            case bl_SocialCounterDisplay.Social.Github:
                GithubDisplay.Add(sd);
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetFacebookCount()
    {
        string json_url = string.Format("http://api.facebook.com/restserver.php?format=json&method=links.getStats&urls={0}", Facebook_ID);
        WWW w = new WWW(json_url);

        yield return w;

        if(w.error == null)
        {
           // Debug.Log("Facebook: " + w.text);
            Facebook = new FacebookJson();
            //get value from json response
            //not use JsonUtily due this not support top level yet.
            Facebook.likes = GetJsonIntegerValue(w.text, "like_count");
            foreach (bl_SocialCounterDisplay d in FacebookDisplay) { d.SendCount(FacebookLike()); }
        }
        else
        {
            Debug.LogWarning("Error: " + w.error);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetYoutubeCount()
    {
        string json_url = string.Format("https://www.googleapis.com/youtube/v3/channels?part=statistics&id={0}&key={1}",ChannelID,YoutubeKey);
        WWW w = new WWW(json_url);

        yield return w;

        if(w.error == null)
        {
            //Debug.Log("Youtube: " + w.text);
            //get value from json deserialize
            YouTube = YoutubeInfo.CreateFromJSON(w.text);
            foreach (bl_SocialCounterDisplay d in YoutubeDisplay) { d.SendCount(YoutubeSuscribers()); }
        }
        else
        {
            Debug.LogWarning("Error: " + w.error);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetTwitterCount()
    {
        string json_url = string.Format("https://cdn.syndication.twimg.com/widgets/followbutton/info.json?screen_names={0}", Twitter_User);
        WWW w = new WWW(json_url);

        yield return w;

        if (w.error == null)
        {
           // Debug.Log( "Twitter: " + w.text);

            Twitter = new TwitterInfo();
            //get value from json response
            //not use JsonUtily due this not support top level yet.
            Twitter.Followers = GetJsonIntegerValue(w.text, "followers_count");
            foreach (bl_SocialCounterDisplay d in TwitterDisplay) { d.SendCount(TwitterFollowers()); }
        }
        else
        {
            Debug.LogWarning("Error: " + w.error);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetGooglePlusCount()
    {
        string json_url = string.Format("https://www.googleapis.com/plus/v1/people/{0}?key={1}",GooglePlus_User,GooglePlus_Key);
        WWW w = new WWW(json_url);

        yield return w;

        if (w.error == null)
        {
           // Debug.Log("GooglePlus: " + w.text);
            GooglePlus = GooglePlusJson.CreateFromJson(w.text);
            foreach (bl_SocialCounterDisplay d in GooglePlusDisplay) { d.SendCount(GooglePlusFollows()); }
        }
        else
        {
            Debug.LogWarning("Error: " + w.error);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetInstagramCount()
    {
        string json_url = string.Format("https://api.instagram.com/v1/users/{0}/?access_token={1}",Instagram_UserID,Instagram_AcessToken);
        WWW w = new WWW(json_url);

        yield return w;

        if (w.error == null)
        {
           // Debug.Log("Instagram: " + w.text);
            Instagram = InstagramJson.ConvertFromJson(w.text);
            foreach (bl_SocialCounterDisplay d in InstagramDisplay) { d.SendCount(InstagramFollowers()); }
        }
        else
        {
            Debug.LogWarning("Error: " + w.error);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetGithubCount()
    {
        string json_url = string.Format("https://api.github.com/users/{0}", Github_User);
        WWW w = new WWW(json_url);

        yield return w;

        if (w.error == null)
        {
            //Debug.Log("Github: " + w.text);
            Github = GithubJson.CreateFromJson(w.text);
            foreach (bl_SocialCounterDisplay d in GithubDisplay) { d.SendCount(GithubFollowers()); }
        }
        else
        {
            Debug.LogWarning("Error: " + w.error);
        }
    }


    public int GetJsonIntegerValue(string jsonstring,string valueToFind)
    {
        //split text instead to deserialize json due JsonUtilty of unity
        //doesn't support Desearilization in top level (in version 5.3.1 and regresion)
        string[] split = jsonstring.Split(","[0]);
        foreach (string t in split)
        {
            if (t.Contains(valueToFind))
            {
                return GetIntigerValue(t);
            }
        }
        return -1;
    }

    public int GetIntigerValue(string line)
    {
        return int.Parse(Regex.Match(line, @"\d+").Value);
    }

    public int FacebookLike()
    {
        return Facebook.likes;
    }

    public int YoutubeSuscribers()
    {
        return YouTube.items[0].statistics.subscriberCount;
    }

    public int TwitterFollowers()
    {
        return Twitter.Followers;
    }

    public int InstagramFollowers()
    {
        return Instagram.data.counts.follows;
    }

    public int GooglePlusFollows()
    {
        return GooglePlus.circledByCount;
    }

    public int GithubFollowers()
    {
        return Github.followers;
    }

    public bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new System.Net.WebClient())
            {
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    #region Twitter
    [Serializable]
    public class TwitterInfo
    {
        public int Followers;
    }
    #endregion

    #region Facebook Json
    [Serializable]
    public class FacebookJson
    {
        public int likes;
        public string id;

        public static FacebookJson CreateFromJson(string jsonstring)
        {
            return JsonUtility.FromJson<FacebookJson>(jsonstring);
        }

    }
    #endregion

    #region YouTube Json
    [System.Serializable]
    public class YoutubeInfo
    {
        public string kind;
        public YoutubeItem[] items;

        public static YoutubeInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<YoutubeInfo>(jsonString);
        }

    }
    [System.Serializable]
    public class YoutubeItem
    {
        public string id;
        public YoutubeStatics statistics;
    }
    [System.Serializable]
    public class YoutubeStatics
    {
        public int subscriberCount;
        public int commentCount;
        public int viewCount;
        public int videoCount;
        public bool hiddenSubscriberCount;
    }
    #endregion

    #region GooglePlus Json
    [Serializable]
    public class GooglePlusJson
    {
        public string displayName;
        public int circledByCount;

        public static GooglePlusJson CreateFromJson(string jsonstring)
        {
            return JsonUtility.FromJson<GooglePlusJson>(jsonstring);
        }
    }
    #endregion

    #region Instagram Json
    [Serializable]
    public class InstagramJson
    {
        public InstagranData data;

        public static InstagramJson ConvertFromJson(string Jsonstring)
        {
            return JsonUtility.FromJson<InstagramJson>(Jsonstring);
        }
    }

    [Serializable]
    public class InstagranData
    {
        public InstagramCount counts;
    }

    [Serializable]
    public class InstagramCount
    {
        public int followed_by;
        public int follows;
        public int media;
    }
    #endregion

    #region Hithub Json
    [Serializable]
    public class GithubJson
    {
        public string login;
        public int followers;
        public int following;

        public static GithubJson CreateFromJson(string Jsonstring)
        {
            return JsonUtility.FromJson<GithubJson>(Jsonstring);
        }
    }
    #endregion
}