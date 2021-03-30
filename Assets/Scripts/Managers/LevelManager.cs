using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelType { Loader, GameplayScene }

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	[Tooltip("Load this scene at the end of the first frame.")]
	public LevelType StartScene;

	[Tooltip("List of scene types and their names.")]
	public List<SceneNameAndEnum> ScenesAndTheirNames;

	Dictionary<LevelType, string> levelTypesAndTheirNames = new Dictionary<LevelType, string>();

	public System.Action OnBeforeSceneLoad;
	public System.Action OnAfterSceneLoad;

	public void LoadLevel(LevelType levelToLoad)
	{
		if(levelTypesAndTheirNames.ContainsKey(levelToLoad) == false)
        {
			Debug.LogError($"Level name not assigned to {levelToLoad}. Update ScenesAndTheirNames List.");
			return;
        }

		OnBeforeSceneLoad?.Invoke();

		SceneManager.LoadScene(levelTypesAndTheirNames[levelToLoad]);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		OnAfterSceneLoad?.Invoke();
    }

	IEnumerator LoadStartSceneAtTheEndOfFrame()
    {
		yield return new WaitForEndOfFrame();

		LoadLevel(StartScene);
    }

	void PopulateSceneTypeAndNameDictionary()
    {
        foreach (SceneNameAndEnum sceneEnumAndNamePair in ScenesAndTheirNames)
		{
			levelTypesAndTheirNames[sceneEnumAndNamePair.SceneEnum] = sceneEnumAndNamePair.SceneName;
        }
    }

	void Awake()
	{
		Instance = this;

		SceneManager.sceneLoaded += OnSceneLoaded;

		PopulateSceneTypeAndNameDictionary();

		StartCoroutine(LoadStartSceneAtTheEndOfFrame());
	}
}

[System.Serializable]
public class SceneNameAndEnum
{
	public LevelType SceneEnum;
	public string SceneName;
}