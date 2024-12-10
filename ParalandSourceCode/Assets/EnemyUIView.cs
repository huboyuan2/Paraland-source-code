using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using I2.Loc;
public class EnemyUIView : MonoBehaviour
{
    bool canwin;
    // Start is called before the first frame update
    public TMP_Text[] textgroup = new TMP_Text[7];
    public Button challengeButton;
    public TMP_Text Challangetxt;
    public Button CancleButton;
    Transform curTarget;
    public Vector3 Offset;
    private RectTransform _rect;

    private static EnemyUIView _instance;
    public static EnemyUIView Instance
    {
        get
        {
            if (_instance == null)
                _instance = (EnemyUIView)FindObjectOfType(typeof(EnemyUIView),true);
            return _instance;
        }
    }
    public Transform CurTarget
    {
        get
        {
            return curTarget;
        }
        set
        {
            _rect = transform as RectTransform;
            curTarget = value;
            FollowPosition(curTarget);
        }
    }
    public void FollowPosition(Transform target)
    {
        var screenPos = Camera.main.WorldToViewportPoint(target.position + Vector3.up * 0.3f)+Offset;
        //if(screenPos.z < 0)
        //    {
        //    screenPos = -Vector3.one;
        //}
        _rect.anchorMin = _rect.anchorMax = screenPos;
        _rect.anchoredPosition = Vector2.zero;
        //_rect = transform as RectTransform;
    }
    private void Awake()
    {
        challengeButton.onClick.AddListener(BattleMgr.Instance.StartBattle);
        CancleButton.onClick.AddListener(CloseSelf);
        CharacterPathFind.PlayerMove += OnMove;
        _rect = transform as RectTransform;
    }
    private void OnMove(int i,bool isnav)
    {
        CloseSelf();
    }
    private void OnEnable()
    {
        //Ref RefreshUI
    }
    public void CloseSelf()
    {
        BattleMgr.Instance.currentDmg = 0;
        BattleMgr.Instance.currentReward = 0;
        BattleMgr.Instance.currentEnm = null;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        FollowPosition(curTarget);
    }
    public void RefreshUI(module inputdata)
    {

        if (localizationManager.instance.lang == localizationManager.Lang.简体中文)
        {
            textgroup[0].text = "名字 " + inputdata.name.ToString();  //改多语言
                                                                    //textgroup[0].transform.GetComponent<Localize>().SetTerm(inputdata.name.ToString());
            textgroup[1].text = inputdata.health.ToString();
            textgroup[2].text = inputdata.gold.ToString();
            BattleMgr.Instance.currentReward = inputdata.gold;

            textgroup[3].text = inputdata.attack.ToString();
            textgroup[4].text = inputdata.defence.ToString();

            textgroup[7].text = inputdata.skill switch
            {
                EnemySkill.None => "特性 无",
                EnemySkill.Double => "<color=red>特性 二连击</color>",
                EnemySkill.Magic => "<color=blue>特性 魔攻</color>",
                EnemySkill.Early => "<color=green>特性 先攻</color>",
                _ => ""
            };
            int damage = BattleMgr.Instance.ApproxDamage(inputdata.health, inputdata.attack, inputdata.defence, out int turns, inputdata.skill);
            if (damage == -1 || damage >= CharacterM.Instance.Health)
            {
                canwin = false;
                if (damage == -1)
                { textgroup[5].text = "<color=red>无法战胜对手</color>"; }
                if (damage >= CharacterM.Instance.Health)
                { textgroup[5].text = "<color=red>总伤害 " + damage.ToString() + "</color>"; }
                challengeButton.interactable = false;
                Challangetxt.text = "无法挑战";
            }
            else
            {
                textgroup[5].text = "总伤害 " + damage.ToString();
                challengeButton.interactable = true;
                Challangetxt.text = "挑战";
            }
            if (turns == -1)
            {
                textgroup[6].text = "";
            }
            else textgroup[6].text = "回合数" + turns.ToString();

            if (BattleMgr.Instance.currentEnm == null)
            {
                challengeButton.interactable = false;
                Challangetxt.text = "距离太远";
            }
            curTarget.GetComponent<Enemy>().diedialog = inputdata.diedialog;
        }
        if (localizationManager.instance.lang == localizationManager.Lang.English)
        {
            //textgroup[0].text = "name " + inputdata.name.ToString();  //改多语言
            textgroup[0].transform.GetComponent<Localize>().SetTerm(inputdata.name.ToString());
            textgroup[1].text = inputdata.health.ToString();
            textgroup[2].text = inputdata.gold.ToString();
            BattleMgr.Instance.currentReward = inputdata.gold;

            textgroup[3].text = inputdata.attack.ToString();
            textgroup[4].text = inputdata.defence.ToString();

            textgroup[7].text = inputdata.skill switch
            {
                EnemySkill.None => "Traits: None",
                EnemySkill.Double => "<color=red>Traits: Double hit</color>",
                EnemySkill.Magic => "<color=blue>Trait: Magic Attack</color>",
                EnemySkill.Early => "<color=green>Trait: First strike</color>",
                _ => ""
            };
            int damage = BattleMgr.Instance.ApproxDamage(inputdata.health, inputdata.attack, inputdata.defence, out int turns, inputdata.skill);
            if (damage == -1 || damage >= CharacterM.Instance.Health)
            {
                canwin = false;
                if (damage == -1)
                { textgroup[5].text = "<color=red>Need levelup</color>"; }
                if (damage >= CharacterM.Instance.Health)
                { textgroup[5].text = "<color=red>Total Damage " + damage.ToString() + "</color>"; }
                challengeButton.interactable = false;
                Challangetxt.text = "Can't defeat";
            }
            else
            {
                textgroup[5].text = "Total Damage " + damage.ToString();
                challengeButton.interactable = true;
                Challangetxt.text = "Challenge";
            }
            if (turns == -1)
            {
                textgroup[6].text = "";
            }
            else textgroup[6].text = "Rounds" + turns.ToString();

            if (BattleMgr.Instance.currentEnm == null)
            {
                challengeButton.interactable = false;
                Challangetxt.text = "Too far";
            }
            curTarget.GetComponent<Enemy>().diedialog = inputdata.diedialog;


        }
    }
    
}
