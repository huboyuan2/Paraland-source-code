using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TextInfos", menuName = "ScriptableObjects/DialogScriptableObject", order = 2)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    //public string prefabName;

    //public int numberOfPrefabsToCreate;
    public TextInfo[] spawnTexts;
}
[System.Serializable]
public class TextInfo
{
    public string character;
    public string dialog;
    public string dialogEng;
    public bool isSelect;
    public int outcome;
    public List<int> nexts;
    public int next1;
    public int next2;
    public int next3;
}
