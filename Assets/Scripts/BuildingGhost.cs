using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] Color cantBuildHereColor = Color.red;
    [SerializeField] Color defaultColor = new Color(1, 1, 1, 0.5f);

    bool followRoundedMousePosition = false;

    Dictionary<Building, GameObject> buildingGhosts = new Dictionary<Building, GameObject>();
    Dictionary<Building, SpriteRenderer> buildingRenderers = new Dictionary<Building, SpriteRenderer>();

    GameObject activeGhost = null;
    SpriteRenderer activeGhostRenderer = null;

    Vector3 lastPosition = new Vector3(-999.9f, -999.9f);
    public System.Action OnPositionChanged;

    public void SetGhost(Building typeOfGhost)
    {
        if (buildingGhosts.ContainsKey(typeOfGhost))
        {
            activeGhost = buildingGhosts[typeOfGhost];
            activeGhostRenderer = buildingRenderers[typeOfGhost];
            activeGhost.SetActive(true);
        }
        else
        {
            activeGhost = CreateBuildingGhost(typeOfGhost);
        }
    }

    public void SetGhostColorRed()
    {
        if (activeGhost == null)
            return;

        activeGhostRenderer.color = cantBuildHereColor;
    }

    public void SetGhostColorToNormal()
    {
        if (activeGhost == null)
            return;

        activeGhostRenderer.color = defaultColor;
    }

    GameObject CreateBuildingGhost(Building typeOfBuilding)
    {
        GameObject g = Instantiate(typeOfBuilding.BuildingPrefab, Vector2.zero,
            Quaternion.identity, this.transform);
        g.transform.localPosition = Vector3.zero;

        buildingGhosts[typeOfBuilding] = g;

        SpriteRenderer sr = g.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
            sr = g.GetComponent<SpriteRenderer>();
        buildingRenderers[typeOfBuilding] = sr;
        activeGhostRenderer = sr;

        sr.color = defaultColor;
        sr.sortingLayerName = Globals.BuildingGhostSortingLayerName;

        Destroy(g.GetComponent<BuildingComponent>());
        Destroy(g.GetComponent<BuildingButton>());

        return g;
    }

    void FollowRoundedMousePosition()
    {
        transform.position = new Vector3(InputManager.Instance.RoundedMousePosition.x,
            InputManager.Instance.RoundedMousePosition.y);

        if (transform.position != lastPosition)
        {
            OnPositionChanged?.Invoke();
        }

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (followRoundedMousePosition)
            FollowRoundedMousePosition();
    }

    private void OnEnable()
    {
        followRoundedMousePosition = true;
    }

    private void OnDisable()
    {
        followRoundedMousePosition = false;

        if (activeGhost != null)
        {
            activeGhost.SetActive(false);
            activeGhost = null;
        }
    }
}
