using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMoveDir
{
    None = 0,
    Up,
    Down,

    Right,
    Left,
}


public class EnemyManager : CharacterManager
{
    private List<EMoveDir> m_MovePriorities = new List<EMoveDir>();
    private void Init_MovePriority()
    {
        m_MovePriorities.Clear();
    }

    private Vector3? Get_MovePathPoint(Vector3 dir, Vector3 currentPoint)
    {
        Vector3 resultDir = Convert_WorldToCellDir(dir);
        Vector3 resultPoint = TileMapManager.Instance.ConvertToCellCenterPose(currentPoint + resultDir);

        if (IsMoveAble(resultPoint))
        {
            Change_CharacterDir(resultDir);
            return resultPoint;
        }

        Init_MovePriority();

        if (Mathf.Abs(resultDir.x) > Mathf.Abs(resultDir.y))
        {
            m_MovePriorities.Add((resultDir.x > 0 ? EMoveDir.Right : EMoveDir.Left));
            m_MovePriorities.Add((resultDir.y > 0 ? EMoveDir.Up : EMoveDir.Down));
            m_MovePriorities.Add((!(resultDir.y > 0) ? EMoveDir.Up : EMoveDir.Down));
            m_MovePriorities.Add((!(resultDir.x > 0) ? EMoveDir.Right : EMoveDir.Left));
        }
        else
        {
            m_MovePriorities.Add((resultDir.y > 0 ? EMoveDir.Up : EMoveDir.Down));
            m_MovePriorities.Add((resultDir.x > 0 ? EMoveDir.Right : EMoveDir.Left));
            m_MovePriorities.Add((!(resultDir.x > 0) ? EMoveDir.Right : EMoveDir.Left));
            m_MovePriorities.Add((!(resultDir.y > 0) ? EMoveDir.Up : EMoveDir.Down));
        }

        for(int i=0; i < m_MovePriorities.Count; i++)
        {
            resultDir = Get_MoveDir(m_MovePriorities[i]);
            resultPoint = TileMapManager.Instance.ConvertToCellCenterPose(currentPoint + resultDir);

            if (IsMoveAble(resultPoint))
            {
                Change_CharacterDir(resultDir);
                return resultPoint;
            }
        }

        return null;
    }

    public override Vector3? Get_MovePoint(Vector3 dragDir)
    {
        Vector3 currentPoint = Character.transform.position;
        Vector3 dir = ( Target == null ?
            Get_RandomPoint() : 
            Get_TargetPoint(currentPoint) );

        Vector3? destPoint = Get_MovePathPoint(dir, currentPoint);
        return destPoint;
    }

    private Vector3 Get_TargetPoint(Vector3 currentPoint)
    {
        Vector3 dir = Target.position - currentPoint;
        return dir.normalized;
    }

    private Vector3 Get_MoveDir(EMoveDir dir)
    {
        Vector3 targetDir = Vector3.zero;
        switch (dir)
        {
            case EMoveDir.Up:
                targetDir = new Vector3(0, 1, 0);
                break;

            case EMoveDir.Down:
                targetDir = new Vector3(0, -1, 0);
                break;

            case EMoveDir.Left:
                targetDir = new Vector3(-1, 0, 0);
                break;

            case EMoveDir.Right:
                targetDir = new Vector3(1, 0, 0);
                break;

            default:
                break;
        }

        return targetDir;
    }

    private Vector3 Get_RandomPoint()
    {
        Vector3 randomPoint = Vector3.zero;
        EMoveDir dir = (EMoveDir)Random.Range(0, 8);

        switch (dir)
        {
            case EMoveDir.Up:
                randomPoint += new Vector3(0, 1, 0);
                break;

            case EMoveDir.Down:
                randomPoint += new Vector3(0, -1, 0);
                break;

            case EMoveDir.Left:
                randomPoint += new Vector3(-1, 0, 0);
                break;

            case EMoveDir.Right:
                randomPoint += new Vector3(1, 0, 0);
                break;

            default:
                break;
        }

        Debug.Log(dir.ToString()+"/ Point: "+randomPoint);

        return randomPoint;
    }
}
