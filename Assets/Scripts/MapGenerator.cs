using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    void GenerateMap()
    {
        for (int x = 0; x < TileManager.Instance.CurrentMapData.MapWidth; x++)
        {
            for (int y = 0; y < TileManager.Instance.CurrentMapData.MapHeight; y++)
            {
                TileManager.Instance.CreateTile(new Vector2Int(x, y));
            }
        }
    }

    private void Start()
    {
        GenerateMap();
    }
}
