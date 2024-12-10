using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PropMgr : MonoBehaviour
{
    public PropMScriptObject PropConfig;
    private static PropMgr _instance;
    // Start is called before the first frame update
    public static PropMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = (PropMgr)FindObjectOfType(typeof(PropMgr));
            return _instance;
        }
    }
    public void InvolkeUniqueFunction(SpecialFunc func)
    {
        switch (func)
        {
            case SpecialFunc.None:
                break;
            case SpecialFunc.UnlockDig:
                CharacterM.Instance.CanDig = true;
                break;
            case SpecialFunc.UnlockFly:
                break;
            case SpecialFunc.UnlockRotate:
                CharacterM.Instance.Canrotate = true;
                //MovePanelView.Instance.LockRockBtn(false);

                break;
            case SpecialFunc.UnlockFog:
                //MoveFog.Instance.InitialHeight =-9f;
                CharacterM.Instance.FogUnlock = true;
                break;
            case SpecialFunc.SetPerspectiveCam:
                CamFollow.Instance.SetPerspective();
                break;
            case SpecialFunc.quitgame:
                PopUpView.Instance.ShowPopUpWindow(string.Format("确认退出游戏？"), "确定", "取消", true, true, () => {
                    Application.Quit();
                });
                break;
            case SpecialFunc.ReloadScene:
                PopUpView.Instance.ShowPopUpWindow(string.Format("确认重玩游戏？"), "确定", "取消", true, true, () => {

                    //SceneManager.LoadScene(0);
                    LoadSceneAsync(0, () => { CharacterM.Instance.RefreshData(); });
                    
                });
                break;
            case SpecialFunc.LoadGame:
                SaveMgr.Instance.LoadData();
                break;
            case SpecialFunc.NewGame:
                PlayerPrefs.DeleteAll();
                CharacterM.Instance.RefreshData();
                break;
        }
    }
    public void LoadScene(int i, System.Action callback)
    {
        StartCoroutine(LoadSceneAsync(i,callback));
    }
    //不该写在这里
    IEnumerator LoadSceneAsync(int i, System.Action callback)
    {
        yield return SceneManager.LoadSceneAsync(i);
        callback?.Invoke();
    }
}
