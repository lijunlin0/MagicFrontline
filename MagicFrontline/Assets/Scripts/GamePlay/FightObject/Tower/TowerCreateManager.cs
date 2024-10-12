using System.IO;
using UnityEngine;
using UnityEngine.EventSystems; // 引入 EventSystem

public class TowerCreateManager
{
    private Map mMap;
    private TowerCreateUI mTowerCreateUI;
    private TowerSelectUI mTowerSelectUI;
    private bool mIsOpenTowerCreateUI = false;
    private bool mIsOpenTowerSelectUI=false;

    public TowerCreateManager()
    {
        mMap = FightModel.GetCurrent().GetMap();
    }

    public void OnUpdate()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 如果点击了 UI，跳过其他逻辑
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mIsOpenTowerCreateUI)
            {
                Debug.Log("删除建造UI");
                GameObject.Destroy(mTowerCreateUI.gameObject);
                mIsOpenTowerCreateUI = false;
                return;
            }
            if (mIsOpenTowerSelectUI)
            {
                Debug.Log("删除选择UI");
                GameObject.Destroy(mTowerSelectUI.gameObject);
                mIsOpenTowerSelectUI = false;
                return;
            }
            Vector3Int logicPosition=FightModel.GetCurrent().GetMap().WorldToLogicPosition(mousePosition);
            // 如果点击在建造区域，创建塔建造 UI
            if (mMap.IsBuild(mousePosition))
            {
                Debug.Log("创建建造UI");
                mIsOpenTowerCreateUI = true;
                mTowerCreateUI = TowerCreateUI.Create(logicPosition,()=>{mIsOpenTowerCreateUI=false;});
            }
            else if (mMap.IsTower(mousePosition))
            {
                Debug.Log("创建选择UI");
                mTowerSelectUI=TowerSelectUI.Create(logicPosition,()=>{mIsOpenTowerSelectUI=false;});
                mIsOpenTowerSelectUI=true;
            }
        }
    }

    private void TowerLevelUpUI()
    {
        // 塔升级的逻辑
    }

}
