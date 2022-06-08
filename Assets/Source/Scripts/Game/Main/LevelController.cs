using Core;
using Core.Attributes;
using Core.Settings;
using UnityEngine;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public class LevelController : DIBehaviour
{
    [SerializeField]
    private Board _board;

    [Inject]
    private GameSettings _gameSettings;

    public ObjectPool<Block> BlocksPool { get; private set; }

    public void OnSwipeN() => OnSwipe(Direction.Up);
    public void OnSwipeE() => OnSwipe(Direction.Right);
    public void OnSwipeS() => OnSwipe(Direction.Down);
    public void OnSwipeW() => OnSwipe(Direction.Left);

    protected override void OnAppInitialized()
    {
        BlocksPool = new ObjectPool<Block>(_gameSettings.BlockPrefab, 36, transform);
        _board.SetupBoard(_gameSettings.Levels[0]);
    }

    private void OnSwipe(Direction direction)
    {
        _board.Swipe(direction);
    }
}