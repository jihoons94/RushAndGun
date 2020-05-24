using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : CharacterManager
{
    public Transform aim;

    protected override void Awake()
    {
        base.Awake();

        aim.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        Transform target = Get_AimTarget();
        if(target != null)
        {
            aim.transform.position = target.transform.position;
            aim.gameObject.SetActive(true);
        }
        else
        {
            aim.gameObject.SetActive(false);
        }
    }

    public override Vector3? Get_MovePoint(Vector3 dragDir)
    {
        Vector3 currentPoint = Character.transform.position;
        Vector3? destPoint = null;

        if (dragDir.Equals(Vector3.zero)== false)
        {
            float xValue = 0;
            if (Mathf.Abs(dragDir.x) > 80)
            {
                xValue = Mathf.Clamp(dragDir.x, -1, 1);
            }

            float yValue = 0;
            if (Mathf.Abs(dragDir.y) > 80)
            {
                yValue = Mathf.Clamp(dragDir.y, -1, 1);
            }

            dragDir = Convert_WorldToCellDir(new Vector3(xValue, yValue, 0));

            Vector3 tileCellPoint = TileMapManager.Instance.ConvertToCellCenterPose(currentPoint + dragDir);

            if (IsMoveAble(tileCellPoint))
            {
                destPoint = tileCellPoint;
            }
        }

        return destPoint;
    }

    private Transform Get_AimTarget()
    {
        float dir_X = Get_CurrentCharacterDir();
        Vector3 dir = new Vector3(dir_X, 0, 0);

        float minValue = Mathf.Infinity;
        var characters = GameManager.Instance.Characters;

        Transform target_Character = null;
        Vector3 target_Dir = Vector3.zero;

        foreach (var enemy in characters)
        {
            if(enemy.Equals(characters[GameManager.Instance.PlayerCharacter_Index]))
            {
                continue;
            }

            Vector3 targetDir = (enemy.transform.position - Character.transform.position);
            float dotValue = Vector3.Dot(dir, targetDir.normalized);
            float sqrMagnitude = targetDir.sqrMagnitude;

            if (dotValue > 0 && minValue > sqrMagnitude)
            {
                target_Character = enemy.transform;
                target_Dir = targetDir;
                minValue = sqrMagnitude;
            }
        }

        if (target_Dir.magnitude > 5f)
        {
            target_Character = null;
        }

        return target_Character;
    }
}
