using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	[Header("DEBUG")] [SerializeField] bool drawGizmos = true;

	public MapData CurrentMapData;
	[SerializeField] GameObject tilePrefab;

	Transform tileParent;

	Tile[,] tiles;

    public static TileManager Instance;

	public bool IsRangeWithinTilemap(Vector2Int startingPos, int rangeX, int rangeY)
	{
		if (startingPos.x < 0 || startingPos.x + (rangeX - 1) >= CurrentMapData.MapWidth)
			return false;

		if (startingPos.y < 0 || startingPos.y + (rangeY - 1) >= CurrentMapData.MapHeight)
			return false;

		return true;
	}

	public bool AreTilesWithinRangeFree(Vector2Int startingPos, int rangeX, int rangeY)
    {
		for (int x = startingPos.x; x < startingPos.x + rangeX; x++)
		{
			for (int y = startingPos.y; y < startingPos.y + rangeY; y++)
			{
				if (tiles[x, y].HasBuilding)
					return false;
			}
		}

		return true;
	}

	public void SetTilesHasBuildingVariable(Tile tile, bool hasBuilding)
	{
		tile.HasBuilding = hasBuilding;
	}

	public void SetTilesHasBuildingVariable(Vector2Int startingTilePos, int buildingSizeX, int buildingSizeY, bool hasBuilding)
    {
        for (int x = startingTilePos.x; x < startingTilePos.x + buildingSizeX; x++)
        {
            for (int y = startingTilePos.y; y < startingTilePos.y + buildingSizeY; y++)
            {
				tiles[x, y].HasBuilding = hasBuilding;
            }
        }
    }

	public Tile GetTileAt(Vector2Int position)
    {
		return tiles[position.x, position.y];
    }

	public Tile CreateTile(Vector2Int position)
    {
		GameObject tileObj = Instantiate(tilePrefab, new Vector3(position.x, position.y), Quaternion.identity, tileParent);
		
		tiles[position.x, position.y] = new Tile(tileObj.GetComponent<TileComponent>(), position);

		tileObj.name = $"Tile[{position.x}, {position.y}]";

		return tiles[position.x, position.y];
    }

	void CreateTileParent()
    {
		tileParent = new GameObject("TileParent").transform;
    }

	void InitializeVariables()
    {
		tiles = new Tile[CurrentMapData.MapWidth, CurrentMapData.MapHeight];
    }

	void Awake()
	{
		Instance = this;

		InitializeVariables();
		CreateTileParent();
	}

    private void OnDrawGizmos()
    {
		if (Application.isPlaying == false)
			return;

		if (drawGizmos == false)
			return;

        foreach (Tile tile in tiles)
        {
			if (tile.HasBuilding == false)
				Gizmos.color = Color.white;
			else
				Gizmos.color = Color.red;

			Gizmos.DrawWireCube(new Vector3(tile.Position.x, tile.Position.y), new Vector3(0.9f, 0.9f));
        }
    }
}