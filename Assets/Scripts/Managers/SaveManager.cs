using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

	Dictionary<Vector2Int, int> buildingPositionAndIndex = new Dictionary<Vector2Int, int>();

	public SaveData CurrentSave { get; private set; }

	public void AddBuildingToSave(Building building, Vector2Int position)
    {
		if (CurrentSave.builtBuildingsPositions.Contains(position) == false)
		{
			CurrentSave.builtBuildings.Add(building);
			CurrentSave.builtBuildingsPositions.Add(position);
			buildingPositionAndIndex[position] = CurrentSave.builtBuildingsPositions.Count - 1;
		}
		else
		{
			buildingPositionAndIndex[position] = CurrentSave.builtBuildingsPositions.IndexOf(position);
		}
	}

	public void RemoveBuildingFromSave(Vector2Int position)
	{
		int index = buildingPositionAndIndex[position];
		CurrentSave.builtBuildings.RemoveAt(index);
		CurrentSave.builtBuildingsPositions.RemoveAt(index);
		buildingPositionAndIndex.Remove(position);

        for (int i = index; i < CurrentSave.builtBuildingsPositions.Count; i++)
        {
			buildingPositionAndIndex[CurrentSave.builtBuildingsPositions[i]] = i;
        }
    }

	public void SaveGame()
    {
		string json = JsonUtility.ToJson(CurrentSave);

		File.WriteAllText(Application.persistentDataPath + "/" + Globals.SaveFileName, json);

		Debug.Log("Game saved");
    }

	public void DeleteSave()
    {
		File.Delete(Application.persistentDataPath + "/" + Globals.SaveFileName);

		Debug.Log("Save deleted");
    }

	void CreateSaveData()
    {
		CurrentSave = new SaveData();

		SaveGame();
    }

	void LoadSaveData()
    {
		string saveString = File.ReadAllText(Application.persistentDataPath + "/" + Globals.SaveFileName);
		CurrentSave = JsonUtility.FromJson<SaveData>(saveString);

		Debug.Log("Save loaded");
    }

	bool SaveFileExists()
    {
		return File.Exists(Application.persistentDataPath + "/" + Globals.SaveFileName);
    }
	
	void Awake()
	{
		Instance = this;

		if (SaveFileExists())
			LoadSaveData();
		else
			CreateSaveData();
	}

    private void Update()
    {
		// TODO: Debug
		if (Input.GetKeyDown(KeyCode.S))
			SaveGame();

		if (Input.GetKeyDown(KeyCode.Delete))
			DeleteSave();
    }
}
