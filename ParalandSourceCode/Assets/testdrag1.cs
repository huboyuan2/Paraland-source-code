using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testdrag1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    Vector3 delta = Vector3.zero;
    private Camera mainCamera; // 主摄像机
    private GameObject selectedObject; // 被选中的物体
    private float initialDistance;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //if (!CamFollow.Instance.isrotating && !CharacterPathFind.Instance.isNavigating && !SubtitleView.Instance.gameObject.activeSelf)
        {

            CamFollow.Instance.ChangeCamRange(Input.mouseScrollDelta.y);
                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    if (touch2.phase == TouchPhase.Began)
                    {
                        initialDistance = Vector2.Distance(touch1.position, touch2.position);
                        //initialScale = transform.localScale.x;
                    }
                    else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                    {
                        float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                        float scaleFactor = currentDistance - initialDistance;
                        CamFollow.Instance.ChangeCamRange(scaleFactor*(-0.005f));
                    }
                }

        }
    }
    void OnMouseDown()
    {
        // load a new scene
        Debug.LogError("Onclick");
    }
    // Update is called once per frame
    public void OnBeginDrag(PointerEventData eventData)
    {
        selectedObject = null;
        CamFollow.Instance.indrag = true;
        CamFollow.Instance.Offset = Vector3.zero;
        //startdragpos = Input.mousePosition;
    }
    public void OnDrag(PointerEventData data)
    {
        CamFollow.Instance.SetOffset(data.delta * -0.01f);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        CamFollow.Instance.indrag = false;
        CamFollow.Instance.Offset = Vector3.zero;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CamFollow.Instance.isrotating)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 0))
            {
                // 如果射线击中了一个物体
                GameObject clickedObject = hit.collider.gameObject;

                // 检查是否为可选中的物体，例如通过标签或特定组件进行判断
                if (clickedObject.CompareTag("earthcube") || clickedObject.CompareTag("railcube"))
                {
                    // 如果之前已经选中了物体，可以在这里进行取消选中的逻辑
                    if (selectedObject != null)
                    {
                        // 取消选中的逻辑，例如修改材质等
                    }

                    // 选中新的物体
                    selectedObject = clickedObject;

                    // 在这里可以进行选中后的逻辑，例如修改材质或执行其他操作
                }
                else if (clickedObject.CompareTag("Enemy"))
                {

                    int enmindex = clickedObject.GetComponent<Enemy>().Enemyindex;
                    if (CharacterPathFind.Instance.JudgeAdjacent(clickedObject))
                    BattleMgr.Instance.currentEnm = clickedObject;
                    EnemyUIView.Instance.CurTarget = clickedObject.transform;
                    BattleMgr.Instance.GetEnemyInfoFromConfig(enmindex);

                }
                else if (clickedObject.CompareTag("Door"))
                {
                    int doorindex = clickedObject.GetComponent<DoorTrigger>().DoorType;
                    if (CharacterPathFind.Instance.JudgeAdjacent(clickedObject))
                    {
                        BattleMgr.Instance.currentDoor = clickedObject;
                        BattleMgr.Instance.OpenDoorUI(doorindex);
                    }
                }
                else if (clickedObject.CompareTag("NPC"))
                {
                    //Debug.LogError("NPC");
                    if (CharacterPathFind.Instance.JudgeAdjacent(clickedObject))
                    {
                        int npcindex = clickedObject.GetComponent<NPC>().index;
                        BattleMgr.Instance.DealWithNPC(npcindex);
                    }
                }
            }
            else
            {
                // 如果点击的地方没有击中物体，可以在这里进行取消选中的逻辑
                if (selectedObject != null)
                {
                    // 取消选中的逻辑，例如修改材质等
                    selectedObject = null;
                }
            }
        }
        if (selectedObject != null)
        {
            if (CharacterPathFind.Instance)
            {
                CharacterPathFind.Instance.NavTarget = selectedObject;
                //Debug.LogError(selectedObject.name);
                CharacterPathFind.Instance.StartNavagation();
            }
        }
    }
    //bool IsPointerOverUIObject(PointerEventData eventData)
    //{
    //    // 检查点击位置是否在UI上
    //    //PointerEventData eventData = new PointerEventData(EventSystem.current);
    //    eventData.position = Input.mousePosition;

    //    EventSystem.current.RaycastAll(eventData, new List<RaycastResult>());
    //    return false;
    //    //Debug.LogError((eventData.pointerCurrentRaycast.gameObject.name).ToString());
    //    //return eventData.pointerCurrentRaycast.gameObject != null;
    //}
}
