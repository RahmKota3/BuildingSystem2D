using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] BuildingComponent buildingComponent;

    public void OnClick()
    {
        BuildingManager.Instance.DestroyBuilding(buildingComponent);
    }
}
