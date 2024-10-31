using System.IO;
using UnityEngine;
using UnityEngine.EventSystems; // 引入 EventSystem

public class TowerCreateManager
{
    private Map mMap;
    private TowerCreateUI mTowerCreateUI;
    private TowerSelectUI mTowerSelectUI;
    private bool mIsOpenTowerCreateUI = false;
    private bool mIsOpenTowerSelectUI = false;
    public TowerCreateManager()
    {
        mMap = FightModel.GetCurrent().GetMap();
    }

    public void OnUpdate()
    {
        // 检测触摸输入
        if (Input.touchCount > 0&&Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);

            // 如果点击了 UI，跳过其他逻辑
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(touch.position);
            mousePosition.z = 0; // 确保 z 坐标为 0
            if (mIsOpenTowerCreateUI)
            {
                GameObject.Destroy(mTowerCreateUI.gameObject);
                mIsOpenTowerCreateUI = false;
                return;
            }
            if (mIsOpenTowerSelectUI)
            {
                GameObject.Destroy(mTowerSelectUI.gameObject);
                mIsOpenTowerSelectUI = false;
                return;
            }
            Vector3Int logicPosition = mMap.WorldToLogicPosition(mousePosition);
            // 如果点击在建造区域，创建塔建造 UI
            if (mMap.IsBuild(logicPosition))
            {
                mIsOpenTowerCreateUI = true;
                mTowerCreateUI = TowerCreateUI.Create(logicPosition, () => { mIsOpenTowerCreateUI = false; });
            }
            else if (mMap.IsTower(logicPosition))
            {
                mTowerSelectUI = TowerSelectUI.Create(logicPosition, () => { mIsOpenTowerSelectUI = false; });
                mIsOpenTowerSelectUI = true;
            }
        }
    }
}
