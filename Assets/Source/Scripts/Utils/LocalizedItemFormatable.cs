using System.Collections;
using Core;
using Core.Attributes;
using TMPro;

public class LocalizedItemFormatable : DIBehaviour
{
    public string StringId = string.Empty;

    [Inject] 
    private ILocaleService _localeService;
    
    private string GetLocalizedString()
    {
        if (string.IsNullOrEmpty(StringId))
        {
            return StringId;
        }
        
        string localizedString = _localeService.GetString(StringId);
        return string.IsNullOrEmpty(localizedString) == false ? localizedString : StringId;
    }

    public void FormatLocalizedText(string prefix, string postfix)
    {
        if (_localeService == null)
        {
            StartCoroutine(FormatLocalizedText_co(prefix, postfix));
            return;
        }
        
        FormatLocalizedTextInternal(prefix, postfix);
    }

    private void FormatLocalizedTextInternal(string prefix, string postfix)
    {
        var label = GetComponent<TextMeshProUGUI>();
        label.text = $"{prefix}{GetLocalizedString()}{postfix}";
    }

    private IEnumerator FormatLocalizedText_co(string prefix, string postfix)
    {
        yield return WaitForStarted();
        FormatLocalizedTextInternal(prefix, postfix);
    }
}