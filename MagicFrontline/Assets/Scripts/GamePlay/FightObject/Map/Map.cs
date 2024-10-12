using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Build,          //可建造
    Tower,          //塔
    Obstacle,       //障碍
}

public class Map : MonoBehaviour
{
    public const int TileSize = 256;
    private int mWidth;
    private int mHeight;    
    private Tilemap mBuildTileMap;
    private Tilemap mWayPointTileMap;
    private List<Vector3Int> mTurningPointList;
    private Tilemap mObstacleTileMap;
    private int mWayPointCount;
    //某位置是否可建造
    private Dictionary<Vector3Int, TileType> mTileTypes;

    public static Map Create()
    {
        GameObject grid=GameObject.Find("Grid");
        Map map=grid.AddComponent<Map>();
        map.Init();
        return map;
    }

    public void Init()
    {
        mBuildTileMap=transform.Find("Build").GetComponent<Tilemap>();
        mWayPointTileMap=transform.Find("WayPoint").GetComponent<Tilemap>();
        mObstacleTileMap=transform.Find("Obstacle").GetComponent<Tilemap>();
        mWidth=mBuildTileMap.cellBounds.size.x;
        mHeight=mBuildTileMap.cellBounds.size.y;
        InitTileType();
        InitWayPoint();
        
    }

    public void InitWayPoint()
    {
        mTurningPointList=new List<Vector3Int>();
        mTurningPointList.Add(new Vector3Int(-5,2,0));
        mTurningPointList.Add(new Vector3Int(-5,-2,0));
        mTurningPointList.Add(new Vector3Int(4,-2,0));
        mTurningPointList.Add(new Vector3Int(4,2,0));
        mTurningPointList.Add(new Vector3Int(-5,2,0));
        mWayPointCount=18;
    }

    public int GetWayPointCount(){return mWayPointCount;}

    public List<Vector3Int> GeturningPointList()
    {
        return mTurningPointList;
    }

    public TileType GetTileType(Vector3 position)
    {
        Vector3Int LogicPosition=WorldToLogicPosition(position);
        return mTileTypes[LogicPosition];
    }

    public bool IsBuild(Vector3 position)
    {
        Vector3Int LogicPosition=WorldToLogicPosition(position);
        return mTileTypes[LogicPosition]==TileType.Build;
    }
    public bool IsTower(Vector3 position)
    {
        Vector3Int LogicPosition=WorldToLogicPosition(position);
        return mTileTypes[LogicPosition]==TileType.Tower;
    }
    public bool IsObstacle(Vector3 position)
    {
        Vector3Int LogicPosition=WorldToLogicPosition(position);
        return mTileTypes[LogicPosition]==TileType.Obstacle;
    }

    public void AddTower(Vector3 position)
    {
       Vector3Int logicPosition = WorldToLogicPosition(position);
       mTileTypes[logicPosition]=TileType.Tower;
    }
    public void RemoveTower(Vector3 position)
    {
        Vector3Int logicPosition = WorldToLogicPosition(position);
       mTileTypes[logicPosition]=TileType.Build;
    }

    public Vector3 LogicToWorldPosition(Vector3Int LogicPosition)
    {
        return mBuildTileMap.CellToWorld(LogicPosition)+new Vector3(TileSize/2,TileSize/2,0);
    }

    public Vector3Int WorldToLogicPosition(Vector3 worldPosition)
    {
        return mBuildTileMap.WorldToCell(worldPosition);
    }

    public int LogicPositionToInt(Vector3Int logicPosition)
    {
        return logicPosition.y*mWidth+logicPosition.x;
    }
    public Vector3Int IntToLogicPosition(int value)
    {
        int x = value % mWidth;
        int y = value / mWidth;
        return new Vector3Int(x, y, 0);
    }

    //转换为标准坐标
    public Vector3 GetStandardPosition(Vector3 position)
    {
        return LogicToWorldPosition(WorldToLogicPosition(position));
    }

    public void InitTileType()
    {
        mTileTypes=new Dictionary<Vector3Int, TileType>();
        BoundsInt bounds=mBuildTileMap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition=new Vector3Int(x,y,0);
                if(mBuildTileMap.HasTile(cellPosition))
                {
                    mTileTypes[cellPosition]=TileType.Build;
                }
            }
        }

        bounds=mWayPointTileMap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition=new Vector3Int(x,y,0);
                if(mWayPointTileMap.HasTile(cellPosition))
                {
                    mTileTypes[cellPosition]=TileType.Obstacle;
                }
            }
        }
        
        bounds=mObstacleTileMap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition=new Vector3Int(x,y,0);
                if(mObstacleTileMap.HasTile(cellPosition))
                {
                    mTileTypes[cellPosition]=TileType.Obstacle;
                }
            }
        }

    }


}