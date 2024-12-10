using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEditor.SceneManagement;
public class SceneObjectIterator : EditorWindow
{
    public static List<cubeinfo> cubelist;
    [MenuItem("Custom Tools/Scene Object Iterator")]
    private static void OpenWindow()
    {
        SceneObjectIterator window = GetWindow<SceneObjectIterator>();
        window.titleContent = new GUIContent("Object Iterator");
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Iterate Objects"))
        {
            IterateSceneObjects();
        }
    }
    [MenuItem("Custom Tools/StatSceneData")]
    public static void ModifyParameter()
    {
        var enemylist = new List<GameObject>();
        var proplist = new List<GameObject>();
        var doorlist = new List<GameObject>();
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            // ��������ԶԱ�������ÿ������������һЩ����
            Debug.Log("Object Name: " + obj.name);
            if (obj.CompareTag("Enemy"))
            {
                //var pos = obj.transform.position;
                enemylist.Add(obj);
            }
            if (obj.CompareTag("triggeritem"))
            {
                //var pos = obj.transform.position;
                proplist.Add(obj);
            }
            if (obj.CompareTag("Door"))
            {
                //var pos = obj.transform.position;
                doorlist.Add(obj);
            }
        }
        //string jsonStr = LitJson.JsonMapper.ToJson(cubelist).Replace("],", "],\n");
        //File.WriteAllText(Application.dataPath + "/cubes.txt", jsonStr);

        GameObject selectedObject = GameObject.Find("persistentSceneData");

        if (selectedObject != null)
        {
            // ��ȡ������ѡ�������ϵĽű����
            PersistentSceneData scriptComponent = selectedObject.GetComponent<PersistentSceneData>();

            if (scriptComponent != null)
            {
                // �޸Ľű�����ֵ
                scriptComponent.proplist = proplist;
                scriptComponent.enemylist = enemylist;
                // ��ǳ���Ϊ���޸ģ���ѡ��
                EditorUtility.SetDirty(selectedObject);
                EditorSceneManager.MarkSceneDirty(selectedObject.scene);
            }
            else
            {
                Debug.LogWarning("ScriptComponent not found on selected object.");
            }
        }
        else
        {
            Debug.LogWarning("No object selected.");
        }
    }
    private void IterateSceneObjects()
    {
        cubelist = new List<cubeinfo>();
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            // ��������ԶԱ�������ÿ������������һЩ����
            Debug.Log("Object Name: " + obj.name);
            if (obj.name.Contains("Cube"))
            {
                var pos = obj.transform.position;
                cubelist.Add(new cubeinfo((int)pos.x, (int)pos.y, (int)pos.z));
            }
        }
        string jsonStr = LitJson.JsonMapper.ToJson(cubelist).Replace("],", "],\n");
        File.WriteAllText(Application.dataPath + "/cubes.txt", jsonStr);

    }
    public struct cubeinfo
    {
        public int x;
        public int y;
        public int z;
        public cubeinfo(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }


}