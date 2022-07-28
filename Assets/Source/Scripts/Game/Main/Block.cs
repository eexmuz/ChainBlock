using System;
using Core;
using Core.Attributes;
using Core.Settings;
using DG.Tweening;
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

    private bool _mergeAnimationQueued;

    private void OnEnable()
    {
        if (_mergeAnimationQueued)
        {
            MergeAnimation();
            _mergeAnimationQueued = false;
        }
    }

    public void SetBlock(int powerOfTwo, bool movable, bool mergeable)
    {
        PowerOfTwo = powerOfTwo;
        Movable = movable && mergeable;
        Mergeable = mergeable;

        _barrier.SetActive(mergeable == false);
        _valueBlock.SetActive(_barrier.activeSelf == false);
        _lock.SetActive(mergeable && movable == false);
        _number.gameObject.SetActive(mergeable);

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

    public BoardCellData GetBlockData()
    {
        return new BoardCellData
        {
            Coords = Coords,
            BlockInfo = new BlockInfo
            {
                Mergeable = Mergeable,
                Movable = Movable,
                PowerOfTwo = PowerOfTwo,
            }
        };
    }
    
    public void ShakeAnimation()
    {
        transform.DOPunchRotation(Vector3.forward * 1.5f, .3f);
    }

    public void PlayMergeAnimation()
    {
        if (gameObject.activeSelf == false)
        {
            _mergeAnimationQueued = true;
        }
        else
        {
            MergeAnimation();
        }
    }

    private void MergeAnimation()
    {
        transform.DOPunchScale(Vector3.one * .15f, .8f);
    }
}