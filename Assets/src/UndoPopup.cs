using UnityEngine;
using System.Collections;

public class UndoPopup : MonoBehaviour, IScreen
{
    public tk2dUIItem continueBtn;
    public tk2dUIItem pack1Btn;
    public tk2dUIItem pack2Btn;
    public tk2dUIItem pack3Btn;

   

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
        continueBtn.OnClick += continueGame;
        pack1Btn.OnClick += buyPack1;
        pack2Btn.OnClick += buyPack2;
        pack3Btn.OnClick += buyPack3;
    }

    void buyPack1()
    {
       
    }

    void buyPack2()
    {
       
    }

    void buyPack3()
    {
        
    }

    void continueGame()
    {
        gameManager.hideUndopopup();
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
