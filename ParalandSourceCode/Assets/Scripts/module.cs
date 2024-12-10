using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class module
{
    public string name;
    public int health;
    public int attack;
    public int defence;
    public int gold;
    public int insight;
    public int key1;
    public int key2;
    public int key3;
    public int diedialog = -1;
    public EnemySkill skill;
    //public Sprite sprite;
#if UNITY_EDITOR
    public GameObject go;
#endif
}
[System.Serializable]
public class Propmodule
{
    public string name;
    public int health;
    public int attack;
    public int defence;
    public int gold;
    public int insight;
    public int key1;
    public int key2;
    public int key3;
    public bool isportrable;
    public int dialogid;
    public bool onlyShowDialogFirstTime;
    public SpecialFunc func;
}
public enum SpecialFunc
{
    None,
    UnlockRotate,
    UnlockFly,
    UnlockDig,
    UnlockFog,
    SetPerspectiveCam,
    quitgame,
    ReloadScene,
    LoadGame,
    NewGame

}
public enum EnemySkill
{
    None,
    Magic,
    Double,
    Early
}