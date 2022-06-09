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
        Subscribe(NotificationType.LoadLevel, OnLoadLevel);
        Subscribe(NotificationType.GenerateRandomLevel, (a, b) => GenerateRandomLevel());
        BlocksPool = new ObjectPool<Block>(_gameSettings.BlockPrefab, 36, transform);
    }

    private void OnLoadLevel(NotificationType notificationType, NotificationParams notificationParams)
    {
        int levelIndex = (int) notificationParams.Data;
        LevelData level = _gameSettings.Levels[levelIndex];
        _board.SetupBoard(level);
        Dispatch(NotificationType.LevelLoaded);
    }

    private void GenerateRandomLevel()
    {
        int targetPOT = 9;
    }

    private void OnSwipe(Direction direction)
    {
        _board.Swipe(direction);
    }

    public void LoadLevel(int level)
    {
        _board.SetupBoard(_gameSettings.Levels[level]);
    }
}