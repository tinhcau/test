using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Tutorial : MonoBehaviour, IScreen
{
    public const float MOVE_TWEEN_DURATION = 0.2f;

    public tk2dUIItem playBtn;
    public GameObject stepsContainer;
    public tk2dSprite points;
    private int _curSprite;
    private Vector3 step1Position = new Vector3(0, 0, 0);
    private Vector3 step2Position = new Vector3(-780, 0, 0);
    public int curSprite
    {
        get
        {
            return _curSprite;
        }
        set
        {
            _curSprite = value;
            points.SetSprite("points" + _curSprite.ToString());
        }
    }

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
        gameManager.startGame();
    }

    void Update()
    {
        if (SwipeManager.swipeDirection == SwipeManager.Swipe.Left && curSprite == 1)
        {
            curSprite = 2;
            HOTween.To(stepsContainer.transform, MOVE_TWEEN_DURATION, "position", step2Position);
        }
        else if (SwipeManager.swipeDirection == SwipeManager.Swipe.Right && curSprite == 2)
        {
            curSprite = 1;
            HOTween.To(stepsContainer.transform, MOVE_TWEEN_DURATION, "position", step1Position);
        }

    }

    public void show()
    {
        stepsContainer.transform.position = step1Position;
        curSprite = 1;
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
