using System;
using System.Collections.Generic;
using Core.Settings;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConstructor))]
public class LevelConstructorEditor : Editor
{
    private const string LEVELS_PATH = "Assets/Source/Content/Settings/Levels";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelConstructor constructor = (LevelConstructor) target;

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel(constructor);
        }

        if (GUILayout.Button("Load Level"))
        {
            LoadLevel(constructor);
        }

        if (GUILayout.Button("Create Block"))
        {
            CreateBlock(constructor);
        }
    }

    private void SaveLevel(LevelConstructor constructor)
    {
        string path = EditorUtility.SaveFilePanel("Save level", LEVELS_PATH, "New Level.asset", "asset");
        if (path.Length == 0)
        {
            return;
        }
        
        path = path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));

        LevelConfig levelAsset = AssetDatabase.LoadAssetAtPath<LevelConfig>(path);
        if (levelAsset == null)
        {
            levelAsset = CreateInstance<LevelConfig>();
            AssetDatabase.CreateAsset(levelAsset, path);
        }

        levelAsset.CellsData = GetLevelObjectsInfo();
        levelAsset.TargetValue = constructor.TargetNumber;
        levelAsset.StarMoves = constructor.StarMoves;
        levelAsset.Dimensions = new int2(5, 5);

        EditorUtility.SetDirty(levelAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        if (constructor.GameSettings.Levels.Contains(levelAsset) == false)
        {
            constructor.GameSettings.Levels.Add(levelAsset);
        }

        constructor.CurrentLevel = levelAsset;
    }

    private List<BoardCellData> GetLevelObjectsInfo()
    {
        ConstructorBlock[] blocks = FindObjectsOfType<ConstructorBlock>();
        List<BoardCellData> savedObjects = new List<BoardCellData>();
        
        foreach (ConstructorBlock block in blocks)
        {
            savedObjects.Add(new BoardCellData
            {
                Coords = block.BlockData.Coords,
                BlockInfo = new BlockInfo
                {
                    PowerOfTwo = block.BlockData.PowerOfTwo,
                    Movable = block.BlockData.Movable,
                    Mergeable = block.BlockData.Mergeable,
                }
            });
        }

        return savedObjects;
    }

    private void LoadLevel(LevelConstructor constructor)
    {
        string path = EditorUtility.OpenFilePanel("Load Level", LEVELS_PATH, "asset");
        if (path.Length == 0)
        {
            return;
        }

        path = path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));

        constructor.CurrentLevel = AssetDatabase.LoadAssetAtPath<LevelConfig>(path);
        if (constructor.CurrentLevel == null)
        {
            Debug.LogError("Can't load level");
            return;
        }

        var currentObjects = FindObjectsOfType<ConstructorBlock>();
        foreach (ConstructorBlock levelObject in currentObjects)
        {
            DestroyImmediate(levelObject.gameObject);
        }

        foreach (BoardCellData cellData in constructor.CurrentLevel.CellsData)
        {
            CreateBlock(constructor, cellData, constructor.CurrentLevel.Dimensions);
        }
    }

    private void CreateBlock(LevelConstructor constructor)
    {
        ConstructorBlock block = CreateBlock(constructor, new BoardCellData
        {
            Coords = new Coords(0, 0),
            BlockInfo = new BlockInfo
            {
                Mergeable = true,
                Movable = true,
                PowerOfTwo = 1,
            }
        }, new int2(5, 5));
        
        Selection.activeGameObject = block.gameObject;
        Undo.RegisterCreatedObjectUndo(block.gameObject, "Block Created");
    }

    private ConstructorBlock CreateBlock(LevelConstructor constructor, BoardCellData cellData, int2 dimensions)
    {
        ConstructorBlock createdObject = Instantiate(constructor.BlockPrefab, constructor.transform);
        createdObject.SetBlock(constructor.GameSettings, cellData);
        createdObject.transform.localPosition = GetBlockPosition(constructor.GameSettings, cellData.Coords, dimensions);
        return createdObject;
    }

    private Vector3 GetBlockPosition(GameSettings gameSettings, Coords coords, int2 dimensions)
    {
        float x = coords.x * gameSettings.CellSize.x - (dimensions.x - 1) * gameSettings.CellSize.x * .5f;
        float y = coords.y * gameSettings.CellSize.y - (dimensions.y - 1) * gameSettings.CellSize.y * .5f;
        return new Vector2(x, y);
    }
}
