using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectedTile
{
	Right,
	Left,
	Up,
	Down
}

public class Tile : MonoBehaviour
{
	SpriteRenderer _spriteRenderer;

	[SerializeField] TileType _tileType;
	[SerializeField] List<ConnectedTile> _connctedTilesInfo;

	[SerializeField]
	private bool _isObstacle = false;

	private Color _originalColor;
	private Color _changedTileColor;

	private const float _blinkTimeInterval = 0.2f;
	private const float _colorChangeTimeInterval = 0.2f;

	public TileType TileType { get { return _tileType; } set { _tileType = value; SetTileInfo(); } }
	public List<ConnectedTile> ConnctedTilesInfo { get { return _connctedTilesInfo; } set { _connctedTilesInfo = value; } }
	public bool IsObstacle { get { return _isObstacle;} }

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();

		_originalColor = new Color(1.0f, 1.0f, 1.0f);
		_changedTileColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
	}

	private void SetTileInfo()
	{
		_spriteRenderer.sprite = TileInfoSaver.Instance.GetTileSpriteByTileType(_tileType);

		if (_tileType == TileType.Blank || _tileType == TileType.Food || _tileType == TileType.BigFood)
			_isObstacle = false;
		else
			_isObstacle = true;
	}

    public float SqrMagnitude(Vector3 target)
    {
        Vector2 offSet = transform.position - target;
        return Vector2.SqrMagnitude(offSet);
    }

	public void StartBlinkSprite()
	{
		StartCoroutine(nameof(BlinkSprite));
	}

	public void StopBlinkSprite()
	{
		StopCoroutine(nameof(BlinkSprite));

		_spriteRenderer.enabled = true;
	}

	IEnumerator BlinkSprite()
	{
		yield return new WaitForSeconds(_blinkTimeInterval);

		if(_spriteRenderer.enabled == true)
		{
			_spriteRenderer.enabled = false;
		}
		else
		{
			_spriteRenderer.enabled = true;
		}
		StartCoroutine(nameof(BlinkSprite));
	}

	public void StartChangeSpriteColor()
	{
		StartCoroutine(nameof(ChangeSpriteColor));
	}

	public void StopChangeSpriteColor()
	{
		StopCoroutine(nameof(ChangeSpriteColor));

		_spriteRenderer.color = _originalColor;
	}

	IEnumerator ChangeSpriteColor()
	{
		yield return new WaitForSeconds(_colorChangeTimeInterval);

		if (_spriteRenderer.color == _originalColor)
		{
			_spriteRenderer.color = _changedTileColor;
		}
		else
		{
			_spriteRenderer.color = _originalColor;
		}
		StartCoroutine(nameof(ChangeSpriteColor));
	}

	public void ClearConnctedTilesInfo()
	{
		_connctedTilesInfo.Clear();
	}
}


