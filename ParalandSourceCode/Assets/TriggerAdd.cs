using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAdd : MonoBehaviour
{
    //public int addhealth;
    //public int addatt;
    //public int adddef;
    //public int addgold;
    //public int addkey;

    public int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Propmodule m = PropMgr.Instance.PropConfig.proplist[index];
            if (m.health > 0)
            {
                EffectMgr.Instance.PlayEffect(1,CharacterAnim.Instance.transform);
                SoundMgr.Instance.PlayAudio(4);
            }
            if (m.attack > 0|| m.defence>0)
            {
                EffectMgr.Instance.PlayEffect(0, CharacterAnim.Instance.transform);
                SoundMgr.Instance.PlayAudio(5);
            }
            CharacterM.Instance.Health += m.health;
            CharacterM.Instance.Attack += m.attack;
            CharacterM.Instance.Defence += m.defence;
            CharacterM.Instance.Gold += m.gold;
            CharacterM.Instance.Key = new int[3] { CharacterM.Instance.Key[0] + m.key1, CharacterM.Instance.Key[1] + m.key2, CharacterM.Instance.Key[2] + m.key3 };
            //CharacterM.Instance.Key[0] += m.key1;
            //CharacterM.Instance.Key[1] += m.key2;
            //CharacterM.Instance.Key[2] += m.key3;
            if (m.dialogid > 0)
            {
                DialogMgr.Instance.CheckDialog(m.dialogid);
            }
            gameObject.SetActive(false);
            SaveMgr.Instance.RecordDeadSceneObj(gameObject);
            if (m.func != SpecialFunc.None)
            {
                PropMgr.Instance.InvolkeUniqueFunction(m.func);
            }
        }
        
    }
}
