using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    [SerializeField] Building buildingToBuild;

    public void OnClick()
    {
        BuildingManager.Instance.ActivateBuildMode(buildingToBuild);
    }
}
