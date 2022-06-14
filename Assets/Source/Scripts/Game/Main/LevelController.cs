using Core;
using Core.Attributes;
using Core.Notifications;
using Core.Services;
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

    [Inject]
    private IPlayerDataService _playerDataService;

    [Inject]
    private IGameService _gameService;

    private int _movesCounter;

    public ObjectPool<Block> BlocksPool { get; private set; }

    public void OnSwipeN() => OnSwipe(Direction.Up);
    public void OnSwipeE() => OnSwipe(Direction.Right);
    public void OnSwipeS() => OnSwipe(Direction.Down);
    public void OnSwipeW() => OnSwipe(Direction.Left);

    protected override void OnAppInitialized()
    {
        Subscribe(NotificationType.LoadLevel, OnLoadLevel);
        Subscribe(NotificationType.GenerateRandomLevel, (a, b) => GenerateRandomLevel());
        Subscribe(NotificationType.BlocksMerge, OnBlocksMerge);
        BlocksPool = new ObjectPool<Block>(_gameSettings.BlockPrefab, 36, transform);
    }

    private void OnBlocksMerge(NotificationType notificationType, NotificationParams notificationParams)
    {
        int mergedPOT = (int) notificationParams.Data;
        if (mergedPOT >= _gameService.CurrentLevel.TargetValue.POT)
        {
            Dispatch(NotificationType.PlayerReachedTargetNumber);
            _playerDataService.CompleteLevel(_gameService.CurrentLevel.LevelIndex, _gameService.CurrentLevel.CalculateStars(_movesCounter));
            Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.VictoryDialog, ViewCreationOptions.None, _movesCounter));
        }
    }

    private void OnLoadLevel(NotificationType notificationType, NotificationParams notificationParams)
    {
        int levelIndex = Mathf.Min((int) notificationParams.Data, _gameSettings.Levels.Count - 1);
        LevelData level = _gameSettings.Levels[levelIndex];
        level.LevelIndex = levelIndex;
        _playerDataService.LastLevel = levelIndex;
        
        _board.SetupBoard(level);

        _movesCounter = 0;
        
        Dispatch(NotificationType.LevelLoaded, NotificationParams.Get(level));
    }

    private void GenerateRandomLevel()
    {
        LevelData levelData = LevelGenerator.GenerateLevel();
        _board.SetupBoard(levelData);
    }

    private void OnSwipe(Direction direction)
    {
        bool anyChanges = _board.Swipe(direction);
        if (anyChanges)
        {
            _movesCounter++;
            Dispatch(NotificationType.OnPlayerMove, NotificationParams.Get(_movesCounter));
        }
    }

    public void LoadLevel(int level)
    {
        _board.SetupBoard(_gameSettings.Levels[level]);
    }
}