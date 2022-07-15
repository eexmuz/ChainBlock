using System.Collections.Generic;
using Core;
using Core.Settings;
using TMPro;
using UnityEngine;

public class ConstructorBlock : DIBehaviour
{
    [SerializeField]
    private TMP_Text _number;

    [SerializeField]
    private GameObject _valueBlock;

    [SerializeField]
    private GameObject _lock;

    [SerializeField]
    private GameObject _barrier;

    [HideInInspector]
    public BlockData BlockData;

    private Dictionary<Direction, Vector3> _vectorDirection = new Dictionary<Direction, Vector3>
    {
        {Direction.Down, Vector3.down},
        {Direction.Left, Vector3.left},
        {Direction.Right, Vector3.right},
        {Direction.Up, Vector3.up},
    };

    public void SetBlock(GameSettings gameSettings, BoardCellData cellData)
    {
        BlockData = new BlockData
        {
            PowerOfTwo = cellData.BlockInfo.PowerOfTwo,
            Movable = cellData.BlockInfo.Movable,
            Mergeable = cellData.BlockInfo.Mergeable,
            Coords = cellData.Coords,
        };

        _barrier.SetActive(BlockData.Movable == false && BlockData.Mergeable == false);
        _valueBlock.SetActive(_barrier.activeSelf == false);
        _lock.SetActive(BlockData.Movable == false && BlockData.Mergeable == true);
        _number.gameObject.SetActive(BlockData.Mergeable == true);

        if (_valueBlock.activeSelf)
        {
            _number.color = gameSettings.BlockColors.GetColor(BlockData.PowerOfTwo);
        }

        _number.text = (1 << BlockData.PowerOfTwo).ToString();
        
        UpdateName();
    }

    public void Move(Direction direction)
    {
        Coords oldCoords = BlockData.Coords;
        
        BlockData.Coords.y += direction == Direction.Down ? -1 : 0;
        BlockData.Coords.y += direction == Direction.Up ? 1 : 0;
        BlockData.Coords.x += direction == Direction.Right ? 1 : 0;
        BlockData.Coords.x += direction == Direction.Left ? -1 : 0;

        BlockData.Coords.x = Mathf.Max(BlockData.Coords.x, 0);
        BlockData.Coords.y = Mathf.Max(BlockData.Coords.y, 0);

        if (oldCoords.Equals(BlockData.Coords) == false)
        {
            transform.localPosition += _vectorDirection[direction];
        }
        
        UpdateName();
    }

    public void SetNumber(int pot)
    {
        BlockData.PowerOfTwo = pot;
        _number.text = (1 << pot).ToString();
        _number.color = FindObjectOfType<LevelConstructor>().GameSettings.BlockColors.GetColor(BlockData.PowerOfTwo);
        _number.ForceMeshUpdate();
        UpdateName();
    }

    public void SetMovable(bool movable)
    {
        BlockData.Movable = movable;
        _barrier.SetActive(BlockData.Movable == false && BlockData.Mergeable == false);
        _lock.SetActive(BlockData.Movable == false && BlockData.Mergeable == true);
        UpdateName();
    }

    public void SetMergeable(bool mergeable)
    {
        BlockData.Mergeable = mergeable;
        _barrier.SetActive(BlockData.Movable == false && BlockData.Mergeable == false);
        _lock.SetActive(BlockData.Movable == false && BlockData.Mergeable == true);
        _number.gameObject.SetActive(mergeable);
        UpdateName();
    }

    private void UpdateName()
    {
        if (BlockData.Mergeable == false)
        {
            name = $"{BlockData.Coords.ToString()} BARRIER";
            return;
        }

        name = $"{BlockData.Coords.ToString()} Block {(1 << BlockData.PowerOfTwo).ToString()}";
        if (BlockData.Movable == false)
        {
            name += " FROZEN";
        }
    }
}
