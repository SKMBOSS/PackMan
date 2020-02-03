using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditTileMap : MonoBehaviour
{
    [SerializeField] EditTile[] _tiles;

	private const int _maxSizeY = 31;
	private const int _maxSizeX = 28;

	private EditTile GetTile(int y, int x)
	{
		if (x < 0 || x >= _maxSizeX)
		{
			Debug.LogError("x is Out Of Range");
			return null;
		}
		if (y < 0 || y >= _maxSizeY)
		{
			Debug.LogError("y is Out Of Range");
			return null;
		}

		return _tiles[y * _maxSizeX + x];
	}

	public List<int> GetTileTypes()
    {
        List<int> _tileTypes = new List<int>();

        foreach (EditTile tile in _tiles)
        {
            _tileTypes.Add((int)tile.TileType);
        }

        return _tileTypes;
    }

    public void Save(string fileName)
    {
        string jsonData = FileIO.ObjectToJson(new ListSerialization<int>(GetTileTypes()));

        FileIO.CreateJsonFile(Application.persistentDataPath + @"/Maps", fileName, jsonData);
    }

    public void Load(string fileName)
    {
        List<int> tileTypes = FileIO.LoadJsonFile<ListSerialization<int>>(Application.persistentDataPath + @"/Maps", fileName).ToList();
        int currentTileNum = 0;

        foreach (int tileType in tileTypes)
        {
            _tiles[currentTileNum++].SetTileInfoForLoad((TileType)tileType);
        }
    }

    public void LoadOriginalMap()
    {
        List<int> tileTypes = FileIO.LoadJsonFile<ListSerialization<int>>(Application.persistentDataPath + @"/Maps/OriginalMap", "OriginalMap").ToList();
        int currentTileNum = 0;

        foreach (int tileType in tileTypes)
        {
            _tiles[currentTileNum++].SetTileInfoForLoad((TileType)tileType);
        }
    }

    public void ChangeAllTileBlank()
    {
        foreach (EditTile tile in _tiles)
        {
            if (tile.IsTouchAble)
            {
                tile.SetTileInfoForLoad(TileType.Blank);
            }
        }
    }
}
