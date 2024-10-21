using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    private int mCurrentCoinCount;
    private TMP_Text mText;
    private AudioSource mAudioSource;
    public static CoinUI Create()
    {
        GameObject coinUI = GameObject.Find("CoinUI");
        CoinUI ui= coinUI.AddComponent<CoinUI>();
        ui.Init();
        return ui;
    }

    private void Init()
    {
        mAudioSource=GetComponent<AudioSource>();
        mText=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mCurrentCoinCount=FightModel.GetCurrent().GetCoins();
        mText.text=mCurrentCoinCount.ToString();
    }

    public void OnCoinsChanged(int changePoints)
    {
        mText.text=(mCurrentCoinCount+=changePoints).ToString();
    }
    public void PlayCSound()
    {
        mAudioSource.Play();
    }
}