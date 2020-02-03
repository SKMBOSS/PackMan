using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum TileType
{
    Blank,
    Food,
    BigFood,
    Obstacle1, Obstacle2, Obstacle3, Obstacle4,
    Obstacle5, Obstacle6, Obstacle7, Obstacle8,
    Obstacle9, Obstacle10, Obstacle11, Obstacle12,
    Obstacle13, Obstacle14, Obstacle15, Obstacle16,
    Obstacle17, Obstacle18, Obstacle19, Obstacle20,
    Obstacle21, Obstacle22, Obstacle23, Obstacle24,
    Obstacle25, Obstacle26, Obstacle27, Obstacle28,
    Obstacle29, Obstacle30, Obstacle31, Obstacle32,
    Obstacle33, Obstacle34, Obstacle35, Obstacle36,
    MonsterEntrance,
}


public class SelectedTileSaver : MonoBehaviour
{
	private static SelectedTileSaver _instance;
	public static SelectedTileSaver Instance { get { return _instance; } }
	private void Awake() { _instance = this; }

	[SerializeField] Image[] _tileSprites;
    [Space]
    [SerializeField] Image[] _ButtonBackImages;
    [Space]
    [SerializeField] Sprite _currentTileSprite;
    [SerializeField] TileType _tileType;

    private bool _isClicked = false;
  
    public Sprite CurrentTileSprite { get { return _currentTileSprite; } set { _currentTileSprite = value; } }
    public TileType TileType { get { return _tileType; } }
    public bool IsClicked { get { return _isClicked; } set { _isClicked = value; } }

    private void Start()
    {
        SetSelectedTileSprite(TileType.Blank);
    }

    public void SetSelectedTileSprite(TileType tileType)
    {
        foreach (Image buttonBackImage in _ButtonBackImages)
        {
            buttonBackImage.enabled = false;
        }
        _currentTileSprite =  _tileSprites[(int)tileType].sprite;
        _tileType = tileType;
    }

    public Sprite GetTileSpriteByTileType(TileType tileType)
    {
        return _tileSprites[(int)tileType].sprite;
    }
}
