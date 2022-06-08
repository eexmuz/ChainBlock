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
            block.Coords = cell.Coords;
            block.transform.position = GetCellPosition(cell.Coords);

            _blocks[cell.Coords.Index(_dimensions.x)] = block;
        }

        _boardModel.transform.localScale = new Vector3(_dimensions.x * _gameSettings.CellSize.x,
            _dimensions.y * _gameSettings.CellSize.y, _boardModel.transform.localScale.z);
    }
    
    public void Swipe(Direction direction)
    {
        Coords coords = new Coords();
        
        if (direction == Direction.Down)
        {
            for (coords.y = 1; coords.y < _dimensions.y; coords.y++)
            {
                for (coords.x = 0; coords.x < _dimensions.x; coords.x++)
                {
                    Block block = _blocks[coords.Index(_dimensions.x)];
                    if (block == null || block.Movable == false)
                    {
                        continue;
                    }

                    Block blockBelow = _blocks[Coords.Index(coords.x, coords.y - 1, _dimensions.x)];
                    if (blockBelow == null)
                    {
                        SwipeBlock(block, direction);
                    }
                    else
                    {
                        TryMergeBlocks(block, blockBelow);
                    }
                }
            }
        }
        if (direction == Direction.Up)
        {
            for (coords.y = _dimensions.y - 2; coords.y >= 0; coords.y--)
            {
                for (coords.x = 0; coords.x < _dimensions.x; coords.x++)
                {
                    Block block = _blocks[coords.Index(_dimensions.x)];
                    if (block == null || block.Movable == false)
                    {
                        continue;
                    }

                    Block blockAbove = _blocks[Coords.Index(coords.x, coords.y + 1, _dimensions.x)];
                    if (blockAbove == null)
                    {
                        SwipeBlock(block, direction);
                    }
                    else
                    {
                        TryMergeBlocks(block, blockAbove);
                    }
                }
            }
        }
        if (direction == Direction.Right)
        {
            for (coords.x = _dimensions.x - 2; coords.x >= 0; coords.x--)
            {
                for (coords.y = 0; coords.y < _dimensions.y; coords.y++)
                {
                    Block block = _blocks[coords.Index(_dimensions.x)];
                    if (block == null || block.Movable == false)
                    {
                        continue;
                    }

                    Block blockRight = _blocks[Coords.Index(coords.x + 1, coords.y, _dimensions.x)];
                    if (blockRight == null)
                    {
                        SwipeBlock(block, direction);
                    }
                    else
                    {
                        TryMergeBlocks(block, blockRight);
                    }
                }
            }
        }
        if (direction == Direction.Left)
        {
            for (coords.x = 1; coords.x < _dimensions.y; coords.x++)
            {
                for (coords.y = 0; coords.y < _dimensions.y; coords.y++)
                {
                    Block block = _blocks[coords.Index(_dimensions.x)];
                    if (block == null || block.Movable == false)
                    {
                        continue;
                    }

                    Block blockRight = _blocks[Coords.Index(coords.x - 1, coords.y, _dimensions.x)];
                    if (blockRight == null)
                    {
                        SwipeBlock(block, direction);
                    }
                    else
                    {
                        TryMergeBlocks(block, blockRight);
                    }
                }
            }
        }
    }

    private void SwipeBlock(Block block, Direction direction)
    {
        Coords target = new Coords();
        if (direction == Direction.Down)
        {
            target.x = block.Coords.x;
            for (target.y = block.Coords.y; ; target.y--)
            {
                if (target.y == 0 || _blocks[Coords.Index(target.x, target.y - 1, _dimensions.x)] != null)
                {
                    break;
                }
            }
        }
        if (direction == Direction.Up)
        {
            target.x = block.Coords.x;
            for (target.y = block.Coords.y; ; target.y++)
            {
                if (target.y == _dimensions.y - 1 || _blocks[Coords.Index(target.x, target.y + 1, _dimensions.x)] != null)
                {
                    break;
                }
            }
        }
        if (direction == Direction.Right)
        {
            target.y = block.Coords.y;
            for (target.x = block.Coords.x; ; target.x++)
            {
                if (target.x == _dimensions.x - 1 || _blocks[Coords.Index(target.x + 1, target.y, _dimensions.x)] != null)
                {
                    break;
                }
            }
        }
        if (direction == Direction.Left)
        {
            target.y = block.Coords.y;
            for (target.x = block.Coords.x; ; target.x--)
            {
                if (target.x == 0 || _blocks[Coords.Index(target.x - 1, target.y, _dimensions.x)] != null)
                {
                    break;
                }
            }
        }

        MoveBlockTo(block, target);
    }

    private void MoveBlockTo(Block block, Coords target)
    {
        _blocks[block.Coords.Index(_dimensions.x)] = null;
        _blocks[target.Index(_dimensions.x)] = block;
        block.Coords = target;
        block.transform.position = GetCellPosition(target);
    }
    
    private void TryMergeBlocks(Block block, Block targetBlock)
    {
        if (block.Mergeable == false || targetBlock.Mergeable == false || block.PowerOfTwo != targetBlock.PowerOfTwo)
        {
            return;
        }

        _blocks[block.Coords.Index(_dimensions.x)] = null;
        _blocks[targetBlock.Coords.Index(_dimensions.x)] = null;
        
        Block mergedBlock = _levelController.BlocksPool.Spawn();
        mergedBlock.SetBlock(targetBlock.PowerOfTwo + 1, true, true);
        mergedBlock.Coords = targetBlock.Coords;
        mergedBlock.transform.position = targetBlock.transform.position;
        
        _blocks[mergedBlock.Coords.Index(_dimensions.x)] = mergedBlock;
        
        _levelController.BlocksPool.Despawn(block);
        _levelController.BlocksPool.Despawn(targetBlock);
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