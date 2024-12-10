using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using I2.Loc;

public class DialogMgr : MonoBehaviour
{
    public byte LanguageIndex = 0;
    public SpawnManagerScriptableObject TextInfos;
    Stack<int> DelayDialog=new Stack<int>();
    // Start is called before the first frame update
    void Start()
    {
        //SubtitleView.Instance.chartime = LanguageIndex == 0 ? 0.1f : 0.05f;
        if (PlayerPrefs.HasKey("data"))
            CheckDialog(48);
        else
        {
            CharacterM.Instance.RefreshData();
            CheckDialog(0);
        }
    }
    private static DialogMgr _instance;

    public static DialogMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = (DialogMgr)FindObjectOfType(typeof(DialogMgr));
            return _instance;
        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}

    //void ShowDialog(int index)
    //{

    //    if (index>0||index < TextInfos.spawnTexts.Length)
    //    {
    //        bool isselect = TextInfos.spawnTexts[index].isSelect;
    //        SubtitleView.Instance.ShowPanel();
    //        SubtitleView.Instance.ModalButton.gameObject.SetActive(true);
    //        SubtitleView.Instance.Selections.SetActive(false);
    //        SubtitleView.Instance.ShowText(LanguageIndex == 0 ? TextInfos.spawnTexts[index].dialog : TextInfos.spawnTexts[index].dialogEng,isselect?()=> {
    //            CheckSelection(index);
    //        }
    //        :()=> { CheckDialog(TextInfos.spawnTexts[index].next1); });
    //    }
    //}
    public void AddDelayDialog(int i)
    {
        DelayDialog.Push(i);
    }
    public void CheckDialog(int index,bool sound =true)
    {
        //Debug.LogError("CheckDialog");
        if (index >= TextInfos.spawnTexts.Length || index < 0)
        {            
            //Debug.LogError("HidePanel");
            SubtitleView.Instance.HidePanel();
        }
        else
        {
            bool isselect = TextInfos.spawnTexts[index].isSelect;
            SubtitleView.Instance.ShowPanel();
            SubtitleView.Instance.ModalButton.gameObject.SetActive(true);
            SubtitleView.Instance.Selections.SetActive(false);
            //if (TextInfos.spawnTexts[index].outcome >= 0)
                GetTalkReward(TextInfos.spawnTexts[index].outcome);
            SubtitleView.Instance.ShowText(LanguageIndex == 0 ? TextInfos.spawnTexts[index].dialog : TextInfos.spawnTexts[index].dialogEng, TextInfos.spawnTexts[index].character, isselect ? () => {
                CheckSelection(index);
            }
            :() => { //CheckDialog(TextInfos.spawnTexts[index].next1);
                CheckNext(index);         
            },sound); 
        }
    }
    void CheckNext(int index)
    {
        if (TextInfos.spawnTexts[index].nexts.Count < 1)
        {
            if (DelayDialog.Count > 0)
            {
                CheckDialog(DelayDialog.Pop());
            }
            else SubtitleView.Instance.HidePanel();
        }
        else if (TextInfos.spawnTexts[index].nexts.Count > 1)
        {
            CheckDialog(TextInfos.spawnTexts[index].nexts[UnityEngine.Random.Range(0, TextInfos.spawnTexts[index].nexts.Count)]);
        }
        else CheckDialog(TextInfos.spawnTexts[index].nexts[0]);
    }
    void GetTalkReward(int index)
    {
        if (index >= PropMgr.Instance.PropConfig.proplist.Length || index < 0)
            return;
        //Debug.LogError("GetTalkReward");
        Propmodule m = PropMgr.Instance.PropConfig.proplist[index];

        if (m.gold < 0)
        {
            SoundMgr.Instance.PlayAudio(3);
            CharacterM.Instance.BuyTimes++;
        }
        CharacterM.Instance.Health += m.health;
        CharacterM.Instance.Attack += m.attack;
        CharacterM.Instance.Defence += m.defence;
        CharacterM.Instance.Gold += m.gold;
        CharacterM.Instance.Key = new int[3] { CharacterM.Instance.Key[0] + m.key1, CharacterM.Instance.Key[1] + m.key2, CharacterM.Instance.Key[2] + m.key3 };
        if (m.func != SpecialFunc.None)
        {
            PropMgr.Instance.InvolkeUniqueFunction(m.func);
        }
    }
    bool CheckSelectable(int id)
    {
        int outcome = TextInfos.spawnTexts[id].outcome;
        if (outcome >= PropMgr.Instance.PropConfig.proplist.Length || outcome < 0)
            return true;
        Propmodule m = PropMgr.Instance.PropConfig.proplist[outcome];


        if (CharacterM.Instance.Gold + m.gold < 0)
            return false;
        return true;

    }
    void CheckSelection(int curindex, Action callback1=null, Action callback2 = null, Action callback3 = null)
    {
        //Debug.LogError("CheckSelection");
        
        if (!TextInfos.spawnTexts[curindex].isSelect)
            return;
        SubtitleView.Instance.ModalButton.gameObject.SetActive(false);
        SubtitleView.Instance.Selections.SetActive(true);
        List<int> choicelist = TextInfos.spawnTexts[curindex].nexts;
        foreach (var item in SubtitleView.Instance.ChoiceList)
        {
            item.onClick.RemoveAllListeners();
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < choicelist.Count; i++)
        {
            int choiceid = choicelist[i];
            SubtitleView.Instance.ChoiceList[i].gameObject.SetActive(true);
            //if (CharacterM.Instance.Gold < 20)
            SubtitleView.Instance.ChoiceList[i].interactable = CheckSelectable(choiceid);
            //SubtitleView.Instance.ChoiceList[i].gameObject.GetComponentInChildren<TMP_Text>().text = TextInfos.spawnTexts[choicelist[i]].dialog;  ∏ƒ∂‡”Ô—‘
            SubtitleView.Instance.ChoiceList[i].gameObject.GetComponentInChildren<Localize>().SetTerm(TextInfos.spawnTexts[choicelist[i]].dialog);
            SubtitleView.Instance.ChoiceList[i].onClick.AddListener(() => {
                callback1?.Invoke();
                //if (TextInfos.spawnTexts[n1].outcome >= 0)
                //Debug.LogError("OUTCOME="+i.ToString());
                GetTalkReward(TextInfos.spawnTexts[choiceid].outcome);
                //CheckDialog(TextInfos.spawnTexts[choicelist[i]].next1);
                CheckNext(choiceid);
            });
        }
        //int n1 = TextInfos.spawnTexts[curindex].next1;
        //int n2 = TextInfos.spawnTexts[curindex].next2;
        //int n3 = TextInfos.spawnTexts[curindex].next3;
        //if (n1 >= TextInfos.spawnTexts.Length || n1 < 0)
        //    SubtitleView.Instance.Choice1.gameObject.SetActive(false);
        //else
        //{
        //    SubtitleView.Instance.Choice1.gameObject.SetActive(true);
        //    SubtitleView.Instance.choice1Text.text = TextInfos.spawnTexts[n1].dialog;
        //    SubtitleView.Instance.Choice1.onClick.AddListener(() => { callback1?.Invoke();
        //        //if (TextInfos.spawnTexts[n1].outcome >= 0)
        //            GetTalkReward(TextInfos.spawnTexts[n1].outcome);
        //        CheckDialog(TextInfos.spawnTexts[n1].next1);
        //    });
        //}
        //if (n2 >= TextInfos.spawnTexts.Length || n2 < 0)
        //    SubtitleView.Instance.Choice2.gameObject.SetActive(false);
        //else
        //{
        //    SubtitleView.Instance.Choice2.gameObject.SetActive(true);
        //    SubtitleView.Instance.choice2Text.text = TextInfos.spawnTexts[n2].dialog;
        //    SubtitleView.Instance.Choice2.onClick.AddListener(() => { callback2?.Invoke();
        //        //if (TextInfos.spawnTexts[n2].outcome >= 0)
        //            GetTalkReward(TextInfos.spawnTexts[n2].outcome);
        //        CheckDialog(TextInfos.spawnTexts[n2].next1);
        //    });
        //}
        //if (n3 >= TextInfos.spawnTexts.Length || n3 < 0)
        //    SubtitleView.Instance.Choice3.gameObject.SetActive(false);
        //else
        //{
        //    SubtitleView.Instance.Choice3.gameObject.SetActive(true);
        //    SubtitleView.Instance.choice3Text.text = TextInfos.spawnTexts[n3].dialog;
        //    SubtitleView.Instance.Choice3.onClick.AddListener(() => { callback3?.Invoke();
        //        //if (TextInfos.spawnTexts[n3].outcome >= 0)
        //            GetTalkReward(TextInfos.spawnTexts[n3].outcome);
        //        CheckDialog(TextInfos.spawnTexts[n3].next1);
        //    });
        //}
    }
}
