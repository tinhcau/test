using UnityEngine;
using System.Collections;
using System;

public class FacebookBtn : MonoBehaviour
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
        if (FB.IsLoggedIn)
            post();
        else
            FB.Login("publish_actions", LoginCallback);
    }

    void post()
    {
        string message = Settings.instance.facebookMessage.Replace("%score%", gameManager.score.ToString());

        #if UNITY_ANDROID
        FB.Feed("", Settings.instance.linkAndroid, Settings.instance.facebookTitle, Settings.instance.facebookTitle, message);
        #else
        FB.Feed("", Settings.instance.linkIOS, Settings.instance.facebookTitle, Settings.instance.facebookTitle, message, Settings.instance.facebookPicture);
        #endif
    }
         
    void LoginCallback(FBResult result)
    {
        if (result.Error != null)
            Debug.Log("Error Response:\n" + result.Error);
        else if (!FB.IsLoggedIn)
            Debug.Log("Login cancelled by Player");
        else
            post();
    }
}
