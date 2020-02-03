using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoSaver :MonoBehaviour
{ 
	private static TileInfoSaver _instance;
	public static TileInfoSaver Instance { get { return _instance; } }
	private void Awake() { _instance = this; }

	[SerializeField] Sprite[] _tileSprites;

	public Sprite GetTileSpriteByTileType(TileType tileType)
	{
		return _tileSprites[(int)tileType];
	}
}
