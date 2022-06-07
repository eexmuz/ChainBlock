using System.Collections.Generic;
using UnityEngine;
using Utility;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data")]
public class LevelData : ScriptableObject
{
    public Coords Dimensions;
    public List<BoardCellData> BoardData;
}
