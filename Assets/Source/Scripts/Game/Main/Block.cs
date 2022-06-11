using System;
using Core;
using Core.Attributes;
using Core.Settings;
using TMPro;
using UnityEngine;

public class Block : DIBehaviour
{
    [SerializeField]
    private TMP_Text _number;

    [SerializeField]
    private GameObject _valueBlock;

    [SerializeField]
    private GameObject _lock;

    [SerializeField]
    private GameObject _barrier;

    [Inject]
    private GameSettings _gameSettings;

    public int PowerOfTwo { get; private set; }
    public bool Movable { get; private set; }
    public bool Mergeable { get; private set; }
    
    public Coords Coords { get; set; }
    public bool JustMerged { get; set; }
    public Coords TargetCoords { get; set; }
    
    public void SetBlock(int powerOfTwo, bool movable, bool mergeable)
    {
        PowerOfTwo = powerOfTwo;
        Movable = movable;
        Mergeable = mergeable;
        
        _barrier.SetActive(movable == false && mergeable == false);
        _valueBlock.SetActive(_barrier.activeSelf == false);
        _lock.SetActive(movable == false && mergeable == true);
        _number.gameObject.SetActive(mergeable == true);

        if (_valueBlock.activeSelf)
        {
            _valueBlock.GetComponent<MeshRenderer>().material.color =
                _gameSettings.BlockColors.GetColor(powerOfTwo);
        }

        _number.text = (1 << powerOfTwo).ToString();
    }

    public void SetBlock(BlockInfo blockInfo)
    {
        SetBlock(blockInfo.PowerOfTwo, blockInfo.Movable, blockInfo.Mergeable);
    }
}