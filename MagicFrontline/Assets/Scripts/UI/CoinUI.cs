using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    private int mCurrentCoin;
    private TMP_Text mText;
    public static CoinUI Create()
    {
        GameObject UIControl=GameObject.Find("UIControl(Clone)").gameObject;
        GameObject prefab = Resources.Load<GameObject>("UI/CoinUI");
        GameObject uiObject=GameObject.Instantiate(prefab,UIControl.transform);
        CoinUI ui= uiObject.AddComponent<CoinUI>();
        ui.Init();
        return ui;
    }

    private void Init()
    {
        mText=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mCurrentCoin=FightModel.GetCurrent().GetCoins();
        mText.text=mCurrentCoin.ToString();
    }

    public void OnCoinsChanged()
    {
        mCurrentCoin=FightModel.GetCurrent().GetCoins();
        mText.text=mCurrentCoin.ToString();
    }
}