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
    private TMP_Text _starCounter;

    [SerializeField]
    private LocalizedItemFormatable _levelCounter;

    [SerializeField]
    private RectTransform _player;

    [SerializeField]
    private RectTransform _target;

    [SerializeField]
    private Vector2 _playerScale;

    [SerializeField]
    private float _yOffset;

    [SerializeField]
    private CanvasGroup _uiGroup;

    [Inject]
    private IDialogsService _dialogs;

    [Inject]
    private IPlayerDataService _playerDataService;

    private bool _nextButtonClicked;

    public override void InitWithData(object data)
    {
        base.InitWithData(data);

        _nextButtonClicked = false;

        (int stars, int level, SpriteRenderer player) = ((int, int, SpriteRenderer)) data;

        _starCounter.text = $"{stars}/3";
        _levelCounter.FormatLocalizedText("", $" {level + 1}");
        
        PlayFinishAnimation(player);
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

    private void PlayFinishAnimation(SpriteRenderer renderer)
    {
        StartCoroutine(UIFade_co());

        Camera camera = Camera.main;
        
        _player.sizeDelta = Vector2.one * GetSpriteHeightInPixels(renderer, camera);
        _player.localRotation = Quaternion.identity;

        Vector2 viewportPosition = camera.WorldToViewportPoint(renderer.transform.position);
        
        Vector2 worldObject_ScreenPosition = new Vector2(
            ((viewportPosition.x * _dialogs.DialogsRootSize.x) - (_dialogs.DialogsRootSize.x * 0.5f)),
            ((viewportPosition.y * _dialogs.DialogsRootSize.y) - (_dialogs.DialogsRootSize.y * 0.5f)) + _yOffset);
 
        //now you can set the position of the ui element
        _player.anchoredPosition = worldObject_ScreenPosition;

        _player.DOAnchorPos(_target.anchoredPosition, 1.2f);
        _player.DOSizeDelta(_target.sizeDelta, 1.2f);
        _player.DORotate(_target.eulerAngles, 1.2f);
    }

    private IEnumerator UIFade_co()
    {
        _uiGroup.alpha = 0f;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            _uiGroup.alpha = alpha;
            yield return null;
        }

        _uiGroup.alpha = 1f;
    }

    private float GetSpriteHeightInPixels(SpriteRenderer renderer, Camera camera)
    {
        Vector2 scaleFactorRange = new Vector2(Screen.width / 946f, Screen.height / 2048f);

        float scaleFactor = Mathf.Lerp(scaleFactorRange.x, scaleFactorRange.y, 0.5f);

        float nativeWorldHeight = renderer.sprite.rect.height / renderer.sprite.pixelsPerUnit;
        float heightInWorldUnits = renderer.transform.localScale.y * nativeWorldHeight;
        float heightAsViewFraction = heightInWorldUnits / (camera.orthographicSize * 2f);
        float heightInScreenPixels = heightAsViewFraction * camera.pixelRect.height;
        
        return heightInScreenPixels / scaleFactor;
    }
}