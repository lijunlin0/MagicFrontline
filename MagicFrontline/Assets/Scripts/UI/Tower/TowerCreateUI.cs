using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class TowerCreateUI : MonoBehaviour
{
    private Vector3Int mCreatePosition;
    private Callback mSelectTowerCallback;

    public static TowerCreateUI Create(Vector3Int createPosition,Callback selectTowerCallback)
    {
        Canvas canvas=GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject prefab=Resources.Load<GameObject>("UI/TowerCreateUI");
        GameObject towerCreateUIObject=Instantiate(prefab,canvas.transform);
        TowerCreateUI towerCreateUI=towerCreateUIObject.AddComponent<TowerCreateUI>();
        towerCreateUI.Init(createPosition,selectTowerCallback);
        return towerCreateUI;
    }

    private void Init(Vector3Int createPosition,Callback selectTowerCallback)
    {
        mCreatePosition=createPosition;
        mSelectTowerCallback=selectTowerCallback;
        transform.position=FightModel.GetCurrent().GetMap().LogicToWorldPosition(mCreatePosition);
        ButtonInit("Bow");
        ButtonInit("FrozenBow");
        ButtonInit("ExplosiveSlingshot");
        ButtonInit("LightwaveStone");
        ButtonInit("Jet");
        ButtonInit("ThunderStone");
    }

    private void ButtonInit(string towerName)
    {
        Button button=transform.Find(towerName).GetComponent<Button>();
        button.onClick.AddListener(()=>
        {
            switch(towerName)
            {
                case "Bow" : FightModel.GetCurrent().AddTower(Bow.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造弓箭");break;
                case "FrozenBow" :  FightModel.GetCurrent().AddTower(FrozenBow.Create(1,mCreatePosition),mCreatePosition);break;
                case "ExplosiveSlingshot" :  FightModel.GetCurrent().AddTower(ExplosiveSlingshot.Create(1,mCreatePosition),mCreatePosition);break; 
                case "LightwaveStone" :  FightModel.GetCurrent().AddTower(LightwaveStone.Create(1,mCreatePosition),mCreatePosition);break;
                case "Jet" :  FightModel.GetCurrent().AddTower(Jet.Create(1,mCreatePosition),mCreatePosition);break;
                case "ThunderStone" :  FightModel.GetCurrent().AddTower(ThunderStone.Create(1,mCreatePosition),mCreatePosition);break;
                default : break;
            }
            if(gameObject!=null)
            {
                Destroy(gameObject);
            }
            mSelectTowerCallback();
        });
    }

}