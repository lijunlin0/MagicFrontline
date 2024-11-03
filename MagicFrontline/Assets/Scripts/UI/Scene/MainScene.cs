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
            GameObject mask=mButtons[i].transform.Find("Mask").gameObject;
            GameObject check=mButtons[i].transform.Find("PassBackground/Check").gameObject;
            if(LevelUtility.GetLevelPass(i+1))
            {
                mask.SetActive(false);
                check.SetActive(true);
            }
            else
            {
                mask.SetActive(true);
                check.SetActive(false);
            }
            Image img=mButtons[i].transform.Find("Image").GetComponent<Image>();
            Sprite levelSprite=Resources.Load<Sprite>("LevelImage/Level"+(index+1).ToString());
            img.sprite=levelSprite;
            TMP_Text text=mButtons[i].transform.Find("Text").GetComponent<TMP_Text>();
            string levelName="Level"+(i+1).ToString();
            text.text=levelName;
            mButtons[index].onClick.AddListener(()=>
            {
                SceneManager.LoadScene(levelName);
            });
        }
    }
}
