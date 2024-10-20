using System;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class TowerCreateUI : MonoBehaviour
{
    private Vector3Int mCreatePosition;
    private Callback mSelectTowerCallback;
    private TMP_Text mBowPriceText;
    private TMP_Text mFrozenBowPriceText;
    private TMP_Text mExplosiveSlingshotPriceText;
    private TMP_Text mLightwaveStonePriceText;
    private TMP_Text mJetPriceText;
    private TMP_Text mThunderStonePriceText;

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
        TowerPriceTextInit();
        ButtonInit("Bow");
        ButtonInit("FrozenBow");
        ButtonInit("ExplosiveSlingshot");
        ButtonInit("LightwaveStone");
        ButtonInit("Jet");
        ButtonInit("ThunderStone");
    }

    public void TowerPriceTextInit()
    {
        mBowPriceText=transform.Find("Bow/Price").GetComponent<TextMeshProUGUI>();
        mBowPriceText.text=TowerUtility.GetTowerCreatePrice("Bow",1).ToString();

        mFrozenBowPriceText=transform.Find("FrozenBow/Price").GetComponent<TextMeshProUGUI>();
        mFrozenBowPriceText.text=TowerUtility.GetTowerCreatePrice("FrozenBow",1).ToString();

        mExplosiveSlingshotPriceText=transform.Find("ExplosiveSlingshot/Price").GetComponent<TextMeshProUGUI>();
        mExplosiveSlingshotPriceText.text=TowerUtility.GetTowerCreatePrice("ExplosiveSlingshot",1).ToString();

        mLightwaveStonePriceText=transform.Find("LightwaveStone/Price").GetComponent<TextMeshProUGUI>();
        mLightwaveStonePriceText.text=TowerUtility.GetTowerCreatePrice("LightwaveStone",1).ToString();

        mJetPriceText=transform.Find("Jet/Price").GetComponent<TextMeshProUGUI>();
        mJetPriceText.text=TowerUtility.GetTowerCreatePrice("Jet",1).ToString();

        mThunderStonePriceText=transform.Find("ThunderStone/Price").GetComponent<TextMeshProUGUI>();
        mThunderStonePriceText.text=TowerUtility.GetTowerCreatePrice("ThunderStone",1).ToString();
    }

    private void ButtonInit(string towerName)
    {
        Button button=transform.Find(towerName).GetComponent<Button>();
        int coinsCount=FightModel.GetCurrent().GetCoins();
        button.onClick.AddListener(()=>
        {
            switch(towerName)
            {
                case "Bow" :
                int price=TowerUtility.GetTowerCreatePrice(towerName,1);
                if(price<=coinsCount)
                {
                    FightModel.GetCurrent().AddTower(Bow.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造弓箭");
                    FightModel.GetCurrent().RemoveCoins(price);  
                }
                break;

                case "FrozenBow" :
                price=TowerUtility.GetTowerCreatePrice(towerName,1);
                if(price<=coinsCount)
                {
                    FightModel.GetCurrent().AddTower(FrozenBow.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造冰冻弓箭");
                    FightModel.GetCurrent().RemoveCoins(price);  
                }
                break;

                case "ExplosiveSlingshot" :
                price=TowerUtility.GetTowerCreatePrice(towerName,1);
                if(price<=coinsCount)
                {
                    FightModel.GetCurrent().AddTower(ExplosiveSlingshot.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造爆炸弹弓");
                    FightModel.GetCurrent().RemoveCoins(price);  
                }
                break;
                case "LightwaveStone" :
                price=TowerUtility.GetTowerCreatePrice(towerName,1);
                if(price<=coinsCount)
                {
                    FightModel.GetCurrent().AddTower(LightwaveStone.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造光波宝石");
                    FightModel.GetCurrent().RemoveCoins(price);  
                }
                break;

                case "Jet" :
                price=TowerUtility.GetTowerCreatePrice(towerName,1);
                if(price<=coinsCount)
                {
                    FightModel.GetCurrent().AddTower(Jet.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造喷石嘴");
                    FightModel.GetCurrent().RemoveCoins(price);  
                }
                break;

                case "ThunderStone" :
                price=TowerUtility.GetTowerCreatePrice(towerName,1);
                if(price<=coinsCount)
                {
                    FightModel.GetCurrent().AddTower(ThunderStone.Create(1,mCreatePosition),mCreatePosition);Debug.Log("建造电击宝石");
                    FightModel.GetCurrent().RemoveCoins(price);  
                }
                break;
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