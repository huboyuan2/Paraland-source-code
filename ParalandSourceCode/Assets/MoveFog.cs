using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFog : MonoBehaviour
{
    float initialHeight;
    public GameObject UnderFogArea;
    private static MoveFog _instance;
    public static MoveFog Instance
    {
        get
        {
            if (_instance == null)
                _instance = (MoveFog)FindObjectOfType(typeof(MoveFog));
            return _instance;
        }
    }
    private void Start()
    {
        UnderFogArea.SetActive(false);
    }
    public float InitialHeight 
    {
        get
        {
            return initialHeight;
        }
        set
        {
            initialHeight = value;
            StartCoroutine("ChangeFogHeightAsync");
            UnderFogArea.SetActive(true); 
            //OnValueChange.Invoke();
        }
    }

    //public Vector3 pos;
    //private void Start()
    //{
    //    InitialHeight = transform.position.y;
    //}
    //private void Update()
    //{

    //    transform.position = CharacterAnim.Instance.transform.position+ CamFollow.Instance.Offset;
    //    transform.position = new Vector3(transform.position.x,InitialHeight,transform.position.z);
    //}
    IEnumerator ChangeFogHeightAsync()
    {
        while (transform.position.y > InitialHeight)
        {
            transform.Translate(new Vector3(0, Time.deltaTime * -1f, 0));
            yield return null;
        }

    }
}
