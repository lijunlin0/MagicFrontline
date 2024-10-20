using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveUI : MonoBehaviour
{
    private int mMaxWaveCount;
    private int mCurrentWaveCount;
    private TMP_Text mText;
    public static EnemyWaveUI Create(int maxWaveCount)
    {
        GameObject EnemyWaveUI = GameObject.Find("EnemyWaveUI");
        EnemyWaveUI ui= EnemyWaveUI.AddComponent<EnemyWaveUI>();
        ui.Init(maxWaveCount);
        return ui;
    }

    private void Init(int maxWaveCount)
    {
        mText=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mCurrentWaveCount=0;
        mMaxWaveCount=maxWaveCount;
        mText.text=mCurrentWaveCount.ToString()+"/"+mMaxWaveCount.ToString()+"波";
    }

    public void OnEnemyWavesChanged()
    {
        mCurrentWaveCount++;
        mText.text=mCurrentWaveCount.ToString()+"/"+mMaxWaveCount.ToString()+"波";
    }
}