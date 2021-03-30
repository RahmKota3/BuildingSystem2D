using UnityEngine;

enum TileType { Grass, Field }

public class Tile
{
    public TileComponent TileComponentReference;
    public Vector2Int Position;

    public bool HasBuilding = false;

    public Tile(TileComponent tileComponent, Vector2Int position)
    {
        TileComponentReference = tileComponent;
        Position = position;
    }
}