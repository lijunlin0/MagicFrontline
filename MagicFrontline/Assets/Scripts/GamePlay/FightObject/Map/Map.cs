using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public enum TileType
{
    Build,          //可建造
    Tower,          //塔
    Obstacle,       //障碍
}

public class Map : MonoBehaviour
{
    public const int TileSize = 128;
    private int mWidth;
    private int mHeight;    
    private Tilemap mBuildTileMap;
    private Tilemap mWayPointTileMap;
    private List<Vector3Int> mTurningPointList;
    private Tilemap mObstacleTileMap;
    private MyCollider mCollider;
    private int mWayPointCount;
    private List<Vector3> mBridgePointList;
    //某位置是否可建造
    private Dictionary<Vector3Int, TileType> mTileTypes;

    public static Map Create()
    {
        GameObject grid=GameObject.Find("Grid");
        Map map=grid.AddComponent<Map>();
        map.Init();
        return map;
    }
    public static void CreatePortal()
    {
        List<Vector3Int> turningPointList=FightModel.GetCurrent().GetMap().GetTurningPointList(); 
        //创建传送门
        Portal.Create(turningPointList[0],false);
        Portal.Create(turningPointList[turningPointList.Count-1],true);
    }

    public void Init()
    {
        mBuildTileMap=transform.Find("Build").GetComponent<Tilemap>();
        mWayPointTileMap=transform.Find("WayPoint").GetComponent<Tilemap>();
        mObstacleTileMap=transform.Find("Obstacle").GetComponent<Tilemap>();
        mCollider=null;
        InitBridge();
        mWidth=mBuildTileMap.cellBounds.size.x;
        mHeight=mBuildTileMap.cellBounds.size.y;
        InitTileType();
        InitWayPoint();
    }

    public void InitBridge()
    {
        Transform waypointsParent = GameObject.Find("Grid").transform.Find("WayPoint/Bridge");
        if(waypointsParent!=null)
        {
            mBridgePointList=new List<Vector3>();
            mCollider=new MyCollider(GameObject.Find("Grid").transform.Find("WayPoint").GetComponent<TilemapCollider2D>());
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                Transform waypoint = waypointsParent.GetChild(i);
                mBridgePointList.Add(waypoint.position);
            }
        }
    }

    public List<Vector3> GetBridgePointList(){return mBridgePointList;}

    public void InitWayPoint()
    {
         Scene currentScene = SceneManager.GetActiveScene();
        // 获取场景名称
        string sceneName = currentScene.name;
        mTurningPointList=LevelUtility.GetTurningPoints(FightModel.LevelNameToLevel(sceneName));
    }

    public void Update()
    {
        if(mCollider!=null)
        {
            mCollider.OnUpdate();
        }
        
    }

    public MyCollider GetCollider()
    {
        return mCollider;
    }

    public int GetWayPointCount(){return mWayPointCount;}

    public List<Vector3Int> GetTurningPointList()
    {
        return mTurningPointList;
    }

    public TileType GetTileType(Vector3Int position)
    {
        return mTileTypes[position];
    }

    public bool IsBuild(Vector3Int position)
    {
        return mTileTypes[position]==TileType.Build;
    }
    public bool IsTower(Vector3Int position)
    {
        return mTileTypes[position]==TileType.Tower;
    }
    public bool IsObstacle(Vector3Int position)
    {
        return mTileTypes[position]==TileType.Obstacle;
    }

    public void AddTower(Vector3Int position)
    {
       mTileTypes[position]=TileType.Tower;
    }
    public void RemoveTower(Vector3Int position)
    {
       mTileTypes[position]=TileType.Build;
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
                    mWayPointCount++;
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