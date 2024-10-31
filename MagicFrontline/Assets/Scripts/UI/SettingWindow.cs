using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    public static SettingWindow Create()
    {
        Canvas windowCanvas=GameObject.Find("WindowCanvas").GetComponent<Canvas>();
        GameObject prefab = Resources.Load<GameObject>("UI/SettingWindow");
        GameObject uiObject=GameObject.Instantiate(prefab,windowCanvas.transform);
        SettingWindow ui= uiObject.AddComponent<SettingWindow>();
        ui.Init();
        return ui;
    }

    private void Init()
    {
        AudioManager.GetCurrent().IsPauseFightMusicPlay(true);
        FightManager.GetCurrent().SetPause(true);
        Button exitButton=transform.Find("Window/Exit").GetComponent<Button>();
        AudioSource exitSource=exitButton.GetComponent<AudioSource>();
        exitButton.onClick.AddListener(()=>
        {
            AudioManager.GetCurrent().StopPlay();
            exitButton.enabled=false;
            ButtonOnClick(exitButton);
            FightManager.GetCurrent().SetPause(false);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.LoadScene("Main");
        });
        Button redoButton=transform.Find("Window/Redo").GetComponent<Button>();
        AudioSource redoSource=redoButton.GetComponent<AudioSource>();
        redoButton.onClick.AddListener(()=>
        {
            redoButton.enabled=false;
            ButtonOnClick(exitButton);
            FightManager.GetCurrent().SetPause(false);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        });
        Button continueButton=transform.Find("Window/Continue").GetComponent<Button>();
        AudioSource continueSource=continueButton.GetComponent<AudioSource>();
        continueButton.onClick.AddListener(()=>
        {
            AudioManager.GetCurrent().IsPauseFightMusicPlay(false);
            continueButton.enabled=false;
            ButtonOnClick(exitButton);
            FightManager.GetCurrent().SetPause(false);
        });
    }

    public void OnSceneUnloaded(Scene scene)
    {
        DOTween.KillAll();
    }

    private void ButtonOnClick(Button button)
    {
        AudioManager.GetCurrent().PlayUIButtonkSound();
        Destroy(gameObject);
    }
}