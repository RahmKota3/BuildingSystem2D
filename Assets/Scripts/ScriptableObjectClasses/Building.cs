using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "ScriptableObjects/Building", order = 0)]
public class Building : ScriptableObject
{
    public GameObject BuildingPrefab;

    [SerializeField] Vector2Int buildingGridSize = Vector2Int.one;

    public int GetSizeX { get { return buildingGridSize.x; } }
    public int GetSizeY { get { return buildingGridSize.y; } }
}
