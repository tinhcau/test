using System;
using UnityEngine;
using System.Xml;

public class Settings
{
    static private Settings _instance;

    static public Settings instance
    {
        get
        {
            if (_instance == null)
                _instance = new Settings();

            return _instance;
        }
    }

    public string admobIOS;
    public string admobAndroid;
    public string flurryIOS;
    public string flurryAndroid;
    public string chartboostID;
    public string chartboostSignature;
    public string leaderboardIOS;
    public string leaderboardAndroid;
    public string playhavenFullscreen;
    public string facebookTitle;
    public string facebookMessage;
    public string facebookPicture;
    public string twitterMessage;
    public string linkIOS;
    public string linkAndroid;

    public Settings()
    {
        init();
    }

    public void init()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("settings");  
        XmlDocument _settings = new XmlDocument();
        _settings.LoadXml(textAsset.text);

        admobIOS = _settings.SelectSingleNode("settings/admob/iOS").InnerText;
        admobAndroid = _settings.SelectSingleNode("settings/admob/Android").InnerText;
        flurryIOS = _settings.SelectSingleNode("settings/flurry/iOS").InnerText;
        flurryAndroid = _settings.SelectSingleNode("settings/flurry/Android").InnerText;
        chartboostID = _settings.SelectSingleNode("settings/chartboost/iOS/id").InnerText;
        chartboostSignature = _settings.SelectSingleNode("settings/chartboost/iOS/signature").InnerText;
        leaderboardIOS = _settings.SelectSingleNode("settings/leaderboard/iOS").InnerText;
        leaderboardAndroid = _settings.SelectSingleNode("settings/leaderboard/Android").InnerText;
        playhavenFullscreen = _settings.SelectSingleNode("settings/playhaven/fullScreenTag").InnerText;
        linkIOS = _settings.SelectSingleNode("settings/link/iOS").InnerText;
        linkAndroid = _settings.SelectSingleNode("settings/link/Android").InnerText;
        facebookTitle = _settings.SelectSingleNode("settings/share/facebook/title").InnerText;
        facebookMessage = _settings.SelectSingleNode("settings/share/facebook/message").InnerText;
        facebookPicture = _settings.SelectSingleNode("settings/share/facebook/picture").InnerText;
        twitterMessage = _settings.SelectSingleNode("settings/share/twitter").InnerText;
    }
}