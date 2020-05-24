using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharcterController : MonoBehaviour
{
    private bool isMoveOn
    {
        get; set;
    }

    public GameObject m_Charcter;
    public GameObject Charcter
    {
        get { return m_Charcter; }
    }

    private Animator m_CharcterAnimator;

    Vector3? startPoint_touch = null;
    Vector3? endPoint_touch = null;
    Vector3? dest = null;

    private void Awake()
    {
        m_CharcterAnimator = Charcter.GetComponent<Animator>();
        m_CharcterAnimator.SetInteger("State", 0);
    }

    private void Start()
    {
        isMoveOn = true;
    }

    private bool Check_DirWall(Vector3 dest)
    {
        Vector3 playerPose = m_Charcter.transform.position;
        Vector3 dir = dest - playerPose;
        RaycastHit2D hit = Physics2D.Raycast(playerPose, dir.normalized, dir.magnitude);
        if(hit)
        {
            Debug.Log(hit.collider.name);
            return hit.collider.name.Equals("wall");
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint_touch = Input.mousePosition;
            if (IsPointerOverUIObject((Vector2)startPoint_touch))
            {
                startPoint_touch = null;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endPoint_touch = Input.mousePosition;
            if (startPoint_touch != null)
            {
                Vector3 startPoint = (Vector3)startPoint_touch;
                Vector3 endPoint = (Vector3)endPoint_touch;
                Vector3 dragDir = endPoint - startPoint;
                float absValue = Mathf.Abs(dragDir.magnitude);

                if (absValue < 10)
                {
                    Debug.Log("기준 민감도 미만의 값");
                    return;
                }

                dragDir.Normalize();
                dest = Charcter.transform.position + (dragDir);
                dest = TileMapManager.Instance.ConvertToCellCenterPose((Vector3)dest);

                if (Check_DirWall((Vector3)dest))
                {
                    dest = null;
                    return;
                }
            }

            startPoint_touch = null;
        }

        if(dest != null)
        {
            Vector3 realDest = (Vector3)dest;
            CharcterMove(realDest);

            float distance = Vector3.Distance(Charcter.transform.position, realDest);
            if(distance < 0.01f)
            {
                m_CharcterAnimator.SetInteger("State", 0);
                dest = null;
            }
            else
            {
                m_CharcterAnimator.SetInteger("State", 2);
            }
        }
    }


    public void Change_CharcterDir()
    {
        Charcter.transform.localScale = (
            Charcter.transform.localScale.x.Equals(-1) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1));
    }

    private void CharcterMove(Vector3 dest)
    {
        Charcter.transform.position = Vector3.MoveTowards(Charcter.transform.position, dest, Time.deltaTime * 2f);
    }

    private bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
