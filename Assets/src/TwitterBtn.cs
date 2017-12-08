using UnityEngine;
using System.Collections;
using System;

public class TwitterBtn : MonoBehaviour
{
    public tk2dUIItem btn;

    private GameManager _gameManager;
    public GameManager gameManager
    {
        get
        {
            if (_gameManager == null)
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            return _gameManager;
        }
    }

    void Start()
    {
        btn.OnClick += click;
    }

    void Update()
    {

    }

    void click()
    {
        string message = Settings.instance.twitterMessage.Replace("%score%", gameManager.score.ToString());
        #if UNITY_ANDROID
        message += Settings.instance.linkAndroid;
        #else
        message += Settings.instance.linkIOS;
        #endif
        string twittershare = "http://twitter.com/home?status=" + Uri.EscapeUriString(message);
        Application.OpenURL(twittershare);
    }


}
