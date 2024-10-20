using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectUI : MonoBehaviour
{
    protected const int ShootRangeSize=100;
    private Vector3Int mCreatePosition;
    private Callback mSelectTowerCallback;

    public static TowerSelectUI Create(Vector3Int createPosition,Callback selectTowerCallback)
    {
        Canvas canvas=GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject prefab=Resources.Load<GameObject>("UI/TowerSelectUI");
        GameObject towerSelectUIObject=Instantiate(prefab,canvas.transform);
        TowerSelectUI towerSelectUI=towerSelectUIObject.AddComponent<TowerSelectUI>();
        towerSelectUI.Init(createPosition,selectTowerCallback);
        return towerSelectUI;
    }

    private void Init(Vector3Int createPosition,Callback selectTowerCallback)
    {
        mCreatePosition=createPosition;
        mSelectTowerCallback=selectTowerCallback;
        transform.position=FightModel.GetCurrent().GetMap().LogicToWorldPosition(mCreatePosition);
        FightModel fightModel=FightModel.GetCurrent();
        //拆除按钮
        Button destroyButton=transform.Find("Destroy").GetComponent<Button>();
        destroyButton.onClick.AddListener(()=>
        {
            fightModel.RemoveTower(createPosition);
            if(gameObject!=null)
            {
                Destroy(gameObject);
            }
            mSelectTowerCallback();
        });
        //攻击范围显示
        Tower tower=fightModel.GetTower(createPosition);
        int shootRange=tower.GetShootRange();
        RectTransform rectTransform=transform.Find("ShootRange").GetComponent<RectTransform>();
        rectTransform.sizeDelta=new Vector2(shootRange*2,shootRange*2);
        int level=fightModel.GetTower(createPosition).GetLevel();
        if(level>=Tower.MaxLevel)
        {
            GameObject levelUpObject=transform.Find("LevelUp").gameObject;
            levelUpObject.SetActive(false);
            return;
        }
        
        

        //升级按钮
        Button levelUpButton=transform.Find("LevelUp").GetComponent<Button>();
        levelUpButton.onClick.AddListener(()=>
        {
            
            fightModel.RemoveTower(createPosition);
            fightModel.AddTower(CreateLevelUpTower(tower,tower.GetLevel()+1,tower.transform.rotation),mCreatePosition);
            if(gameObject!=null)
            {
                Destroy(gameObject);
            }
            mSelectTowerCallback();
        });
    }

    private Tower CreateLevelUpTower(Tower tower,int level,Quaternion rotation)
    {
        if (tower is Bow)
        {
            return Bow.Create(level,mCreatePosition,rotation);
        }
        else if (tower is FrozenBow)
        {
            return FrozenBow.Create(level,mCreatePosition,rotation);
        }
        else if (tower is Jet)
        {
            return Jet.Create(level,mCreatePosition,rotation);
        }
        else if (tower is ExplosiveSlingshot)
        {
            return ExplosiveSlingshot.Create(level,mCreatePosition,rotation);
        }
        else if (tower is LightwaveStone)
        {
            return LightwaveStone.Create(level,mCreatePosition,rotation);
        }
        else
        {
            return ThunderStone.Create(level,mCreatePosition,rotation);
        }
    }

}