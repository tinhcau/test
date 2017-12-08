using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    static private bool _GAME_RUNNING;
    static public bool GAME_RUNNING
    {
        get
        {
            return _GAME_RUNNING;
        }
        set
        {
            _GAME_RUNNING = value;
            SwipeManager.swipeDirection = SwipeManager.Swipe.None;
        }
    }

    public Game game;
    public StartScreen startScreen;
    public GameOverScreen gameOverScreen;
    public OptionsScreen optionsScreen;
    public Tutorial tutorial;
    public WinPopup winPopup;
    public UndoPopup undoPopup;

    public int score;

    private IScreen _curScreen;

    void Start()
    {

        GAME_RUNNING = false;
        game.hide();
        game.init();
        gameOverScreen.hide();
        optionsScreen.hide();
        tutorial.hide();
        winPopup.hide();
        undoPopup.hide();

        _curScreen = startScreen;
        _curScreen.show();
    }

    public void startGame()
    {
        _showScreen(game);
        game.restart();
    }

    public void showTutorial()
    {
        GAME_RUNNING = false;
        _showScreen(tutorial);
    }

    public void showWinpopup(int score)
    {
        
        this.score = score;
        GAME_RUNNING = false;
        winPopup.show();
    }

    public void hideWinpopup(bool needRestart)
    {
        GAME_RUNNING = true;
        winPopup.hide();
        if (needRestart)
            game.restart();
    }

    public void showUndopopup()
    {
        GAME_RUNNING = false;
        undoPopup.show();
    }

    public void hideUndopopup()
    {
        GAME_RUNNING = true;
        undoPopup.hide();
    }

    public void continueGame()
    {
        _showScreen(game);
        GAME_RUNNING = true;
    }

    public void showOptions()
    {
        GAME_RUNNING = false;
        _showScreen(optionsScreen);
    }

    public void showGameOver(int score)
    {
       
        this.score = score;
        GAME_RUNNING = false;
        gameOverScreen.init(score);
        _showScreen(gameOverScreen);
    }
        
    private void _showScreen(IScreen screen)
    {
        _curScreen.hide();
        _curScreen = screen;
        _curScreen.show();
    }
}
