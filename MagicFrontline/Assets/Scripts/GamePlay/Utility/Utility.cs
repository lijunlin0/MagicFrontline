using UnityEngine;

public static class Utility
{
    public static int WindowWidth=0;
    public const int WindowHeight=1080;
    public static bool IsPC=false;

    public static void Init()
    {
        WindowWidth=Screen.width;
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
    }
}