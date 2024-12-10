using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanelView : MonoBehaviour
{
    // Start is called before the first frame update
    public Button ulButton;
    public Button urButton;
    public Button drButton;
    public Button dlButton;
    public Button farButton;
    public Button nearButton;
    public Button turnright;
    public Button turnLeft;
    public Image left;
    public Image right;

    private static MovePanelView _instance;
    Color initialcolor;
    public static MovePanelView Instance
    {
        get
        {
            if (_instance == null)
                _instance = (MovePanelView)FindObjectOfType(typeof(MovePanelView),true);
            return _instance;
        }
    }
    private void Awake()
    {
        if (FindObjectsOfType<MovePanelView>().Length > 1)
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        DontDestroyOnLoad(transform.parent.gameObject);
        //ulButton.onClick.AddListener(CharacterPathFind.Instance.MoveUL);
        //urButton.onClick.AddListener(CharacterPathFind.Instance.MoveUR);
        //drButton.onClick.AddListener(CharacterPathFind.Instance.MoveDR);
        //dlButton.onClick.AddListener(CharacterPathFind.Instance.MoveDL);
        farButton.onClick.AddListener(CamFollow.Instance.AddCamRange);
        nearButton.onClick.AddListener(CamFollow.Instance.DecCamRange);
        turnright.onClick.AddListener(CamFollow.Instance.TurnRight);
        turnLeft.onClick.AddListener(CamFollow.Instance.TurnLeft);
        initialcolor = left.color;
        LockRockBtn(!CharacterM.Instance.Canrotate);
    }

    // Update is called once per frame
    public void LockRockBtn(bool freeze)
    {

        turnLeft.interactable = !freeze;
        turnright.interactable = !freeze;
        left.color = freeze ? turnLeft.colors.disabledColor : initialcolor;
        right.color = freeze ? turnright.colors.disabledColor : initialcolor;
    }
}
