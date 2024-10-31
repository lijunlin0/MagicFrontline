using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndWindow : MonoBehaviour
{
    private bool mIsWin;
    private TMP_Text mText;
    public static EndWindow Create(bool isWin)
    {
        Canvas windowCanvas=GameObject.Find("WindowCanvas").GetComponent<Canvas>();
        GameObject windowPrefab = Resources.Load<GameObject>("UI/EndWindow");
        GameObject windowObject=GameObject.Instantiate(windowPrefab,windowCanvas.transform);
        
        EndWindow window= windowObject.AddComponent<EndWindow>();
        window.Init(isWin);
        return window;
    }

    private void Init(bool isWin)
    {
        AudioManager.GetCurrent().StopPlay();
        mIsWin=isWin;
        mText=transform.Find("Window/Text").GetComponent<TextMeshProUGUI>();
        mText.text=isWin?" 赢了!":"    输了...";
        GameObject buttonPrefab = isWin?Resources.Load<GameObject>("UI/Next"):Resources.Load<GameObject>("UI/Redo");
        GameObject buttonObject=GameObject.Instantiate(buttonPrefab,transform.Find("Window"));
        FightManager.GetCurrent().SetPause(true);
        Button exitButton=transform.Find("Window/Exit").GetComponent<Button>();
        exitButton.onClick.AddListener(()=>
        {
            ButtonOnClick(exitButton);
            FightManager.GetCurrent().SetPause(false);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.LoadScene("Main");
        });
        //赢了--下一关按钮 输了--重玩按钮
        Button button=buttonObject.GetComponent<Button>();
        button.onClick.AddListener(()=>
        {
            ButtonOnClick(button);
            FightManager.GetCurrent().SetPause(false);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            if(mIsWin)
            {
                SceneManager.LoadScene("Level"+(FightModel.GetCurrent().GetLevel()+1).ToString());
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } 
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