using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAnim : MonoBehaviour
{
    private Animator anim;
    private Renderer renderer;
    private Rigidbody rb;
    private static CharacterAnim _instance;

    public static CharacterAnim Instance
    {
        get
        {
            if (_instance == null)
                _instance = (CharacterAnim)FindObjectOfType(typeof(CharacterAnim));
            return _instance;
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if (FindObjectsOfType<CharacterAnim>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        anim = transform.Find("MaleCharacterPBR").gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (anim == null)
            Debug.LogError("Anim==null!!");
        CharacterPathFind.PlayerMove += SetRun;
        //anim.SetTrigger("battle");
    }
    public void SetBattle()
    {
        anim.SetTrigger("battle");
        EffectMgr.Instance.PlayEffect(2, transform);
    }
    public void SetNavRun(bool val)
    {
        anim.SetBool("navrun",val);
    }
    public void SetRun(int target,bool isnav)
    {
        var go = CharacterPathFind.Instance.moveTargets[target];
        //Debug.LogError();
        if (rb.useGravity)
        {
            if (!isnav)
            {
                
                if (go.CompareTag("railcube")||go.CompareTag("Stair"))
                {
                    if (!CharacterPathFind.Instance.JudgeWalkable(go, transform.forward))
                        return;
                }
                anim.SetTrigger("run");
            }
            
            if (CharacterPathFind.Instance.moveTargets[target].CompareTag("Stair"))
            {
                StartCoroutine(WalkOnStair(target, isnav ? () => {
                    if (CharacterPathFind.Instance.CloseList.Count > 0)
                    {
                        CharacterPathFind.Instance.CloseList.RemoveAt(0);
                    }
                        CharacterPathFind.Instance.WalkAlongNavRoad(); } : null));
            }
            else
            StartCoroutine(moveFoward(isnav?0.3f:0.5f, new Vector3(0, 0, 3f), target, go.transform.position,isnav?()=>{ CharacterPathFind.Instance.WalkAlongNavRoad(); }:null));
        }
    }
    //左上右上先平移再偷偷变位置，左下右下先偷偷变位置再平移
    IEnumerator moveFoward(float time, Vector3 distance, int targetid,Vector3 goal, Action callback = null)
    {

        //这里有点耦合，可以改进
            //Vector3 goal = CharacterPathFind.Instance.moveTargets[targetid].transform.position;
            float t = 0f;
            rb.useGravity = false;
            if (targetid >= 2)
            {
                transform.position = goal + new Vector3(0, 1.5f, 0) - transform.forward * 3f;
            }
            while (t < time)
            {
                transform.Translate(distance / time * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            //if (targetid < 2)
            {
                transform.position = goal + new Vector3(0, 1.5f, 0);
            }
            rb.useGravity = true;
        callback?.Invoke();
    }
    IEnumerator WalkOnStair(int targetid,Action callback=null)
    {
        GameObject stair = CharacterPathFind.Instance.moveTargets[targetid];
        bool isup = targetid < 2;
        //Debug.LogError(targetid);
        Vector3 virtualtarget = stair.transform.position + (stair.transform.right) * (isup ? 3f : -3f) - (isup ? Vector3.zero : stair.transform.up*3f)+new Vector3(0,1.5f,0);
        RaycastHit hit;
        Physics.Raycast(virtualtarget - Camera.main.transform.forward*20f, Camera.main.transform.forward, out hit, Mathf.Infinity, 1 << 0);
        if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
        {
            float t = 0f;
            rb.useGravity = false;
            anim.speed = 0.5f;
            SoundMgr.Instance.StepFrequest *= 2f;
            if (targetid >= 2)
            {
                transform.position = stair.transform.position + new Vector3(0, 1.5f, 0) - transform.forward * 3f;
            }
            else if (testraycast.Instance.CompareDepth(stair.transform.position, transform.position) == 0)
            {
                transform.position = stair.transform.position - new Vector3(0, 1.5f, 0) - transform.forward * 3f;
            }
            while (t <0.25f)
            {
                transform.Translate(new Vector3(0,0,1.5f) / 0.25f * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            while (t < 0.5f)
            {
                transform.Translate((isup? new Vector3(0, 1.5f, 1.5f) : new Vector3(0, -1.5f, 1.5f)) / 0.25f * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            if (targetid >= 2)
            {
                if (testraycast.Instance.CompareDepth(stair.transform.position, hit.collider.transform.position) == 1)
                    transform.position = hit.collider.transform.position + new Vector3(0, 3f, 0) - transform.forward * 3f;
            }
            while (t < 0.75f)
            {
                transform.Translate((isup ? new Vector3(0, 1.5f, 1.5f) : new Vector3(0, -1.5f, 1.5f)) / 0.25f * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            while (t < 1f)
            {
                transform.Translate(new Vector3(0, 0, 1.5f) / 0.25f * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            transform.position = hit.collider.transform.position + new Vector3(0, 1.5f, 0);
            anim.speed = 1f;
            SoundMgr.Instance.StepFrequest /= 2f;
            rb.useGravity = true;
            callback?.Invoke();
        }
        else
            Debug.LogError("无法使用楼梯");
        //while (true)
        //{
#if UNITY_EDITOR
        Debug.DrawRay(virtualtarget - Camera.main.transform.forward * 20f, Camera.main.transform.forward * hit.distance, Color.blue,100);
#endif
        yield return null;
        //}
    }
    void Start()
    {
        
    }
    public void InitPos()
    {
        var bornplace = GameObject.Find("bornplace");
        if(bornplace!=null)
        transform.position=bornplace.transform.position;
    }

}
