using Core;
using Core.Settings;
using Unity.Mathematics;
using UnityEngine;

public class LevelConstructor : DIBehaviour
{
    public GameSettings GameSettings;
    public ConstructorBlock BlockPrefab;
    
    [Space]
    
    public IntPOT TargetNumber;
    [Tooltip("X = 0 stars, Y = 1 star, Z = 2 stars")]
    public int3 StarMoves;
    public LevelConfig CurrentLevel { get; set; }
}