using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : MonoBehaviour
{
    
    private static EffectMgr _instance;
    public static EffectMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = (EffectMgr)FindObjectOfType(typeof(EffectMgr));
            return _instance;
        }
    }
    // ͵����д��,����Ǵ�AB����
    public List<GameObject> EffectList;
    public void PlayEffect(int effid,Transform parent)
    {
        GameObject effobj = EffectList[effid];
        if (effobj != null)
        {
            Instantiate(effobj,parent);
        }
    }
}
