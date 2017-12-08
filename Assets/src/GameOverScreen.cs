using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour, IScreen
{

    public tk2dUIItem restartBtn;
    public tk2dTextMesh scoreTf;
    public tk2dTextMesh bestScoreTf;

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
    }

    void restart()
    {
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

    public void init(int score)
    {
        scoreTf.text = score.ToString();
        scoreTf.Commit();

        bestScoreTf.text = UserData.bestScore.ToString();
        bestScoreTf.Commit();
    }
}
