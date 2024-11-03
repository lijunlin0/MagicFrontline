using UnityEngine;

public static class Utility
{
    public static int ScreenWidth=0;
    public static int ScreenHeight=0;
    public static int WindowWidth=0;
    public const int WindowHeight=1080;
    public static bool IsPC=false;

    public static void Init()
    {
        ScreenWidth=Screen.width;
        ScreenHeight=Screen.height;
        Debug.Log("屏幕宽度："+WindowWidth);
        //PC运行
        if( Application.platform == RuntimePlatform.WindowsPlayer || 
            Application.platform == RuntimePlatform.OSXPlayer || 
            Application.platform == RuntimePlatform.LinuxPlayer)
        {
            IsPC=true;
        }
        else
        {
            IsPC=false;
        }
        WindowWidth=(int)((float)WindowHeight/ScreenHeight*ScreenWidth);
    }
}