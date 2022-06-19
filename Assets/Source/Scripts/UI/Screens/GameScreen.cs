using System.Linq;
using Aig.Client.Integration.Runtime.Subsystem;
using Core;
using Core.Attributes;
using Core.Notifications;
using Core.Services;
using Core.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : DIBehaviour
{
    [SerializeField]
    private TMP_Text _targetBlockNumber;

    [SerializeField]
    private Image _targetBlockImage;

    [SerializeField]
    private TMP_Text _levelNumber;

    [SerializeField]
    private TMP_Text _moves;

    [SerializeField]
    private GameObject[] _stars;

    [Inject]
    private GameSettings _gameSettings;

    [Inject]
    private IGameService _gameService;

    [Inject] 
    private ISoundService _soundService;

    protected override void OnAppInitialized()
    {
        IntegrationSubsystem.Instance.AdsService.ShowBanner();
        
        Subscribe(NotificationType.LevelLoaded, OnLevelLoaded);
        Subscribe(NotificationType.MovesCounterChanged, OnPlayerMove);

        foreach (var button in GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => _soundService.PlaySound(Sounds.Click));
        }
    }

    private void OnPlayerMove(NotificationType notificationType, NotificationParams notificationParams)
    {
        int moves = (int) notificationParams.Data;
        _moves.text = "MOVES: " + moves;
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].SetActive(moves < _gameService.CurrentLevel.StarMoves[i]);
        }
    }

    private void OnLevelLoaded(NotificationType notificationType, NotificationParams notificationParams)
    {
        LevelConfig loadedLevel = (LevelConfig) notificationParams.Data;
        _levelNumber.text = "LEVEL " + (loadedLevel.LevelIndex + 1);
        _targetBlockNumber.text = loadedLevel.TargetValue.Number.ToString();
        _targetBlockImage.color = _gameSettings.BlockColors.GetColor(loadedLevel.TargetValue);
        foreach (var star in _stars)
        {
            star.SetActive(true);
        }
    }

    public void OnPauseButtonClick()
    {
        Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.PauseDialog));
    }

    public void OnLevelsMenuButtonClick()
    {
        Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.LevelsMenu));
    }

    public void OnGenerateRandomLevel()
    {
        Dispatch(NotificationType.GenerateRandomLevel);
    }
}
