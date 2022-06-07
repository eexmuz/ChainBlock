using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : Toggle
{
    private ToggleSprite _toggleSprite;
    
    public void SetIsOnWithoutNotify(bool value)
    {
        base.SetIsOnWithoutNotify(value);
        _toggleSprite ??= GetComponent<ToggleSprite>();
        _toggleSprite.OnValueChanged(value);
    }
}