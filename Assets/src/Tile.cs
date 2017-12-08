using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class Tile : MonoBehaviour
{
    private const int TILE_WIDTH = 168;
    public const float MOVE_TWEEN_DURATION = 0.2f;
    public const float CREATE_TWEEN_DURATION = 0.2f;
    public const float MERGE_TWEEN_DURATION = 0.1f;

    static private Vector3 _createScale = new Vector3(0, 0, 0);
    static private Vector3 _normalScale = new Vector3(1, 1, 1);
    static private Vector3 _mergeScale = new Vector3(1.2f, 1.2f, 1);
    static private Vector2 FIRST_TILE_POSITION = new Vector2(- TILE_WIDTH * 1.5f, TILE_WIDTH * 1.5f - 62);
    static private Color[] COLORS = {
        new Color(247.0f / 255.0f, 242.0f / 255.0f, 222.0f / 255.0f),
        new Color(247.0f / 255.0f, 225.0f / 255.0f, 168.0f / 255.0f),
        new Color(247.0f / 255.0f, 212.0f / 255.0f, 121.0f / 255.0f),
        new Color(247.0f / 255.0f, 198.0f / 255.0f, 72.0f / 255.0f),
        new Color(247.0f / 255.0f, 190.0f / 255.0f, 37.0f / 255.0f),
        new Color(247.0f / 255.0f, 174.0f / 255.0f, 124.0f / 255.0f),
        new Color(247.0f / 255.0f, 153.0f / 255.0f, 86.0f / 255.0f),
        new Color(242.0f / 255.0f, 120.0f / 255.0f, 58.0f / 255.0f),
        new Color(240.0f / 255.0f, 97.0f / 255.0f, 26.0f / 255.0f),
        new Color(235.0f / 255.0f, 103.0f / 255.0f, 73.0f / 255.0f),
        new Color(235.0f / 255.0f, 76.0f / 255.0f, 40.0f / 255.0f),
        new Color(51.0f / 255.0f, 204.0f / 255.0f, 82.0f / 255.0f),
        new Color(45.0f / 255.0f, 173.0f / 255.0f, 109.0f / 255.0f)
    };
        
    static public int[] NUMBERS = {2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192};

    public tk2dSprite numberSprite;
    public tk2dSprite bgSprite;
    public bool mergedInTurn = false;

    private Sequence _sequence;
    private Stack<Tile> _tilesPool;

    private IntVector2 _position = new IntVector2(0, 0);
    public IntVector2 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            HOTween.To(gameObject.transform, MOVE_TWEEN_DURATION, "position", _getPosition(position));
        }
    }

    private int _curNumber;
    public int curNumber
    {
        get 
        {
            return _curNumber;
        }
        set
        {
            _curNumber = value;
            mergedInTurn = true;
            if (gameObject.activeSelf)
                StartCoroutine(_mergeAnimation());
        }
    }

    private IEnumerator _mergeAnimation()
    {
        yield return new WaitForSeconds(MOVE_TWEEN_DURATION);

        numberSprite.SetSprite(NUMBERS[_curNumber].ToString());
        bgSprite.color = COLORS[_curNumber];
        _sequence.Restart();
        mergedInTurn = false;
    }
        

    // Use this for initialization
    void Start()
    {
        _sequence = new Sequence();
        _sequence.Append(HOTween.To(gameObject.transform, MERGE_TWEEN_DURATION, "localScale", _mergeScale));
        _sequence.Append(HOTween.To(gameObject.transform, MERGE_TWEEN_DURATION, "localScale", _normalScale));
        _sequence.autoKillOnComplete = false;
    }
    // Update is called once per frame
    void Update()
    {
	
    }

    public void tweenAndDisable(IntVector2 newPosition, Stack<Tile> tilesPool)
    {
        _tilesPool = tilesPool;
        TweenParms parms = new TweenParms();
        parms.Prop("position", _getPosition(newPosition, -10));
        parms.OnComplete(_deactivateGO);
        HOTween.To(gameObject.transform, MOVE_TWEEN_DURATION, parms);
    }

    private void _deactivateGO()
    {
        gameObject.SetActive(false);
        _tilesPool.Push(this);
    }

    public void init(IntVector2 position, int number)
    {
        mergedInTurn = false;
        _curNumber = number;
        numberSprite.SetSprite(NUMBERS[_curNumber].ToString());
        bgSprite.color = COLORS[_curNumber];
        _position = position;
        gameObject.transform.position = _getPosition(position);
        gameObject.transform.localScale = _createScale;
        gameObject.SetActive(true);
        HOTween.To(gameObject.transform, CREATE_TWEEN_DURATION, "localScale", _normalScale);
    }

    private Vector3 _getPosition(IntVector2 position, float z = 0)
    {
        Vector3 transformPosition = new Vector3(0, 0, z);
        transformPosition.x = FIRST_TILE_POSITION.x + position.x * TILE_WIDTH;
        transformPosition.y = FIRST_TILE_POSITION.y - position.y * TILE_WIDTH;
        return transformPosition;
    }
}
