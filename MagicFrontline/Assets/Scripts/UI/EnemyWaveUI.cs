using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveUI : MonoBehaviour
{
    private int mMaxWave;
    private int mCurrentWaveCount;
    private TMP_Text mText;
    public static EnemyWaveUI Create()
    {
        GameObject UIControl=GameObject.Find("UIControl(Clone)").gameObject;
        GameObject prefab = Resources.Load<GameObject>("UI/EnemyWaveUI");
        GameObject uiObject=GameObject.Instantiate(prefab,UIControl.transform);
        EnemyWaveUI ui= uiObject.AddComponent<EnemyWaveUI>();
        ui.Init();
        return ui;
    }

    private void Init()
    {
        mText=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mCurrentWaveCount=0;
        mMaxWave=FightModel.GetCurrent().GetEnemyMaxWave();
        mText.text=mCurrentWaveCount.ToString()+"/"+mMaxWave.ToString()+"波";
    }

    public void OnEnemyWavesChanged()
    {
        mCurrentWaveCount++;
        mText.text=mCurrentWaveCount.ToString()+"/"+mMaxWave.ToString()+"波";
    }
}