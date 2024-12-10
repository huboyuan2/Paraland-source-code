using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

public class localizationManager : MonoBehaviour
{
    public static localizationManager instance;

    public  Lang lang = new Lang();

    public  int lang_num;

    public TMP_Text lang_text;

    public enum Lang
    { 
     简体中文,English
    }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("language"))
        {
            lang_num = PlayerPrefs.GetInt("language");
            lang = (Lang)lang_num;
        }
        else
        {
            lang_num = 1;
            change_language(lang_num);
            lang = (Lang)lang_num;
            lang_text.text = lang.ToString();
            
        }
    }

    public void next_language()
    {
        lang_num = (lang_num + 1) % 2;  //以后这个值改为支持的语言数
        lang = (Lang)lang_num;
        lang_text.text = lang.ToString();
        //切语言
        change_language(lang_num);
    }

    public void last_language()
    {
        lang_num = (lang_num + 2 - 1) % 2;  //以后这个值改为支持的语言数
        lang = (Lang)lang_num;
        lang_text.text = lang.ToString();
        //切语言
        change_language(lang_num);
    }

    public void change_language(int langnum)
    {
        if (langnum == 0)
        {
            I2.Loc.LocalizationManager.CurrentLanguage = "Chinese";
            PlayerPrefs.SetInt("language", langnum);
            return;
        }
        if (langnum == 1)
        {
            I2.Loc.LocalizationManager.CurrentLanguage = "English";
            PlayerPrefs.SetInt("language", langnum);
            return;
        }
        //if (langnum == 2)
        //{
        //    I2.Loc.LocalizationManager.CurrentLanguage = "Japanese";
        //    PlayerPrefs.SetInt("language", langnum);
        //    return;
        //}
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
