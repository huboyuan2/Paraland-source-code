using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Enemyindex;
    private Animator anim;
    private Renderer rend;
    public int diedialog;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroySelf()
    {
        //gameObject.SetActive(false);
        StartCoroutine(DefeatAnim());
    }
    IEnumerator DefeatAnim()
    {
        Enemyindex = -1;
           Quaternion quaternion = Quaternion.identity;
        quaternion.SetLookRotation(CharacterAnim.Instance.transform.forward*-1f);
        transform.rotation = quaternion;
        if (anim != null)
        {
            anim.SetTrigger("Battle");

            if (rend != null)
            {
                float index = 0f;
                while (index < 1f)
                {
                    rend.material.SetFloat("_Dissolve", index);
                    index += Time.deltaTime*0.5f;
                    yield return null;
                }

            }
            else
                yield return new WaitForSeconds(2f);
        }
        gameObject.SetActive(false);
        SaveMgr.Instance.RecordDeadSceneObj(gameObject);
        if (diedialog > 0)
        {
            DialogMgr.Instance.CheckDialog(diedialog);
        }
    }
}
