using UnityEngine;
using System.Collections;

public class RateBtn : MonoBehaviour
{
    public tk2dUIItem btn;


    void Start()
    {
        btn.OnClick += click;
    }

    void Update()
    {

    }

    void click()
    {
        #if UNITY_ANDROID
        Application.OpenURL(Settings.instance.linkAndroid);
        #else
        Application.OpenURL(Settings.instance.linkIOS);
        #endif

    }


}
