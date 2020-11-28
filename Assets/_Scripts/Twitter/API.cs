using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;


public class API : MonoBehaviour
{



    public string API_ENDPOINT = "api.giphy.com/v1/stickers/trending?api_key=Jn6yHj0clmAYmX78g3ddqv1MycVw8rLo";

    public string twitterEndpoint = "https://api.twitter.com/1.1/trends/place.json?id=";
    public string woeid;

    public bool canSpawn = true;
    public float offsetAmount = 5;
    public float waitTime = 0.1f;


    public int amountOfResponses;

    int minVol;
    int maxVol;
    int total;


    public Vector3 center = Vector3.zero;


    private static API _instance;
    public static API Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            if (_instance == null)
                _instance = value;
        }
    }


    void Start()
    {
        Instance = this;

        string url = twitterEndpoint + woeid;
        Debug.Log(url);
    }

    public void loadCountries(int country)
    {
        StartCoroutine(GetTwitter(twitterEndpoint+country));
        GameManager.Instance.EnterPlaying();
    }


    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }


    private void GetMinMaxHotness(SimpleJSON.JSONNode input,int index)
    {
        int hotness = input[0]["trends"][index]["tweet_volume"].AsInt;

        if (minVol > hotness && hotness != 0) minVol = hotness;

        if (maxVol < hotness) maxVol = hotness;

        total += hotness;
    }

    // TWITTER API

    IEnumerator GetTwitter(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer AAAAAAAAAAAAAAAAAAAAAKWm9gAAAAAA2Swy8VhRJfMisrjGwdhEZ%2FOHRYg%3DRX2zHTVyV0WQQsIZJ0wcHTYQuxc14OREjms0pyk8Df9HdUrW9b");
        yield return www.SendWebRequest();

        // If error happens during request
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        // Successful response
        {
            minVol = 10000000;
            maxVol = 0;
            total = 0;

            // Response as string
            string myJsonString = www.downloadHandler.text;

            // Making response string into a json object
            var jsonResponse = JSON.Parse(myJsonString);

            amountOfResponses = jsonResponse[0]["trends"].Count;
            if (www.isDone)
            {
                // Get Min Max Value Range
                for (int i = 0; i < amountOfResponses; i++)
                {
                    //Finds min and max hot rating to be used in remap of sizes
                    GetMinMaxHotness(jsonResponse,i);

                }

                float maxSize;
                maxSize = Remap(total, 500000, 5000000, 3.2f, 3.2f);
                float minSize = 1f;

                //Debug.Log("total tweets hotness = " + total);


                //WRAP THIS
                Spawner.Instance.SpawnTweetWrapper(jsonResponse,maxSize,minSize,maxVol,minVol);


            }
        }
    }
}