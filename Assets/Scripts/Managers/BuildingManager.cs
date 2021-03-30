using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    Building buildingToBuild = null;

    [SerializeField] GameObject buildingGhostPrefab;
    BuildingGhost buildingGhost = null;

    Transform buildingsParent;

    bool canBuildHere = false;
    bool lastCanBuildHere = true;

    bool buildModeActive = false;

    public void DestroyBuilding(BuildingComponent bc)
    {
        foreach (Tile tile in bc.TakenTiles)
        {
            TileManager.Instance.SetTilesHasBuildingVariable(tile, false);
        }

        Vector2Int pos = new Vector2Int((int)bc.gameObject.transform.position.x, (int)bc.gameObject.transform.position.y);
        SaveManager.Instance.RemoveBuildingFromSave(pos);

        Destroy(bc.gameObject);
    }

	public void ActivateBuildMode(Building building)
    {
        buildModeActive = true;
        buildingToBuild = building;
        buildingGhost.gameObject.SetActive(true);
        buildingGhost.SetGhost(building);

        InputManager.Instance.OnLeftMouseClicked += OnLeftMouseClicked;
        InputManager.Instance.OnRightMouseClicked += DeactivateBuildMode;
    }

	public void DeactivateBuildMode()
    {
        buildModeActive = false;
        buildingToBuild = null;
        buildingGhost.gameObject.SetActive(false);

        InputManager.Instance.OnLeftMouseClicked -= OnLeftMouseClicked;
        InputManager.Instance.OnRightMouseClicked -= DeactivateBuildMode;
    }

    public void BuildFromSaveGame()
    {
        SaveData save = SaveManager.Instance.CurrentSave;

        for (int i = 0; i < save.builtBuildings.Count; i++)
        {
            Build(save.builtBuildingsPositions[i], save.builtBuildings[i]);
        }
    }

    void CheckIfBuildingAvailable()
    {
        canBuildHere = CanBuildHere(InputManager.Instance.RoundedMousePosition);

        if(canBuildHere != lastCanBuildHere)
        {
            InitiateGhostColorChange(canBuildHere);
        }

        lastCanBuildHere = canBuildHere;
    }

    void InitiateGhostColorChange(bool canBuildHere)
    {
        if (canBuildHere)
            buildingGhost.SetGhostColorToNormal();
        else
            buildingGhost.SetGhostColorRed();
    }

    bool CanBuildHere(Vector2Int buildPosition)
    {
        if (Application.isFocused == false)
            return false;

        if (TileManager.Instance.IsRangeWithinTilemap(buildPosition, buildingToBuild.GetSizeX, 
            buildingToBuild.GetSizeY) == false)
            return false;

        if (TileManager.Instance.AreTilesWithinRangeFree(buildPosition, buildingToBuild.GetSizeX, 
            buildingToBuild.GetSizeY) == false)
            return false;

        return true;
    }

    void OnLeftMouseClicked()
    {
        if (buildModeActive)
            Build();
    }

    /// <summary>
    /// If building from mouse click, leave all variables as null.
    /// If building from save or otherwise, set all variables.
    /// </summary>
    /// <param name="buildPosition"></param>
    /// <param name="building"></param>
    void Build(Vector2Int? buildPosition = null, Building building = null)
    {
        if (canBuildHere == false && building == null)
            return;

        if(buildPosition.HasValue == false || buildPosition == null)
            buildPosition = InputManager.Instance.RoundedMousePosition;

        if (building == null)
            building = buildingToBuild;

        CreateBuildingObject(building, buildPosition.Value);
        TileManager.Instance.SetTilesHasBuildingVariable(buildPosition.Value, building.GetSizeX,
            building.GetSizeY, true);

        SaveManager.Instance.AddBuildingToSave(building, buildPosition.Value);
    }

    void CreateBuildingObject(Building building, Vector2Int position)
    {
        Vector3 buildPos = new Vector3(position.x, position.y, 0);
        GameObject buildingObj = Instantiate(building.BuildingPrefab, buildPos, Quaternion.identity, buildingsParent);

        SetBuildingComponentTakenTiles(buildingObj, building);
    }

    void SetBuildingComponentTakenTiles(GameObject buildingObj, Building building)
    {
        BuildingComponent bc = buildingObj.GetComponent<BuildingComponent>();

        for (int x = (int)buildingObj.transform.position.x; x < (int)buildingObj.transform.position.x + 
            building.GetSizeX; x++)
        {
            for (int y = (int)buildingObj.transform.position.y; y < (int)buildingObj.transform.position.y + 
                building.GetSizeY; y++)
            {
                bc.TakenTiles.Add(TileManager.Instance.GetTileAt(new Vector2Int(x, y)));
            }
        }
    }

    void CreateBuildingGhost()
    {
        GameObject g = Instantiate(buildingGhostPrefab, Vector2.zero, Quaternion.identity, buildingsParent);
        buildingGhost = g.GetComponent<BuildingGhost>();
        buildingGhost.gameObject.SetActive(false);
    }

    void CreateBuildingParent()
    {
        buildingsParent = new GameObject("BuildingsParent").transform;
        buildingsParent.parent = this.transform;
    }

    IEnumerator LoadSaveAtTheEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        BuildFromSaveGame();
    }

	void Awake()
	{
		Instance = this;

        CreateBuildingParent();
        CreateBuildingGhost();

        buildingGhost.OnPositionChanged += CheckIfBuildingAvailable;

        StartCoroutine(LoadSaveAtTheEndOfFrame());
	}

    private void OnDestroy()
    {
        DeactivateBuildMode();
    }
}