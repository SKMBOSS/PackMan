using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    [SerializeField] Tile[] _tiles;
    [SerializeField] List<Tile> _bigFoodTiles;

    private const int _maxSizeY = 31;
    private const int _maxSizeX = 28;

    public int MaxSizeY { get { return _maxSizeY; } }
    public int MaxSizeX { get { return _maxSizeX; } }

   
    public Tile GetTile(int y, int x)
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

    public void LoadOriginalMap(out int foodCount)
    {
        List<int> tileTypes = FileIO.LoadJsonFile<ListSerialization<int>>(Application.persistentDataPath + @"/Maps/OriginalMap", "OriginalMap").ToList();
        int currentTileNum = 0;
        foodCount = 0;

        StopBlinkBigFood();    //디버그용 버튼 때문에 추가
        _bigFoodTiles.Clear(); //디버그용 버튼 때문에 추가

        foreach (int tileType in tileTypes)
        {
            if (tileType == (int)TileType.Food)
            {
                foodCount++;
            }

            else if (tileType == (int)TileType.BigFood)
            {
                _bigFoodTiles.Add(_tiles[currentTileNum]);
                foodCount++;
            }

            _tiles[currentTileNum++].TileType = ((TileType)tileType);
        }
        SetConnectedInfo();
    }

    public void LoadTilesFromCustomFile(string fileName, out int foodCount)
    {
        List<int> tileTypes = FileIO.LoadJsonFile<ListSerialization<int>>(Application.persistentDataPath + @"/Maps", fileName).ToList();
        int currentTileNum = 0;
        foodCount = 0;

        StopBlinkBigFood();    //디버그용 버튼 때문에 추가 
        _bigFoodTiles.Clear(); //디버그용 버튼 때문에 추가

        foreach (int tileType in tileTypes)
        {
            if (tileType == (int)TileType.Food)
            {
                foodCount++;
            }

            else if (tileType == (int)TileType.BigFood)
            {
                _bigFoodTiles.Add(_tiles[currentTileNum]);
                foodCount++;
            }

            _tiles[currentTileNum++].TileType = ((TileType)tileType);
        }
        SetConnectedInfo();
    }

    public void SetTargetToBlankTile(int y, int x)
    {
        GetTile(y, x).TileType = TileType.Blank;
    }

    private void SetConnectedInfo()
    {
        for (int y = 0; y < MaxSizeY; y++)
        {
            for (int x = 0; x < MaxSizeX; x++)
            {
                if ((x + 1 < MaxSizeX) && !GetTile(y, x + 1).IsObstacle)
                {
                    GetTile(y, x).ConnctedTilesInfo.Add(ConnectedTile.Right);
                }

                if ((x - 1 > 0) && !GetTile(y, x - 1).IsObstacle)
                {
                    GetTile(y, x).ConnctedTilesInfo.Add(ConnectedTile.Left);
                }

                if ((y - 1 > 0) && !GetTile(y - 1, x).IsObstacle)
                {
                    GetTile(y, x).ConnctedTilesInfo.Add(ConnectedTile.Up);
                }

                if ((y + 1 < MaxSizeY) && !GetTile(y + 1, x).IsObstacle)
                {
                    GetTile(y, x).ConnctedTilesInfo.Add(ConnectedTile.Down);
                }
            }
        }
    }

    public void ClearConnectedInfoInTiles()
    {
        foreach (Tile tile in _tiles)
        {
            tile.ClearConnctedTilesInfo();
        }
    }

    public void StartBlinkBigFood()
    {
        foreach (Tile tile in _bigFoodTiles)
        {
            tile.StartBlinkSprite();
        }
    }

    public void StopBlinkBigFood()
    {
        foreach (Tile tile in _bigFoodTiles)
        {
            tile.StopBlinkSprite();
        }
    }

    public void StartChangeMapColor()
    {
        foreach (Tile tile in _tiles)
        {
            tile.StartChangeSpriteColor();
        }
    }

    public void StopChangeMapColor()
    {
        foreach (Tile tile in _tiles)
        {
            tile.StopChangeSpriteColor();
        }
    }

    public void RemoveBlinkFoodInBigFoodTiles(Tile tile)
    {
        tile.StopBlinkSprite();
        _bigFoodTiles.Remove(tile);
    }


    ////Use When Tiles Gone
    //private void OnValidate()
    //{
    //    LoadTileByLoader();
    //}

    //private void LoadTileByLoader()
    //{
    //    int s = 0;
    //    Vector2 tilePos = transform.position;
    //    //Vector2 tilePos = this.transform.position;
    //    AssetLoader.Load("Tiles");

    //    for (int y = 0; y < MaxSizeY; y++)
    //    {
    //        for (int x = 0; x < MaxSizeX; x++)
    //        {
    //            AssetLoader.Instance("Tile", tilePos, this.transform);
    //            s++;
    //            tilePos.x++;
    //        }
    //        tilePos.y--;
    //        tilePos.x = transform.position.x;
    //    }
    //    Debug.Log(s);
    //    _tiles = GetComponentsInChildren<Tile>();


    //    for (int y = 0; y < MaxSizeY; y++)
    //    {
    //        for (int x = 0; x < MaxSizeX; x++)
    //        {
    //            Tiles(y, x).gameObject.name = "Tile(" + y + "," + x + ")";
    //        }

    //    }

    //}

}