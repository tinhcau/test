using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    public AudioClip move;
    public AudioClip merge;

    void Awake()
    {
        soundIsEnabled = UserData.soundIsOn;
    }

    private bool _soundIsEnabled;
    public bool soundIsEnabled
    {
        get
        {
            return _soundIsEnabled;
        }
        set
        {
            _soundIsEnabled = value;
            if (_soundIsEnabled)
            {
                AudioListener.volume = 1;
                //AudioListener.pause = false;
            }
            else
            {
                AudioListener.volume = 0;
                //AudioListener.pause = true;
            }
        }
    }

	public void playSound(AudioClip audioClip)
	{
        if (!soundIsEnabled)
            return;

		GetComponent<AudioSource>().PlayOneShot(audioClip);
	}
}
