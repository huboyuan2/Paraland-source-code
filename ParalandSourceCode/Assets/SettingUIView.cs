using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class SettingUIView : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;
    public ToggleGroup Graphic;
    public Toggle low;
    public Toggle mid;
    public Toggle high;
    // Start is called before the first frame update

    private void Awake()
    {
        BGMSlider.onValueChanged.AddListener((vol) => { SoundMgr.Instance.SetBGMVolume(vol);
            PlayerPrefs.SetFloat("BGM",vol);
        });
        SFXSlider.onValueChanged.AddListener((vol) => { SoundMgr.Instance.SetSFXVolume(vol);
            PlayerPrefs.SetFloat("SFX", vol);
        });
        low.onValueChanged.AddListener((val)=> {
            if (val)
                SetRendererScale(0.5f);
        });
        mid.onValueChanged.AddListener((val) => {
            if (val)
                SetRendererScale(0.75f);
        });
        high.onValueChanged.AddListener((val) => {
            if (val)
                SetRendererScale(1f);
        });
        InitializeSettingData();
    }
    public void QuitGame()
    {
        PopUpView.Instance.ShowPopUpWindow(string.Format("确认退出游戏？"), "确定", "取消", true, true, () => {
            Application.Quit(); 
        });
    }
    void SetRendererScale(float scale)
    {
        UniversalRenderPipeline.asset.renderScale = scale;
        PlayerPrefs.SetFloat("RenderScale", scale);
    }
    void InitializeSettingData()
    {
        if (PlayerPrefs.HasKey("BGM"))
            BGMSlider.value = PlayerPrefs.GetFloat("BGM");
        if (PlayerPrefs.HasKey("SFX"))
            BGMSlider.value = PlayerPrefs.GetFloat("SFX");
        if (PlayerPrefs.HasKey("RenderScale"))
        {
            float scale = PlayerPrefs.GetFloat("RenderScale");
            if (scale > 0.9f)
            { high.isOn = true; }
            else if (scale < 0.6f)
            { low.isOn = true; }
            else
            { mid.isOn = true; }
        }
    }
}
