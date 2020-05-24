using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Target;
    private Vector3 dest;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            dest = Get_MovePose();
        }
    }

    private Vector3 Get_MovePose()
    {
        Vector3 targetDir = Target.transform.position - this.transform.position;
        targetDir.Normalize();
        return TileMapManager.Instance.ConvertToCellCenterPose(this.transform.position + targetDir);
    }
}
