using System.Collections;
using Core;
using Core.Attributes;
using Core.Notifications;
using Core.Services;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

public class VictoryDialog : BaseViewController
{
    [SerializeField]
    private TMP_Text _levelCounter;

    [Inject]
    private IPlayerDataService _playerDataService;

    [Inject]
    private IGameService _gameService;

    private bool _nextButtonClicked;

    public override void InitWithData(object data)
    {
        base.InitWithData(data);
        
        _nextButtonClicked = false;

        int moves = (int) data;
        int level = _gameService.CurrentLevel.LevelIndex;

        _levelCounter.text = (level + 1).ToString();
    }

    public override void OnShroudClicked() { }

    public void OnNextButtonClick()
    {
        if (_nextButtonClicked)
        {
            return;
        }

        _nextButtonClicked = true;
        CloseDialog();
        Dispatch(NotificationType.LoadNextLevel);
        if (_playerDataService.CanShowRateUs())
        {
            Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.RateUsDialog));
        }
    }
}