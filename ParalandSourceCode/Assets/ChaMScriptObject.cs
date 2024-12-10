
//using UnityEngine;

////[System.Serializable]
//[CreateAssetMenu(fileName = "modules", menuName = "ScriptableObjects/modulesScriptableObject", order = 1)]
//public class SpawnManagerScriptableObject : ScriptableObject
//{
//    //public string prefabName;

//    //public int numberOfPrefabsToCreate;
//    public module[] tests;
//}
using UnityEngine;

[CreateAssetMenu(fileName = "TextAssets", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class ChaMScriptObject : ScriptableObject
{
    //public string prefabName;

    //public int numberOfPrefabsToCreate;
    public string[] spawnTexts;
    public module[] tests;
}
