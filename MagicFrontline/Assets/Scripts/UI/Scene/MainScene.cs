using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    private const int CanvasWidth=6000;
    private const int CanvasHeight=Utility.WindowHeight;
    private const int ImageWidth=640;
    private Button[] mButtons;

    public void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        canvas.transform.GetComponent<RectTransform>().sizeDelta=new Vector3(CanvasWidth,CanvasHeight);
        HorizontalLayoutGroup group=canvas.transform.Find("Scroll View/Viewport/Content").GetComponent<HorizontalLayoutGroup>();
        int paddingValue=CanvasWidth/2-ImageWidth/2;
        Debug.Log("值:"+paddingValue);
        Debug.Log("屏幕宽度:"+Utility.WindowWidth);
        group.padding=new RectOffset(paddingValue,paddingValue,0,0);
        GameObject mContent = canvas.transform.Find("Scroll View/Viewport/Content").gameObject;
        mButtons = mContent.GetComponentsInChildren<Button>();
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera.orthographicSize = Utility.WindowHeight/2;
        LevelButtonInit();
    }

    public void LevelButtonInit()
    {
        for(int i=0;i<mButtons.Count();i++)
        {
            int index = i; // 捕获当前索引
            TMP_Text text=mButtons[i].transform.Find("Text").GetComponent<TMP_Text>();
            string levelName="Level"+(index+1).ToString();
            text.text=levelName;
            mButtons[index].onClick.AddListener(()=>
            {
                SceneManager.LoadScene(levelName);
            });
        }
    }
}
