using UnityEngine;
using System.Collections;

public class OptionsScreen : MonoBehaviour, IScreen
{
    public tk2dUIItem restartBtn;
    public tk2dUIItem continueBtn;
    public tk2dUIItem tutorialBtn;
    public GameObject soundBtn;
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
        restartBtn.OnClick += restart;
        continueBtn.OnClick += continueGame;
        tutorialBtn.OnClick += showTutorial;
    }

    void showTutorial()
    {
        gameManager.showTutorial();
    }

    void restart()
    {
        gameManager.startGame();
    }

    void continueGame()
    {
        gameManager.continueGame();
    }

    void Update()
    {

    }

    public void show()
    {
        soundBtn.SetActive(true);
        gameObject.SetActive(true);
    }

    public void hide()
    {
        soundBtn.SetActive(false);
        gameObject.SetActive(false);
    }

    public void init()
    {
    }
}
