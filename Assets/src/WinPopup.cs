using UnityEngine;
using System.Collections;

public class WinPopup : MonoBehaviour, IScreen
{
    public tk2dUIItem restartBtn;
    public tk2dUIItem continueBtn;
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
    }

    void restart()
    {
        gameManager.hideWinpopup(true);
    }

    void continueGame()
    {
        gameManager.hideWinpopup(false);
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
