using UnityEngine;
using System.Collections;

public class UndoButton : MonoBehaviour
{
    public tk2dTextMesh amountTf;
    public tk2dUIItem btn;
    public Game game;

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

    private int _undoCnt;
    public int undoCnt
    {
        get
        {
            return _undoCnt;
        }
        set
        {
            _undoCnt = value;
            UserData.undoCnt = _undoCnt;
            amountTf.text = _undoCnt.ToString();
            amountTf.Commit();
        }
    }

    void Start()
    {
        undoCnt = UserData.undoCnt;
        btn.OnClick += click;
    }

    void Update()
    {
	
    }

    void click()
    {
        if (undoCnt == 0)
            gameManager.showUndopopup();
        else if (game.undoStep())
            undoCnt--;
    }


}
