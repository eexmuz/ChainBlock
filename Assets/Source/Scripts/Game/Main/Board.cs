using System;
using System.Collections.Generic;
using Core;
using Core.Attributes;
using Core.Settings;
using UnityEngine;

public class Board : DIBehaviour
{
    [SerializeField]
    private LevelController _levelController;

    [SerializeField]
    private GameObject _boardModel;

    [Inject]
    private GameSettings _gameSettings;

    private Block[] _blocks;
    private Coords _dimensions;

    public void SetupBoard(LevelData levelData)
    {
        ClearBoard();
        
        _dimensions = levelData.Dimensions;
        _blocks = new Block[_dimensions.x * _dimensions.y];
        
        foreach (var cell in levelData.CellsData)
        {
            Block block = _levelController.BlocksPool.Spawn();
            block.SetBlock(cell.BlockInfo);
            block.transform.position = GetCellPosition(cell.Coords);

            _blocks[cell.Coords.Index(_dimensions.x)] = block;
        }

        _boardModel.transform.localScale = new Vector3(_dimensions.x * _gameSettings.CellSize.x,
            _dimensions.y * _gameSettings.CellSize.y, _boardModel.transform.localScale.z);
    }
    
    public void Swipe(Direction direction)
    {
    }

    private Vector2 GetCellPosition(Coords coords)
    {
        float x = coords.x * _gameSettings.CellSize.x - (_dimensions.x - 1) * _gameSettings.CellSize.x * .5f;
        float y = coords.y * _gameSettings.CellSize.y - (_dimensions.y - 1) * _gameSettings.CellSize.y * .5f;
        return new Vector2(x, y);
    }

    private void ClearBoard()
    {
        if (_blocks == null)
        {
            return;
        }

        for (int i = 0; i < _blocks.Length; i++)
        {
            _levelController.BlocksPool.Despawn(_blocks[i]);
        }

        _blocks = null;
    }
}