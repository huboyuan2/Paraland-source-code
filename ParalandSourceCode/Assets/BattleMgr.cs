using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public ChaMScriptObject enemyDataObj;
    private static BattleMgr _instance;
    public GameObject EnemyUI;
    public GameObject currentEnm;
    public GameObject currentDoor;
    public int currentDmg;
    public int currentReward;
    public static BattleMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = (BattleMgr)FindObjectOfType(typeof(BattleMgr));
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public int ApproxDamage(int h, int a, int d, out int turns,EnemySkill skill=EnemySkill.None)
    {
        turns = 0;
        int damege = 0;
        if (CharacterM.Instance.Attack <= d)
        {
            turns = -1;
            return -1;
        }
        if (skill == EnemySkill.Early)
        {
            damege += a - CharacterM.Instance.Defence;
        }
        while (h > 0)
        {
            h -= (CharacterM.Instance.Attack - d);
            if (h > 0)
            {
                if (skill == EnemySkill.Magic)
                {
                    damege += a;
                    turns++;
                }
                else
                {
                    if (a > CharacterM.Instance.Defence)
                    { damege += (a - CharacterM.Instance.Defence)*(skill==EnemySkill.Double?2:1); }
                    turns++;
                }
            }
        }
        currentDmg = damege;
        return damege;
    }
    public void StartBattle()
    {
        SoundMgr.Instance.PlayAudio(1);
        CharacterM.Instance.Health -= currentDmg;
        CharacterM.Instance.Gold += currentReward;
        CharacterAnim.Instance.SetBattle();
        currentEnm.GetComponent<Enemy>().DestroySelf();
        EnemyUIView.Instance.CloseSelf();
    }
    public void DealWithNPC(int id)
    {
        switch (id)
        {
            case 0:
                DialogMgr.Instance.CheckDialog(15);
                break;
            case 1:
                if (CharacterM.Instance.CanDig)
                {
                    PopUpView.Instance.ShowPopUpWindow("使用 挖掘", "挖掘", "取消", true, true, () => {
                        DialogMgr.Instance.CheckDialog(36);
                        CharacterM.Instance.CanDig = false;
                    });
                }
                else {
                    DialogMgr.Instance.CheckDialog(28);
                }
                break;
            case 2:
                {
                    PopUpView.Instance.ShowPopUpWindow("传送到教学关卡？", "确认", "取消", true, true, () => {
                       PropMgr.Instance.LoadScene(1, () => { 
                           CharacterM.Instance.RefreshData();
                           CharacterAnim.Instance.InitPos();
                           CamFollow.Instance.FindTarget();
                           SaveMgr.Instance.ChangeSceneData();
                       });
                    });
                }
                break;
            case 3:
                {
                    PopUpView.Instance.ShowPopUpWindow("传送到主关卡？", "确认", "取消", true, true, () => {
                        PropMgr.Instance.LoadScene(0, () => {
                            CharacterM.Instance.RefreshData();
                            CharacterAnim.Instance.InitPos();
                            CamFollow.Instance.FindTarget();
                            SaveMgr.Instance.ChangeSceneData();
                        });
                    });
                }
                break;
            default:
                break;
        }
    }
    public void GetEnemyInfoFromConfig(int index)
    {
        if (index < 0 || index >= enemyDataObj.tests.Length)
            return;
        module enmmodule= enemyDataObj.tests[index];
        EnemyUI.SetActive(true);
        EnemyUIView.Instance.RefreshUI(enmmodule);
    }
    public void OpenDoorUI(int doorid)
    {
        //54 87 41
        int[] spritearray = new int[]{54,87,41 };
        bool canopendoor = CharacterM.Instance.Key[doorid] > 0;
        PopUpView.Instance.ShowPopUpWindow(string.Format("开锁消耗{0}",spritearray[doorid]),"开锁","取消",canopendoor,true,()=> {
            CharacterM.Instance.Key[doorid]--;
            //由于CharacterM将整个数组封装为属性导致单独改动数组中的一项导致无法触发更新UI
            CharacterM.Instance.Key = CharacterM.Instance.Key;
            SoundMgr.Instance.PlayAudio(0);
            currentDoor.SetActive(false);
            SaveMgr.Instance.RecordDeadSceneObj(currentDoor);
        });

    }
    //在地图序列化完成后须在每次消灭怪物或开门后更新地图序列化数据
    void RefreshMapConfig()
    { 
    
    }
}
