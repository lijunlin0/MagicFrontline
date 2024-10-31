using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public static SettingButton Create()
    {
        Canvas UICanvas=GameObject.Find("UICanvas").GetComponent<Canvas>();
        GameObject prefab = Resources.Load<GameObject>("UI/SettingButton");
        GameObject uiObject=GameObject.Instantiate(prefab,UICanvas.transform);
        SettingButton ui= uiObject.AddComponent<SettingButton>();
        ui.Init();
        return ui;
    }

    private void Init()
    {
        Button button=GetComponent<Button>();
        button.onClick.AddListener(()=>
        {
            SettingWindow.Create();
            AudioManager.GetCurrent().PlaySettingButtonkSound();
        });
    }
}