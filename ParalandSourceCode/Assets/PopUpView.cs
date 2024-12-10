using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

public class PopUpView : MonoBehaviour
{
    private static PopUpView _instance;
    public TMP_Text TitleText;
    public TMP_Text ConfirmText;
    public TMP_Text CancelText;
    public Button ConfirmBtn;
    public Button CancelBtn;
    public static PopUpView Instance
    {
        get
        {
            if (_instance == null)
                _instance = (PopUpView)FindObjectOfType(typeof(PopUpView),true);
            return _instance;
        }
    }
    public void ShowPopUpWindow(string title, string confirm, string cancel, bool canconfirm = true,bool cancancel=true, Action confirmact=null,Action cancelact=null)
    {
        this.gameObject.SetActive(true);
        TitleText.GetComponent<Localize>().SetTerm(title);
        ConfirmText.GetComponent<Localize>().SetTerm(confirm);
        CancelText.GetComponent<Localize>().SetTerm(cancel);
        ConfirmBtn.interactable = canconfirm;
        CancelBtn.interactable = cancancel;
        ConfirmBtn.onClick.AddListener(() => { confirmact?.Invoke();
            ClosePanel();
        });
        CancelBtn.onClick.AddListener(() => { cancelact?.Invoke();
            ClosePanel();
        });
    }
    void ClosePanel()
    {
        ConfirmBtn.onClick.RemoveAllListeners();
        CancelBtn.onClick.RemoveAllListeners();
        this.gameObject.SetActive(false);
    }
}
