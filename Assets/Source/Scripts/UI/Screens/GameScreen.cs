using System.Collections;
using System.Collections.Generic;
using Aig.Client.Integration.Runtime.Subsystem;
using Core;
using Core.Attributes;
using Core.Notifications;
using Core.Services;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class GameScreen : DIBehaviour
{
    [Inject]
    private IPlayerDataService _playerDataService;

    [Inject] 
    private ISoundService _soundService;
    

    protected override void OnAppInitialized()
    {
        IntegrationSubsystem.Instance.AdsService.ShowBanner();
        
        Subscribe(NotificationType.LevelLoaded, OnLevelLoaded);

        foreach (var button in GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => _soundService.PlaySound(Sounds.Click));
        }
    }

    private void OnLevelLoaded(NotificationType notificationType, NotificationParams notificationParams)
    {
        int level = notificationParams == null ? _playerDataService.LastLevel : (int) notificationParams.Data;
    }

    public void OnPauseButtonClick()
    {
        Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.PauseDialog));
    }

    public void OnLevelsMenuButtonClick()
    {
        Dispatch(NotificationType.ShowView, ShowViewNotificationParams.Get(ViewName.LevelsMenu));
    }
}
