using System.Collections.ObjectModel;
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
    private float _victoryDelay = .5f;
    
    [SerializeField]
    private CameraController _camera;
    
    [SerializeField]
    private Board _board;

    [Inject]
    private GameSettings _gameSettings;

    [Inject]
    private IPlayerDataService _playerDataService;

    [Inject]
    private IGameService _gameService;

    [Inject]
    private IDelayedCallService _delayedCallService;

    private int _movesCounter;
    private bool _playing;

    public ObjectPool<Block> BlocksPool { get; private set; }

    public void OnSwipeN() => OnSwipe(Direction.Up);
    public void OnSwipeE() => OnSwipe(Direction.Right);
    public void OnSwipeS() => OnSwipe(Direction.Down);
    public void OnSwipeW() => OnSwipe(Direction.Left);

    protected override void OnAppInitialized()
    {
        Subscribe(NotificationType.LoadNewLevel, OnLoadNewLevel);
        Subscribe(NotificationType.LoadSavedLevel, OnLoadSavedLevel);
        Subscribe(NotificationType.GenerateRandomLevel, (a, b) => GenerateRandomLevel());
        Subscribe(NotificationType.BlocksMerge, OnBlocksMerge);
        BlocksPool = new ObjectPool<Block>(_gameSettings.BlockPrefab, 36, transform);
    }

    private LevelData GetLevelData()
    {
        return new LevelData
        {
            BlocksData = _board.GetBlocksData(),
            MovesCount = _movesCounter,
            LevelIndex = _gameService.CurrentLevel.LevelIndex,
        };
    }

    private void OnBlocksMerge(NotificationType notificationType, NotificationParams notificationParams)
    {
        int mergedPOT = (int) notificationParams.Data;
        if (mergedPOT >= _gameService.CurrentLevel.TargetValue.POT)
        {
            _playing = false;
            Dispatch(NotificationType.PlayerReachedTargetNumber);
            _playerDataService.CompleteLevel(_gameService.CurrentLevel.LevelIndex, _gameService.CurrentLevel.CalculateStars(_movesCounter));
            _delayedCallService.DelayedCall(_victoryDelay, () => Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.VictoryDialog, ViewCreationOptions.None, _movesCounter)));
        }
    }

    private void OnLoadNewLevel(NotificationType notificationType, NotificationParams notificationParams)
    {
        int levelIndex = Mathf.Min((int) notificationParams.Data, _gameSettings.Levels.Count - 1);
        LevelConfig level = _gameSettings.Levels[levelIndex];
        level.LevelIndex = levelIndex;
        _playerDataService.LastLevel = levelIndex;
        
        LoadLevel(level);
    }
    
    private void OnLoadSavedLevel(NotificationType notificationType, NotificationParams notificationParams)
    {
        LevelData savedData = (LevelData) notificationParams.Data;
        LevelConfig level = _gameSettings.Levels[savedData.LevelIndex];
        level.LevelIndex = savedData.LevelIndex;
        _playerDataService.LastLevel = savedData.LevelIndex;
        
        LoadLevel(level, savedData);
    }

    private void LoadLevel(LevelConfig levelConfig, LevelData savedData = null)
    {
        _camera.SetupCamera(levelConfig);
        _board.SetupBoard(levelConfig, savedData);

        _movesCounter = savedData?.MovesCount ?? 0;

        _playing = true;
        Dispatch(NotificationType.LevelLoaded, NotificationParams.Get(levelConfig));
        Dispatch(NotificationType.MovesCounterChanged, NotificationParams.Get(_movesCounter));
    }

    private void GenerateRandomLevel()
    {
        LevelConfig levelConfig = LevelGenerator.GenerateLevel();
        
        _board.SetupBoard(levelConfig);
    }

    private void OnSwipe(Direction direction)
    {
        if (_playing == false)
        {
            return;
        }
        
        bool anyChanges = _board.Swipe(direction);
        if (anyChanges)
        {
            _movesCounter++;
            Dispatch(NotificationType.MovesCounterChanged, NotificationParams.Get(_movesCounter));
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused == false)
        {
            return;
        }

        Debug.Log("Saving level data . . .");
        _playerDataService.LevelData = GetLevelData();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Saving level data . . .");
        _playerDataService.LevelData = GetLevelData();
    }
}