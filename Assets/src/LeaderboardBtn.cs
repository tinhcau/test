using UnityEngine;
using System.Collections;

public class LeaderboardBtn : MonoBehaviour
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
        
    }


}
