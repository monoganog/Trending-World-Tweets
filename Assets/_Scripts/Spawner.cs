using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    public float waitTime = 0.1f;

    public Material[] materials;
    public GameObject tweetPrefab;

    public GameObject[] countryMenuItems;

    public bool canMenuSpawn;
    public bool canTweetSpawn = true;

    public  List<GameObject> tweetItemsInstances = new List<GameObject>();
    public List<GameObject> MenuItemsInstances = new List<GameObject>();

    private static Spawner _instance;
    public static Spawner Instance
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
    }

    bool CollisionCheck(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        if (hitColliders.Length == 0) return true;
        return false;
    }

    Vector3 RandomCircle(Vector3 center, float radius, float a)
    {
        Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    public IEnumerator SpawnMenuItems()
    {
        for (int i = 0; i < countryMenuItems.Length; i++)
        {
            if (canMenuSpawn)
            {
                bool spawned = false;
                while (!spawned)
                {
                    Vector3 pos = new Vector3();
                    pos.x = Random.Range(-24f,24f);
                    pos.y = Random.Range(-24f,24f);

                    if (CollisionCheck(pos, 0.5f) && canMenuSpawn)
                    {
                        GameObject instance = Instantiate(countryMenuItems[i], pos, Quaternion.identity);
                        instance.transform.localScale = new Vector3(0, 0, 0);
                        MenuItemsInstances.Add(instance);
                        yield return new WaitForSeconds(waitTime);
                        if (instance != null)
                        {
                            instance.transform.DOScale(instance.GetComponent<Attractor>().finalSize, 1f);
                        }
                        spawned = true;
                    }
                    else
                    {
                        pos.x = Random.Range(-Screen.width, Screen.width);
                        pos.y = Random.Range(-Screen.height, Screen.height);
                    }
                }
            }
        }
    }

    public IEnumerator PurgeMenuItems()
    {
        if (MenuItemsInstances != null)
        {
            for (int i = 0; i < MenuItemsInstances.Count; i++)
            {
                MenuItemsInstances[i].transform.DOScale(0, waitTime / 5f);
                Destroy(MenuItemsInstances[i], waitTime / 5);

                yield return new WaitForSeconds(waitTime / 5);
            }
            MenuItemsInstances.RemoveAll((o) => o == null);
         }
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

    public void SpawnTweetWrapper(SimpleJSON.JSONNode jsonResponse,float maxSize,float minSize,float maxVol,float minVol)
    {
        StartCoroutine(SpawnTweetSetup(jsonResponse,maxSize,minSize,maxVol,minVol));
    }

    public IEnumerator SpawnTweetSetup(SimpleJSON.JSONNode jsonResponse,float maxSize, float minSize,float maxVol, float minVol)
    {
        int tweetColour = Random.Range(0, materials.Length);

        for (int i = 0; i < API.Instance.amountOfResponses; i++)
        {
            if (canTweetSpawn)
            {
                string url = jsonResponse[0]["trends"][i]["url"].Value;
                string responseNameText = jsonResponse[0]["trends"][i]["name"].Value;
                int hotness = jsonResponse[0]["trends"][i]["tweet_volume"].AsInt;

                float hotnessRemapped;

                if (hotness == 0)  hotnessRemapped = 1.5f; 
                else               hotnessRemapped = Remap(hotness, minVol, maxVol, minSize, maxSize);

                Vector3 remappedSizeVector = new Vector3(hotnessRemapped, hotnessRemapped, hotnessRemapped);

                Debug.Log("remapped size vector = " + remappedSizeVector);

                Vector3 pos = new Vector3();
                pos.x = Random.Range(-24f, 24f);
                pos.y = Random.Range(-24f, 24f);

                bool spawned = false;

                while (!spawned)
                {
                    if (CollisionCheck(pos, 0.1f))
                    {
                        InstantiateTweet(tweetPrefab, pos, remappedSizeVector, responseNameText,url,tweetColour);
                        spawned = true;
                    }
                    else
                    {
                        pos.x = Random.Range(-25f, 25f);
                        pos.y = Random.Range(-25f, 25f);
                    }
                }
                yield return new WaitForSeconds(waitTime);
            }
        }
        canTweetSpawn = true;
    }

    public void InstantiateTweet(GameObject tweetPrefab, Vector3 pos, Vector3 remapped, string responseNameText,string url,int colour)
    {

        GameObject instance = Instantiate(tweetPrefab, pos, Quaternion.identity);

        // Apply Material
        instance.GetComponentInChildren<Renderer>().material = materials[colour];

        instance.GetComponent<Attractor>().finalSize = remapped;
        instance.GetComponent<Attractor>().UpdateMass();
        // Set size

        instance.transform.localScale = new Vector3(0, 0, 0);
        instance.transform.DOScale(instance.GetComponent<Attractor>().finalSize.x, 1f);

        // Setting tweet Names
        instance.GetComponentInChildren<Text>().text = responseNameText;

        instance.name = responseNameText;

        instance.GetComponentInChildren<Hyperlink>().url = url;

        tweetItemsInstances.Add(instance);

    }

    public IEnumerator PurgeTweets()
    {
        Debug.Log("purging tweets");
        if (tweetItemsInstances != null)
        {
            for (int i = 0; i < tweetItemsInstances.Count; i++)
            {
                tweetItemsInstances[i].transform.DOScale(0, waitTime / 5f);
                Destroy(tweetItemsInstances[i]);

                yield return new WaitForSeconds(waitTime/5);

            }
            tweetItemsInstances.RemoveAll((o) => o == null);
        }
    }
}
