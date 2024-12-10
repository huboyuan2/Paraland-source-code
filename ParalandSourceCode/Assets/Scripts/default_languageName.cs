using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class default_languageName : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<TMP_Text>().text = localizationManager.instance.lang.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
