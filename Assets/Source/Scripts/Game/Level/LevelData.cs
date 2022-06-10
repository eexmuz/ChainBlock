using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data")]
public class LevelData : ScriptableObject
{
    public LevelStatus DefaultLevelStatus = new LevelStatus();
    public IntPOT TargetValue;
    [Tooltip("X = 0 stars, Y = 1 star, Z = 2 stars")]
    public int3 StarMoves;
    public int2 Dimensions;
    public List<BoardCellData> CellsData;

    public int LevelIndex { get; set; }
}
