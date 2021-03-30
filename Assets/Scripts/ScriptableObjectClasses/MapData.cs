using UnityEngine;

[CreateAssetMenu(fileName = "NewMapSettings", menuName = "ScriptableObjects/MapSettings", order = 0)]
public class MapData : ScriptableObject
{
    public int MapWidth = 100;
    public int MapHeight = 100;
}
