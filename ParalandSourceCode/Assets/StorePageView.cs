using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePageView : MonoBehaviour
{
    public Button healthButton;
    public Button attackButton;
    public Button defenceButton;
    public Button cancelButton;
    // Start is called before the first frame update

    private void Awake()
    {
        healthButton.onClick.AddListener(BuyHealth);
        attackButton.onClick.AddListener(BuyAtt);
        defenceButton.onClick.AddListener(BuyDef);
        cancelButton.onClick.AddListener(CloseSelf);
        CharacterPathFind.PlayerMove += OnMove;
    }
    private void OnMove(int i,bool isnav)
    {
        CloseSelf();
    }
    void CloseSelf()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        SetEnable();
    }
    void BuyHealth()
    {
        if (CharacterM.Instance.Gold >= 20)
        {
            CharacterM.Instance.Gold -= 20;
            CharacterM.Instance.Health += 500;
        }
        if (CharacterM.Instance.Gold < 20)
            SetDisable();
    }

    void BuyAtt()
    {
        if (CharacterM.Instance.Gold >= 20)
        {
            CharacterM.Instance.Gold -= 20;
            CharacterM.Instance.Attack += 3;
        }
        if (CharacterM.Instance.Gold < 20)
            SetDisable();
    }
    void BuyDef()
    {
        if (CharacterM.Instance.Gold >= 20)
        {
            CharacterM.Instance.Gold -= 20;
            CharacterM.Instance.Defence += 3;
        }
        if (CharacterM.Instance.Gold < 20)
            SetDisable();
    }
    void SetDisable()
    {
        healthButton.interactable = false;
        attackButton.interactable = false;
        defenceButton.interactable = false;
    }
    void SetEnable()
    {
        healthButton.interactable = true;
        attackButton.interactable = true;
        defenceButton.interactable = true;
    }
}
