using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class PersistentSceneData : MonoBehaviour
{
    //感觉这个类不太应该用单例，应该直接从场景里面找
    public List<GameObject> proplist;
    public List<GameObject> enemylist;
    public List<bool> propdeath;
    public List<bool> enemydeath;
    public List<GameObject> doorlist;
    public List<bool> doordeath;
    StringBuilder sb;
    private void Awake()
    {
        sb = new StringBuilder();
    }
    public string SaveSceneData()
    {
        string result = "";
        result = string.Format("{0}#{1}#{2}", EncodeBoolList(propdeath), EncodeBoolList(enemydeath), EncodeBoolList(doordeath));
        return result;
    }
    public void LoadSceneData(string scenedatastr)
    {
        if (string.IsNullOrEmpty(scenedatastr))
            return;
        var dataArray = scenedatastr.Split("#");
        propdeath= DecodeBoolList(dataArray[0]);
        enemydeath = DecodeBoolList(dataArray[1]);
        doordeath = DecodeBoolList(dataArray[2]);
        RefreshPropList();
        RefreshEnemyList();
        RefreshDoorList();
    }
    public string EncodeBoolList(List<bool> boollist)
    {
        sb.Clear();
        foreach (var item in boollist)
        {
            sb.Append(item?1:0);
        }
        return sb.ToString();
    }
    public List<bool> DecodeBoolList(string boolstring)
    {
        List<bool> result = new List<bool>();
        foreach (var item in boolstring)
        {
            result.Add(item == '1');
        }
        return result;
    }

    public void RefreshPropList()
    {
        for (int i = 0; i < propdeath.Count; i++)
        {
            proplist[i].SetActive(!propdeath[i]);
        }
    }
    public void RefreshEnemyList()
    {
        for (int i = 0; i < enemydeath.Count; i++)
        {
            enemylist[i].SetActive(!enemydeath[i]);
        }
    }
    public void RefreshDoorList()
    {
        for (int i = 0; i < doordeath.Count; i++)
        {
            doorlist[i].SetActive(!doordeath[i]);
        }
    }
    //效率低，以后改成用哈希查找
    public void OnSceneObjDeath(GameObject obj)
    {
        if (obj.CompareTag("Enemy"))
        {
            //var pos = obj.transform.position;
            int i = enemylist.IndexOf(obj);
            enemydeath[i] = true;
        }
        if (obj.CompareTag("triggeritem"))
        {
            int i = proplist.IndexOf(obj);
            propdeath[i] = true;
        }
        if (obj.CompareTag("Door"))
        {
            int i = doorlist.IndexOf(obj);
            doordeath[i] = true;
        }
        SaveMgr.Instance.SaveSceneData();
    }

}

