using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModelPanelView : MonoBehaviour
{
    // Start is called before the first frame update
    
    public TMP_Text[] textgroup=new TMP_Text[5];
    void Awake()
    {
        CharacterM.OnValueChange += RefreshData;
    }
    void RefreshData()
    {
        textgroup[0].text = CharacterM.Instance.Health.ToString();
        textgroup[1].text = CharacterM.Instance.Attack.ToString();
        textgroup[2].text = CharacterM.Instance.Defence.ToString();
        textgroup[3].text = CharacterM.Instance.Gold.ToString();
        textgroup[4].text = CharacterM.Instance.Key[0].ToString();
    }
}
