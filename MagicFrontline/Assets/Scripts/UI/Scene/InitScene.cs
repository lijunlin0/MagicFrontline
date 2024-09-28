using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    public void Awake()
    {
        GameManager.GetInstance();
    }

    public void Start()
    {
        InitScreen();
        SceneManager.LoadScene("Main");
    }
    public void InitScreen()
    {
        Utility.WindowWidth=Screen.width;
        Utility.WindowHeight=Screen.height;
        //PC运行
        if( Application.platform == RuntimePlatform.WindowsPlayer || 
            Application.platform == RuntimePlatform.OSXPlayer || 
            Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Utility.IsPC=true;
        }
        else
        {
            Utility.IsPC=false;
        }
    }
}