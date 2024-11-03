using DG.Tweening;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    FightManager mFightManager;

    public void Awake()
    {
       InitScene();
       mFightManager=new FightManager();
    }
    public FightScene GetCurrent(){return this;}
    public void Update()
    {
        mFightManager.OnUpdate();
    }
    public void Start()
    {
    }
    public void InitScene()
    {
        SettingButton.Create();
        GameObject prefab=Resources.Load<GameObject>("UI/UIControl");
        GameObject.Instantiate(prefab,GameObject.Find("UICanvas").transform);
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera.orthographicSize = Utility.WindowHeight/2;
        Canvas[] canvasList=FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in canvasList)
        {
            RectTransform rectTransform=canvas.GetComponent<RectTransform>();
            rectTransform.sizeDelta=new Vector2(Utility.WindowWidth,Utility.WindowHeight);
        }

    }
}