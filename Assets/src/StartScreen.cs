using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour, IScreen
{
    public tk2dUIItem playBtn;

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
        playBtn.OnClick += startGame;
    }

    void startGame()
    {
        if (UserData.isFirstPlay)
            gameManager.showTutorial();
        else
            gameManager.startGame();
    }

    void Update()
    {
	
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void init()
    {
    }
}
