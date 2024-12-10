using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using I2.Loc;
using UnityEditor.Rendering;

public class SubtitleView : MonoBehaviour
{
    public TMP_Text dialogtext;
    StringBuilder sb = new StringBuilder();
    public float chartime=0.05f;
    //public AudioSource typesound;
    private static SubtitleView _instance;
    public GameObject Selections;
    public Button ModalButton;
    public List<Button> ChoiceList;
    //public Button Choice1;
    //public Button Choice2;
    //public Button Choice3;
    public TMP_Text Title;
    //public TMP_Text choice1Text;
    //public TMP_Text choice2Text;
    //public TMP_Text choice3Text;
    DialogStates curstate;
    enum DialogStates
    { 
    Playing,
    WaitingNext,
    WaitingClose,
    WaitingChoice,
    Closed
    }
    public static SubtitleView Instance
    {
        get
        {
            if (_instance == null)
                _instance = (SubtitleView)FindObjectOfType(typeof(SubtitleView),true);
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        ModalButton.onClick.AddListener(OnClick);
        
        //ShowPanel();
        //StartCoroutine(ShowTextAsyn("暴雪回归，近20万人预约《魔兽世界》！玩家：账号是否能找回？"));
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    void OnClick()
    {
        if (curstate == DialogStates.Playing)
        {
            curstate = DialogStates.WaitingNext;
        }
        else if (curstate == DialogStates.WaitingNext)
        {
            curstate = DialogStates.Playing;
        }
        else if (curstate == DialogStates.WaitingClose)
        {
            HidePanel();
        }
    }
    public void ShowText(string text,string title,Action callback=null,bool sound=true)
    {
       // Debug.LogError(text);
        text = I2.Loc.LocalizationManager.GetTranslation(text); //拿到多语言版本文字再进入打字机，不用SetTerm方法以免出问题
        StartCoroutine(ShowTextAsyn(text,title,callback,sound));
    }
    IEnumerator ShowTextAsyn(string text,string title, Action callback = null,bool sound=true)
    {
        if (!string.IsNullOrEmpty(title))
        {
            //Title.text = title;
            Title.GetComponent<Localize>().SetTerm(title);
        }
        else Title.text = string.Empty;
        bool skiprichtest = false;
        curstate = DialogStates.Playing;
        sb.Clear();
        int i = 0;
        while (i < text.Length)
        {
            if (text[i] == '<')
                skiprichtest = true;
            if (text[i] == '>')
                skiprichtest = true;
            sb.Append(text[i]);
            i++;
            dialogtext.text = sb.ToString();  //改多语言
            //dialogtext.GetComponent<Localize>().SetTerm(sb.ToString());
            //if (typesound != null)
            // typesound.Play();
            if (sound)
                if (localizationManager.instance.lang == localizationManager.Lang.English)
                {
                    if (text[i-1] == ' ')
                        SoundMgr.Instance.PlayDialogSound();
                }
                else
                    SoundMgr.Instance.PlayDialogSound();
            if (curstate == DialogStates.Playing)
            {
                if(!skiprichtest)
                yield return new WaitForSeconds(chartime);
            }
               
        }        
        curstate = DialogStates.WaitingNext;
        while (curstate == DialogStates.WaitingNext)
            yield return null;
        callback.Invoke();
    }
    public void ShowPanel()
    {
        this.gameObject.SetActive(true);
        MovePanelView.Instance.gameObject.SetActive(false);
    }
    public void HidePanel()
    {
        this.gameObject.SetActive(false);
        MovePanelView.Instance.gameObject.SetActive(true);
    }
}
