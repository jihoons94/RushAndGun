using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager Instance;

    public Tilemap blockTileMap;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    public Grid m_MapGrid;

    public bool IsMoveAbleTile(Vector3 worldPose)
    {
        Vector3Int cellPose = m_MapGrid.WorldToCell(worldPose);
        TileBase tile = blockTileMap.GetTile(cellPose);
        return blockTileMap.ContainsTile(tile) ==false;
    }

    public Vector3 ConvertToCellCenterPose(Vector3 worldPose)
    {
        Vector3Int cellPose = m_MapGrid.WorldToCell(worldPose);
        Vector3 centerPose = m_MapGrid.GetCellCenterWorld(cellPose);
        return centerPose;
    }
}
