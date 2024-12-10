using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterEffect : MonoBehaviour
{
    Renderer chararend;
    float dissindex;
    // Start is called before the first frame update
    void Start()
    {
        dissindex = 0f;
        chararend = gameObject.GetComponent<Renderer>();
        //chararend.material.SetFloat("_Dissolve", 0.5f);
        //StartCoroutine("CharDissolve",0.1f);
    }

    public void Dissolve(float speed = 1f)
    {
        StartCoroutine("CharDissolve",speed);
    }
    public void Appear(float speed = 1f)
    {
        StartCoroutine("CharAppear",speed);
    }

    IEnumerator CharDissolve(float speed =1f)
    {
        while (dissindex<1f)
        {
            dissindex += Time.unscaledDeltaTime*speed;
            //float dis = Mathf.PingPong(Time.time * 0.1f, 1.0f);
            //Debug.Log(dissindex);
            chararend.material.SetFloat("_Dissolve", dissindex);

            yield return new WaitForEndOfFrame();
        }
        chararend.material.SetFloat("_Dissolve", 1f);
        yield return null;
    }
    IEnumerator CharAppear(float speed = 1f)
    {
        while (dissindex > 0.01f)
        {
            dissindex -= Time.unscaledDeltaTime * speed;
            //float dis = Mathf.PingPong(Time.time * 0.1f, 1.0f);
            //Debug.Log(dissindex);
            chararend.material.SetFloat("_Dissolve", dissindex);

            yield return new WaitForEndOfFrame();
        }
        chararend.material.SetFloat("_Dissolve", 0f);
        yield return null;
    }
}
