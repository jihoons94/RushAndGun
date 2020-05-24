using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CharacterManager : MonoBehaviour
{
    public CharacterInfo Info;

    public Transform Target;

    protected Transform Character
    {
        get
        {
            return this.transform;
        }
    }

    protected static bool isMainStreamOn = false;
    public Vector3? DestPoint;


    protected virtual void Awake()
    {
        DestPoint = null;
        isMainStreamOn = true;
    }

    protected virtual void Update()
    {
        if (!isMainStreamOn)
        {
            return;
        }

        if (DestPoint != null)
        {
            Character.position = Vector3.MoveTowards(Character.position, (Vector3)DestPoint, Time.deltaTime * 2f);
            if (DestPoint.Equals(Character.position))
            {
                DestPoint = null;
            }
        }
    }

    public virtual void Change_CharacterDirByToggle()
    {
        Character.localScale = (
            Character.localScale.x.Equals(-1) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1));
    }

    public virtual void Change_CharacterDir(Vector3 dir)
    {
        if(dir.x > 0)
        {
            Character.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            Character.localScale = new Vector3(-1, 1, 1);
        }
    }

    public virtual float Get_CurrentCharacterDir()
    {
        return Character.localScale.x;
    }

    public virtual void Set_AnimationStatus()
    {

    }

    public virtual void Get_AnimationStatue()
    {

    }

    public virtual void Start_MainStream()
    {
        isMainStreamOn = true;
    }

    public virtual void Stop_MainStream()
    {
        isMainStreamOn = false;
    }

    public virtual bool IsMoveAble(Vector3 destPosition)
    {
        bool isOtherNotExist = (GameManager.Instance.CharacterPoint_d.ContainsValue(destPosition) == false);
        bool isNotWall = TileMapManager.Instance.IsMoveAbleTile(destPosition);

        return isOtherNotExist && isNotWall;
    }

    public virtual Vector3? Get_MovePoint(Vector3 dragDir)
    {
        return null;
    }


    public virtual Vector3 Convert_WorldToCellDir(Vector3 dir)
    {
        float x = 0;
        float y = 0;

        x =  (dir.x > 0 ? Mathf.CeilToInt(dir.x) : Mathf.FloorToInt(dir.x));
        y = (dir.y > 0 ? Mathf.CeilToInt(dir.y) : Mathf.FloorToInt(dir.y));

        return new Vector3(x,y,dir.z);
    }

    protected bool IsPointerOverUIObject(Vector2 touchPos)
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
