using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data")]
public class LevelData : ScriptableObject
{
    public LevelStatus DefaultLevelStatus = new LevelStatus();
    public IntPOT TargetValue;
    [Tooltip("X = 2 stars, Y = 1 star, Z = 0 stars")]
    public int3 StarMoves;
    public Coords Dimensions;
    public List<BoardCellData> CellsData;
}
