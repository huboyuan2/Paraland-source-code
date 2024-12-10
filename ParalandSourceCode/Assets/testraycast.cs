using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testraycast : MonoBehaviour
{
    //public float scale = 1f;
    //RaycastHit[] dirs = new RaycastHit[4];
    public Transform chatacter;
    //2.12132f,1.22474487f
    Vector3[] offsetGroup = new Vector3[4] { new Vector3(-2.12132f, 1.22474487f, 0f), new Vector3(2.12132f, 1.22474487f, 0f), new Vector3(2.12132f, -1.22474487f, 0f) , new Vector3(-2.12132f, -1.22474487f, 0f)};
    private static testraycast _instance;
    public static testraycast Instance
    {
        get
        {
            if (_instance == null)
                _instance = (testraycast)FindObjectOfType(typeof(testraycast));
            return _instance;
        }
    }
    private void Awake()
    {
        if (FindObjectsOfType<testraycast>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var item in CharacterPathFind.Instance.moveTargets)
        //{
        //    Debug.LogError(Camera.main.WorldToViewportPoint(item.transform.position));
        //}
    }
    public void TestRay(int index)
    {
        bool temp0=false;
        //bool temp1=false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.TransformDirection(offsetGroup[index]*0.9f), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity,1<<0))
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + transform.TransformDirection(offsetGroup[index]*0.9f), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow,100);
#endif
            //Debug.Log("upleft Did Hit");
            if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
            {
                temp0 = true;
                //CharacterPathFind.Instance.dirAccess[index] = true;
                CharacterPathFind.Instance.moveTargets[index] = hit.collider.gameObject;
            }
            else
            {
                temp0 = false;
                //CharacterPathFind.Instance.dirAccess[index] = false;   
                CharacterPathFind.Instance.moveTargets[index] = hit.collider.gameObject;
            }
            if (temp0)
                CharacterPathFind.Instance.dirAccess[index] = true;
            else
                CharacterPathFind.Instance.dirAccess[index] = false;
            if (hit.collider.gameObject.CompareTag("earthcube"))
            {
                if ((index < 2) && CompareDepth(CharacterPathFind.Instance.moveTargets[index].transform.position, chatacter.position - new Vector3(0, 1.5f, 0)) == 0)
                    CharacterPathFind.Instance.dirAccess[index] = false;
                else if ((index >= 2) && CompareDepth(CharacterPathFind.Instance.moveTargets[index].transform.position, chatacter.position - new Vector3(0, 1.5f, 0)) == 1)
                    CharacterPathFind.Instance.dirAccess[index] = false;
            }
        }


        //Debug.LogError((index.ToString())+ (CompareDepth(CharacterPathFind.Instance.moveTargets[index].transform.position).ToString()));
    }
    //用于A*寻路时判断
    public bool TestAcceess(int index, Vector3 pos, out GameObject cube, bool isstair = false)
    {
        bool result = false;

        bool downstair = (index > 1) && isstair;
        Vector3 camoffset = offsetGroup[index];
        //下楼梯的情况需要特殊处理
        if (downstair)
            camoffset.y *= 3f;
        cube = null;
        Vector3 virtualcampos = pos + new Vector3(0, 1.5f, 0) - Camera.main.gameObject.transform.forward * 200f;
        RaycastHit hit;
        if (Physics.Raycast(virtualcampos + transform.TransformDirection(camoffset * 0.95f), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, 1 << 0))
        {
#if UNITY_EDITOR
            Debug.DrawRay(virtualcampos + transform.TransformDirection(camoffset * 0.95f), transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 10);
#endif
            //Debug.Log("upleft Did Hit");
            result = Vector3.Dot(hit.normal, Vector3.up) > 0.5f;

            var go = hit.collider.gameObject;
            if (go)
            {
                //楼梯和桥不允许从侧面通过
                if (go.CompareTag("Stair") || go.CompareTag("railcube"))
                {
                    if (!CharacterPathFind.Instance.JudgeWalkable(go, CharacterPathFind.Instance.rotatedirs[(CamFollow.Instance.target + index) % 4]))
                    {
                        result = false;
                        //Debug.LogError("不允许通过");
                    }
                }
                if (go.CompareTag("Enemy") || go.CompareTag("Door")|| go.CompareTag("NPC"))
                {
                   // if (!CharacterPathFind.Instance.JudgeWalkable(go, CharacterPathFind.Instance.rotatedirs[(CamFollow.Instance.target + index) % 4]))
                    //{
                        result = false;
                        //Debug.LogError("不允许通过");
                    //E}
                }
                if (!go.CompareTag("Stair"))
                {
                    if ((index < 2) && CompareDepth(go.transform.position, pos) == 0)
                        result = false;
                }
                else if ((index >= 2) && CompareDepth(go.transform.position, pos) == 1)
                    result = false;
                if (result)
                {
                    cube = hit.collider.gameObject;
                }
            }
        }

        return result;
    }
    public int CompareDepth(Vector3 targetpos,Vector3 curpos)
    {
        float targetdepth = Camera.main.WorldToScreenPoint(targetpos).z;
        float curdepth = Camera.main.WorldToScreenPoint(curpos).z;
        if (targetdepth - curdepth > 0.01f)
            return 1;
        else if ((curdepth - targetdepth > 0.01f)) return 0;
        else return -1;
    }
    public void RayUpdate()
    {
        //RaycastHit hit;
        TestRay(0);
        TestRay(1);
        TestRay(2);
        TestRay(3);
    }

}
