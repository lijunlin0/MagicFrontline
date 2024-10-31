using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private int mCurrentHealth;
    private TMP_Text mText;
    public static HealthUI Create()
    {
        GameObject UIControl=GameObject.Find("UIControl(Clone)").gameObject;
        GameObject prefab = Resources.Load<GameObject>("UI/HealthUI");
        GameObject uiObject=GameObject.Instantiate(prefab,UIControl.transform);
        HealthUI ui= uiObject.AddComponent<HealthUI>();
        ui.Init();
        return ui;
    }

    private void Init()
    {
        mText=transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mCurrentHealth=FightModel.GetCurrent().GetHealth();
        mText.text=mCurrentHealth.ToString();
    }

    public void OnHealthsChanged()
    {
        mCurrentHealth=FightModel.GetCurrent().GetHealth();
        mText.text=mCurrentHealth.ToString();
    }
}