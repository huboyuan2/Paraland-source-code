using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SaveMgr : MonoBehaviour
{
    private static SaveMgr _instance;
    PersistentSceneData sceneData;
    // Start is called before the first frame update
    public static SaveMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = (SaveMgr)FindObjectOfType(typeof(SaveMgr));
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
       CharacterM.OnValueChange += SaveData;
        sceneData = (PersistentSceneData)FindObjectOfType(typeof(PersistentSceneData));
    }
    //场景切换时更新当前场景数据
    public void ChangeSceneData()
    {
        sceneData = (PersistentSceneData)FindObjectOfType(typeof(PersistentSceneData));
    }
     public void SaveData()
    {
        if (CharacterM.Instance.Health <= 0)
            return;
        string datastr =CharacterM.Instance.EncodeData();
        PlayerPrefs.SetString("data",datastr);
        //SavePosData();
        //SaveSceneData();
    }
    public void SavePosData()
    {
        //string.Format("{0}#{1}#{2}", transform.position.x.ToString(), transform.position.y.ToString(), transform.position.z.ToString());
        PlayerPrefs.SetFloat("posx", transform.position.x);
        PlayerPrefs.SetFloat("posy", transform.position.y);
        PlayerPrefs.SetFloat("posz", transform.position.z);

    }
    public void LoadPosData()
    {
        //Debug.LogError("LoadPos");
        if (PlayerPrefs.HasKey("posx"))
        {
            //Debug.LogError("LoadPos1"+ PlayerPrefs.GetFloat("posx").ToString());
            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("posx"), PlayerPrefs.GetFloat("posy"), PlayerPrefs.GetFloat("posz"));
            transform.position = pos;
        }
    }
    public void SaveSceneData()
    {
        string datastr = sceneData.SaveSceneData();
        PlayerPrefs.SetString("scenedata", datastr);
    }
    public void RecordDeadSceneObj(GameObject obj)
    {
        //SavePosData();
        sceneData.OnSceneObjDeath(obj);
    }
    public void LoadData()
    {
        //Debug.LogError("LoadData");
        if (PlayerPrefs.HasKey("data"))
        {
           
            string loadstr = PlayerPrefs.GetString("data");
            //Debug.LogError(loadstr);
            CharacterM.Instance.DecodeData(loadstr);
            if (PlayerPrefs.HasKey("scenedata"))
            {
                //Debug.LogError(PlayerPrefs.GetString("scenedata"));
                sceneData.LoadSceneData(PlayerPrefs.GetString("scenedata"));
            }
            LoadPosData();
        }
    }
}
