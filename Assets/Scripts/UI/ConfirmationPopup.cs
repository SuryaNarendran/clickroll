using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmationPopup : Singleton<ConfirmationPopup>
{
    private System.Action confirmCallback;
    private System.Action cancelCallback;

    [SerializeField] Transform confirmationPopupOverlay;
    [SerializeField] TMP_Text promptTextLabel;

    public static void Raise(string prompt, System.Action confirmCallback, System.Action cancelCallback = null)
    {
        Instance.confirmationPopupOverlay.gameObject.SetActive(true);
        Instance.promptTextLabel.text = prompt;
        Instance.confirmCallback = confirmCallback;
        Instance.cancelCallback = cancelCallback;
    }

    public void OnConfirm()
    {
        confirmationPopupOverlay.gameObject.SetActive(false);
        confirmCallback?.Invoke();
    }

    public void OnCancel()
    {
        confirmationPopupOverlay.gameObject.SetActive(false);
        cancelCallback?.Invoke();
    }
}
