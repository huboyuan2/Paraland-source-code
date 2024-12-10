using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CamFollow : MonoBehaviour
{
    public Camera mycam;
    private static CamFollow _instance;
    public float rotateVelocity = 1f;
    // Start is called before the first frame update
    public GameObject camtarget;
    public Vector3 Offset=Vector3.zero;
    public bool indrag = false;
    public bool isrotating;
    public bool canrotate=true;
    public Transform bornplace;
    float distance = 100f;
    Vector3[] povGroup = new Vector3[4] { new Vector3(-1f, 1f, 1f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, -1f), new Vector3(-1f, 1f, -1f) };
    public int target = 100;
    public int now = 100;
    float rate = 0;
    public static CamFollow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (CamFollow)FindObjectOfType(typeof(CamFollow));
            }
                
            return _instance;
        }
    }
    //void Start()
    //{
    //    Debug.LogError("OnStart");
    //}
    private void Awake()
    {
        if (FindObjectsOfType<CamFollow>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
    }
    public void FindTarget()
    {
        camtarget = GameObject.Find("3rdPersonController");
        //if (!camtarget)
        //{
        //    camtarget = Instantiate(Resources.Load<GameObject>("3rdPersonController"));
        //}
        testraycast.Instance.chatacter = camtarget.transform;
    }
    public void ChangeCamRange(float val)
    {
        mycam.orthographicSize += val;
        if (mycam.orthographicSize > 24f)
            mycam.orthographicSize = 24f;
        if (mycam.orthographicSize < 2f)
            mycam.orthographicSize = 2f;
    }
    public void AddCamRange()
    {
        //Mathf.Clamp(2f, 12f, mycam.orthographicSize + 2f);
        mycam.orthographicSize += 2f;
        if(mycam.orthographicSize>24f)
            mycam.orthographicSize =24f;

    }
    public void DecCamRange()
    {
        //Mathf.Clamp(2f, 12f, mycam.orthographicSize + 2f);
        mycam.orthographicSize -= 2f;
        if (mycam.orthographicSize < 2f)
            mycam.orthographicSize = 2f;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{ TurnRight();           
        //    //StartCoroutine("CamRotate", povGroup[target % 4]);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{ TurnLeft();
        //    //StartCoroutine("CamRotate", povGroup[target % 4]);
        //}
        if (now != target)
        {
            if (rate < 1.57f)
            {
                isrotating = true;
                rate += Time.deltaTime*rotateVelocity;
                transform.position = camtarget.transform.position + Vector3.RotateTowards(povGroup[now % 4]*distance - Vector3.up * distance, povGroup[target % 4] * distance - new Vector3(0, 1f, 0) * distance, rate, 0) +Vector3.up * distance;
                transform.LookAt(camtarget.transform);
            }
            else
            {
                now = target;
                isrotating = false;
            }
        }
        else transform.position = camtarget.transform.position +Offset+ povGroup[target % 4] * distance;
        //transform.position=camtarget.transform.position+ Vector3.Lerp(transform.position, povGroup[target%4], 0.01f);
        //transform.position = camtarget.transform.position + Vector3.Slerp(povGroup[now % 4], povGroup[target % 4], rate);

        //Quaternion rotation = Quaternion.LookRotation(-povGroup[target % 4], Vector3.up);
        //transform.rotation = rotation;
    }
    public void SetOffset(Vector3 dragdelta)
    {
    Offset= transform.right*dragdelta.x + transform.up * dragdelta.y;
    }
    public void SetOffset(Vector2 dragdelta)
    {
        Offset += transform.right * dragdelta.x + transform.up * dragdelta.y;
    }
    //private void OnMouseDown()
    //{
    //    Debug.LogError("OnMouseDown");
    //}
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    //Offset = Vector3.zero;
    //}
    //public void OnDrag(PointerEventData data)
    //{
    //    Debug.LogError("OnDrag");
    //    //if (m_DraggingIcon != null)
    //    //    SetDraggedPosition(data);
    //    //Offset = data.worldPosition;
    //}
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    //Offset = Vector3.zero;
    //}
    public void TurnLeft()
    {
        if (now == target&&canrotate)
        {
            rate = 0;
            target--;
        }
    }
    public void TurnRight()
    {
        if (now == target&&canrotate)
        {
            rate = 0;
            target++;
        }
    }
    public void SetPerspective()
    {
        StartCoroutine("ZoomAnim");
    }
    IEnumerator ZoomAnim()
    {
        var cam = Camera.main;
        cam.fieldOfView = 10f;
        cam.orthographic = false;
        while (distance >= 20)
        {
            if (now == target)
            {
                target++;
                rate = 0f;
            }
            distance -= 0.2f;
            cam.fieldOfView += 0.12f;
            yield return null;
        }
        //while (true)
        //{
        //    if (now == target)
        //    { 
        //        target++;
        //        rate = 0f;
        //    }
        //    yield return null;
        //}
    }

}
