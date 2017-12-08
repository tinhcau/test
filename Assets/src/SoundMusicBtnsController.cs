using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class SoundMusicBtnsController : MonoBehaviour {

    public tk2dUIItem soundBtn;
    public tk2dSlicedSprite soundBtnSprite;

    private SoundController _soundController;
    public SoundController soundController
    {
        get
        {
            if (_soundController == null)
                _soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
            return _soundController;
        }
    }

    void OnEnable()
    {
        soundBtnSprite.SetSprite(UserData.soundIsOn ? "soundOn" : "soundOff");
    }

	// Use this for initialization
	void Start () 
    {
        soundBtn.OnClick += clickSound;
        switchSound(UserData.soundIsOn);
	}
        
    void clickSound()
    {
        switchSound(!UserData.soundIsOn);
        soundController.playSound(soundController.move);
    }

    void switchSound(bool value)
    {
        UserData.soundIsOn = value;
        soundController.soundIsEnabled = value;
        soundBtnSprite.SetSprite(value ? "soundOn" : "soundOff");
    }

}