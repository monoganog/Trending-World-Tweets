using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;


    public class GameManager : MonoBehaviour
    {
        public enum State { Menu, Intro, Playing};
        public GameObject MenuUI, IntroUI, PlayingUI;
        
        public State state;

        public Text locationText;

        public Text scoreText;

        private static GameManager _instance;
        public static GameManager Instance
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
            EnterMenu();
            
    }

        void FixedUpdate()
        {
            switch (state)
            {
                case State.Menu:
                    {
                        
                        break;
                    }

                case State.Intro:
                    {
                        break;

                    }

                case State.Playing:
                    {
                        break;
                    }

            }
        }

    /* 
     * Public Functions to be called from UI elements or from other scripts etc
     * these will then change the game state to be ran every frame. Can also be 
     * used like constructers to set up play states 
    */

    //Vector3 RandomCircle(Vector3 center, float radius, float a)
    //{
    //    Debug.Log(a);
    //    float ang = a;
    //    Vector3 pos;
    //    pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
    //    pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
    //    pos.z = center.z;
    //    return pos;
    //}

    public void EnterMenu()
    {
        locationText = GameObject.FindWithTag("locationText").GetComponent<Text>();
        locationText.text = null;


        Spawner.Instance.StopCoroutine("SpawnTweetSetup");
        Spawner.Instance.StartCoroutine("PurgeTweets");
        Spawner.Instance.canTweetSpawn = false;

        Spawner.Instance.StartCoroutine("SpawnMenuItems");

        PlayingUI.SetActive(false);

        state = State.Menu;
        MenuUI.SetActive(true);
    }

    public void EnterIntro()
    {
        state = State.Intro;
        MenuUI.SetActive(false);
        IntroUI.SetActive(true);
    }

    public void EnterPlaying()
    {
        Spawner.Instance.canTweetSpawn = true;
        Spawner.Instance.StopCoroutine("SpawnMenuItems");
        Spawner.Instance.StartCoroutine("PurgeMenuItems");

        //API.Instance.gameObject.SetActive(true);
        state = State.Playing;

        IntroUI.SetActive(false);
        PlayingUI.SetActive(true);
    }
}
