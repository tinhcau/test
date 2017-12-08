using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour, IScreen
{
    private const int FIELD_SIZE = 4;
    private const int TILE_TYPES = 11;

    public tk2dTextMesh scoreTF;
    public tk2dTextMesh bestScoreTF;
    public tk2dUIItem optionsBtn;
    public bool debugFill;
    public GameObject HUD;

    private Tile[][] _tiles;
    private int _freeCellsCnt;
    private bool _turnCompleted = false;
    private bool _turnAvailable = true;
    private List<int[][]> _prevStates;
    private List<int> _prevStateScores;
    private Stack<Tile> _tilesPool;
    private bool _winPopupShowed;
    private bool _needWinPopupShow;
    private bool _needMergeSound;
    private bool _needMoveSound;

    private int _score;
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            scoreTF.text = _score.ToString();
            scoreTF.Commit();
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

    private SoundController _soundController;
    public SoundController soundController
    {
        get
        {
            if (_soundController == null)
                _soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
            return _soundController;
        }
    }

    public void restart()
    {
        GameManager.GAME_RUNNING = true;
        _needMoveSound = false;
        _needMergeSound = false;
        _winPopupShowed = false;
        _needWinPopupShow = false;
        score = 0;
        bestScoreTF.text = UserData.bestScore.ToString();
        bestScoreTF.Commit();

        _prevStates = new List<int[][]>();
        _prevStateScores = new List<int>();

        if (_tiles != null)
        {
            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
                {
                    if (_tiles[i][j] != null)
                    {
                        _tiles[i][j].gameObject.SetActive(false);
                        _tilesPool.Push(_tiles[i][j]);
                        _tiles[i][j] = null;
                    }
                }
            }
        }

        _tiles = new Tile[FIELD_SIZE][];
        for (int i = 0; i < FIELD_SIZE; i++)
        {
            _tiles[i] = new Tile[FIELD_SIZE];
        }

        _freeCellsCnt = FIELD_SIZE * FIELD_SIZE;

        if (debugFill)
        {
            for (int i = 0; i < 15; i++)
                _addTile();

            int number = 0;
            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
                {
                    if (_tiles[i][j] != null)
                        _tiles[i][j].curNumber = (number++) % 12;
                }
            }
        }
        else
        {
            _addTile();
            _addTile();
        }

        _rememberState();
    }

    private void _gameOver()
    {
        UserData.bestScore = score;
        gameManager.showGameOver(score);
    }
        
    private IEnumerator _delayedAddTile()
    {
        if (_needMergeSound)
            soundController.playSound(soundController.merge);
        else if (_needMoveSound)
            soundController.playSound(soundController.move);

        _needMoveSound = false;
        _needMergeSound = false;

        yield return new WaitForSeconds(Tile.MOVE_TWEEN_DURATION);
        _addTile();
        _rememberState();
        yield return new WaitForSeconds(Tile.CREATE_TWEEN_DURATION);
        _turnAvailable = true;
        if (_needWinPopupShow)
        {
            gameManager.showWinpopup(score);
            _needWinPopupShow = false;
        }
    }

    void Start()
    {
        optionsBtn.OnClick += showOptions;
    }

    void showOptions()
    {
        gameManager.showOptions();
    }

    public void init()
    {
        _tilesPool = new Stack<Tile>();
        Tile tile;

        for (int i = 0; i < FIELD_SIZE * FIELD_SIZE * 2; i++)
        {
            tile = Instantiate(Resources.Load("Tile", typeof(Tile))) as Tile;
            tile.gameObject.transform.parent = gameObject.transform;
            tile.gameObject.SetActive(false);
            _tilesPool.Push(tile);
        }
    }

    public void show()
    {
        gameObject.SetActive(true);
        HUD.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
        HUD.SetActive(false);
    }
        
    void Update()
    {
        if (GameManager.GAME_RUNNING == false)
            return;

        if (_turnAvailable == false)
            return;

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SwipeManager.swipeDirection = SwipeManager.Swipe.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SwipeManager.swipeDirection = SwipeManager.Swipe.Right;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            SwipeManager.swipeDirection = SwipeManager.Swipe.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SwipeManager.swipeDirection = SwipeManager.Swipe.Down;
        #endif

        _turnCompleted = false;

        if (SwipeManager.swipeDirection != SwipeManager.Swipe.None)
        {
            _turnAvailable = false;
            _moveField(SwipeManager.swipeDirection);
            if (_turnCompleted)
                StartCoroutine(_delayedAddTile());
            else
                _turnAvailable = true;
        }

        if (_freeCellsCnt == 0 && _checkTurnsEnabled() == false)
            _gameOver();

        if (Input.GetKeyDown(KeyCode.R))
            restart();
    }

    private void _rememberState()
    {
        int[][] state = new int[FIELD_SIZE][];
        for (int i = 0; i < FIELD_SIZE; i++)
        {
            state[i] = new int[FIELD_SIZE];
            for (int j = 0; j < FIELD_SIZE; j++)
            {
                state[i][j] = _tiles[i][j] == null ? -1 : _tiles[i][j].curNumber;
            }
        }
        _prevStates.Add(state);
        _prevStateScores.Add(score);
    }

    public bool undoStep()
    {
        if (_prevStates.Count <= 1)
            return false;

        _prevStates.RemoveAt(_prevStates.Count - 1);
        _prevStateScores.RemoveAt(_prevStateScores.Count - 1);

        int[][] state = _prevStates[_prevStates.Count - 1];
        score = _prevStateScores[_prevStateScores.Count - 1];

        for (int i = 0; i < FIELD_SIZE; i++)
        {
            for (int j = 0; j < FIELD_SIZE; j++)
            {
                if (_tiles[i][j] != null) // TODO optimaze
                {
                    _tiles[i][j].gameObject.SetActive(false);
                    _tilesPool.Push(_tiles[i][j]);
                    _freeCellsCnt++;
                    _tiles[i][j] = null;
                }

                if (state[i][j] != -1)
                {
                    Tile tile = _tilesPool.Pop();
                    tile.init(new IntVector2(i, j), state[i][j]);
                    _freeCellsCnt--;
                    _tiles[i][j] = tile;
                }
            }
        }

        return true;
    }

    private bool _checkTurnsEnabled()
    {
        for (int i = 0; i < FIELD_SIZE; i++)
        {
            for (int j = 0; j < FIELD_SIZE; j++)
            {
                if (i != FIELD_SIZE - 1 && _tiles[i][j].curNumber == _tiles[i + 1][j].curNumber)
                    return true;
                if (j != FIELD_SIZE - 1 && _tiles[i][j].curNumber == _tiles[i][j + 1].curNumber)
                    return true;
            }
        }

        return false;
    }

    private void _moveField(SwipeManager.Swipe swipeDirection)
    {
        switch (swipeDirection)
        {
            case SwipeManager.Swipe.Up:
                _moveUp();
                break;
            case SwipeManager.Swipe.Down:
                _moveDown();
                break;
            case SwipeManager.Swipe.Left:
                _moveLeft();
                break;
            case SwipeManager.Swipe.Right:
                _moveRight();
                break;
        }
    }

    private void _moveUp()
    {
        IntVector2 position = new IntVector2();
        IntVector2 prevPosition = new IntVector2();

        for (int i = 0; i < FIELD_SIZE; i++)
        {
            for (int j = 0; j < FIELD_SIZE; j++)
            {
                if (_tiles[i][j] != null)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        position.x = i;
                        position.y = k;

                        prevPosition.x = i;
                        prevPosition.y = k + 1;

                        if (_processStep(_tiles[i][j], _tiles[i][k], position, prevPosition, k == 0))
                            break;
                    }
                }
            }
        }
    }

    private void _moveDown()
    {
        IntVector2 position = new IntVector2();
        IntVector2 prevPosition = new IntVector2();

        for (int i = FIELD_SIZE - 1; i >= 0; i--)
        {
            for (int j = FIELD_SIZE - 1; j >= 0; j--)
            {
                if (_tiles[i][j] != null)
                {
                    for (int k = j + 1; k < FIELD_SIZE; k++)
                    {
                        position.x = i;
                        position.y = k;

                        prevPosition.x = i;
                        prevPosition.y = k - 1;

                        if (_processStep(_tiles[i][j], _tiles[i][k], position, prevPosition, k == FIELD_SIZE - 1))
                            break;
                    }
                }
            }
        }
    }

    private void _moveLeft()
    {
        IntVector2 position = new IntVector2();
        IntVector2 prevPosition = new IntVector2();

        for (int i = 0; i < FIELD_SIZE; i++)
        {
            for (int j = 0; j < FIELD_SIZE; j++)
            {
                if (_tiles[i][j] != null)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        position.x = k;
                        position.y = j;

                        prevPosition.x = k + 1;
                        prevPosition.y = j;

                        if (_processStep(_tiles[i][j], _tiles[k][j], position, prevPosition, k == 0))
                            break;
                    }
                }
            }
        }
    }

    private void _moveRight()
    {
        IntVector2 position = new IntVector2();
        IntVector2 prevPosition = new IntVector2();

        for (int i = FIELD_SIZE - 1; i >= 0; i--)
        {
            for (int j = FIELD_SIZE - 1; j >= 0; j--)
            {
                if (_tiles[i][j] != null)
                {
                    for (int k = i + 1; k < FIELD_SIZE; k++)
                    {
                        position.x = k;
                        position.y = j;

                        prevPosition.x = k - 1;
                        prevPosition.y = j;
                        if (_processStep(_tiles[i][j], _tiles[k][j], position, prevPosition, k == FIELD_SIZE - 1))
                            break;
                    }
                }
            }
        }
    }

    private void _printField()
    {
        for (int i = 0; i < FIELD_SIZE; i++)
        {
            string s = "";
            for (int j = 0; j < FIELD_SIZE; j++)
                s += _tiles[j][i] == null ? "X" : _tiles[j][i].curNumber.ToString();
            Debug.Log(s);
        }
    }

    private bool _processStep(Tile tileFrom, Tile tileTo, IntVector2 position, IntVector2 prevPosition, bool lastCell = false)
    {
        if (tileTo == null)
        {
            if (lastCell)
            {
                _tiles[tileFrom.position.x][tileFrom.position.y] = null;
                tileFrom.position = position;
                _tiles[tileFrom.position.x][tileFrom.position.y] = tileFrom;
                _turnCompleted = true;
                _needMoveSound = true;
                return true;
            }
            else
                return false;
        }
        else if (tileTo.mergedInTurn == false && tileTo.curNumber == tileFrom.curNumber)
        {
            tileTo.curNumber++;
            if (_winPopupShowed == false && tileTo.curNumber == 10) // reach 2048 tile
            {
                _winPopupShowed = true;
                _needWinPopupShow = true;
            }
            score += Tile.NUMBERS[tileTo.curNumber];
            _tiles[tileFrom.position.x][tileFrom.position.y] = null;
            tileFrom.tweenAndDisable(position, _tilesPool);
            _freeCellsCnt++;
            _turnCompleted = true;
            _needMergeSound = true;
            return true;
        }
        else if (tileFrom.position.isEqual(prevPosition) == false)
        {
            _tiles[tileFrom.position.x][tileFrom.position.y] = null;
            tileFrom.position = prevPosition;
            _tiles[tileFrom.position.x][tileFrom.position.y] = tileFrom;
            _turnCompleted = true;
            _needMoveSound = true;
            return true;
        }

        return true;
    }
        
    private void _addTile()
    {
        int position = Random.Range(0, _freeCellsCnt);
        _freeCellsCnt--;
        for (int i = 0; i < FIELD_SIZE; i++)
        {
            for (int j = 0; j < FIELD_SIZE; j++)
            {
                if (_tiles[i][j] == null)
                {
                    position--;
                    if (position == -1)
                    {
                        Tile tile = _tilesPool.Pop();
                        int type = Random.Range(0, 10) == 0 ? 1 : 0; // 2 - 90%, 4 - 10%
                        tile.init(new IntVector2(i, j), type);
                        _tiles[i][j] = tile;
                        break;
                    }
                }
            }
            if (position == -1)
                break;
        }
    }
}
