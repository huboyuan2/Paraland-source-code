using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.Text;
[SerializeField]
public class CharacterM : MonoBehaviour
{
    // Start is called before the first frame update
   
    //string name;
    int health;
    int attack;
    int defence;
    int gold;
    int buyTimes;
    int[] key= { 0,0,0};
    bool canDig = false;
    bool canrotate =false;
    bool keysold = false;
    bool fogUnlock = false;
    //[System.NonSerialized]
    private static CharacterM _instance;
    StringBuilder sb;
    private void Awake()
    {
        //OnValueChange += SaveData;
        sb = new StringBuilder();
    }
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            //if (value > health)
            //{
            //    EffectMgr.Instance.PlayEffect(1, transform);
            //}
            health = value;
            OnValueChange.Invoke();
        }
    }
    public int BuyTimes
    {
        get
        {
            return buyTimes;
        }
        set
        {
            
            buyTimes = value;
            if (buyTimes == 5)
            {
                //DialogMgr.Instance.CheckDialog(46);
                DialogMgr.Instance.AddDelayDialog(46);
            }
        }
    }
    public int Attack
    {
        get
        {
            return attack;
        }
        set
        {
            //if (value > attack)
            //{
            //    EffectMgr.Instance.PlayEffect(0, transform);
            //}
            attack = value;
            OnValueChange.Invoke();
        }
    }
    public int Defence
    {
        get
        {
            return defence;
        }
        set
        {
            //if (value > defence)
            //{
            //    EffectMgr.Instance.PlayEffect(0, transform);
            //}
            defence = value;
            OnValueChange.Invoke();
        }
    }
    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            OnValueChange.Invoke();
        }
    }
    //赶时间偷懒的写法，不要学习
    public string EncodeData()
    {
        sb.Clear();
        sb.Append(health).Append("#");
        sb.Append(attack).Append("#");
        sb.Append(defence).Append("#");
        sb.Append(gold).Append("#");
        sb.Append(buyTimes).Append("#");
        sb.Append(key[0]).Append("#");
        sb.Append(canDig).Append("#");
        sb.Append(canrotate).Append("#");
        sb.Append(keysold).Append("#");
        sb.Append(fogUnlock);
        return(sb.ToString());
    }
    public void DecodeData(string datastr)
    {
        var dataArray = datastr.Split("#");
        health = int.Parse(dataArray[0]);
        attack = int.Parse(dataArray[1]);
        defence = int.Parse(dataArray[2]);
        gold = int.Parse(dataArray[3]);
        buyTimes = int.Parse(dataArray[4]);
        key[0]= int.Parse(dataArray[5]);
        canDig = dataArray[6] == "True";
        canrotate = dataArray[7] == "True";
        keysold = dataArray[8] == "True";
        fogUnlock= dataArray[9] == "True";
        MovePanelView.Instance.LockRockBtn(canrotate);
        if (fogUnlock)
        {
            //Debug.LogError("UnlockFog");
            MoveFog.Instance.InitialHeight = -9f;
        }
        //为了更新UI
        Health = health;
    }
    public int[] Key
    {
        get
        {
            return key;
        }
        set
        {
            key = value;
            OnValueChange.Invoke();
        }
    }
    public static Action OnValueChange;
    public static CharacterM Instance
    {
        get
        {
            if (_instance == null)
                _instance = (CharacterM)FindObjectOfType(typeof(CharacterM));
            return _instance;
        }
    }

    public bool CanDig { 
        get => canDig;
        set
        {
            canDig = value;
            OnValueChange.Invoke();
        }
    }

    public bool Canrotate
    {
        get => canrotate;
        set
        {
            canrotate = value;
            MovePanelView.Instance.LockRockBtn(!canrotate);
            OnValueChange.Invoke();
        }
    }
    public bool FogUnlock
    {
        get => fogUnlock;
        set
        {
            fogUnlock = value;
            if (value)
            MoveFog.Instance.InitialHeight = -9f;
            OnValueChange.Invoke();
        }
    }

    private void Start()
    {
        //Health = 500;
        //Attack = 10;
        //Defence = 10;
        //Gold = 0;
        //Key[0] = 0;
    }

    public void RefreshData()
    {
        Health = 500;
        Attack = 10;
        Defence = 10;
        Gold = 0;
        Key[0] = 0;
        key[1] = 0;
        key[2] = 0;
        canDig = false;
        Canrotate = false;
        keysold = false;
        fogUnlock = false;
        //DialogMgr.Instance.CheckDialog(0);
    }
}
