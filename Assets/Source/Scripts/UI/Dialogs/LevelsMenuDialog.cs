using System.Collections.Generic;
using Core;
using Core.Attributes;
using Core.Services;
using Core.Settings;
using Lean.Touch;
using UI;
using UnityEngine;

public class LevelsMenuDialog : BaseViewController
{
    [SerializeField]
    private LevelsMenuItem _menuItemPrefab;

    [SerializeField]
    private Transform _menuItemsParent;

    [Inject]
    private IPlayerDataService _playerDataService;

    [Inject]
    private ISoundService _soundService;

    private List<LevelsMenuItem> _items;

    public override void InitWithData(object data)
    {
        base.InitWithData(data);

        var levels = _playerDataService.LevelStatusList;
        for (int i = 0; i < levels.Count; i++)
        {
            LevelsMenuItem item = Instantiate(_menuItemPrefab, _menuItemsParent);
            item.Init(i, levels[i]);
        }
        
        Subscribe(NotificationType.OpenLevelFromMenu, OnOpenLevelFromMenu);
    }

    public void OnBackButtonClick()
    {
        CloseDialog();
    }

    private void OnOpenLevelFromMenu(NotificationType notificationType, NotificationParams notificationParams)
    {
        _soundService.PlaySound(Sounds.Click);
        CloseDialog();
    }
}