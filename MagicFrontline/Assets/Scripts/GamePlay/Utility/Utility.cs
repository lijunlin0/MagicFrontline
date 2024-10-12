using UnityEngine;

public static class Utility
{
    
    public static int WindowWidth=0;
    public static int WindowHeight=0;
    public static bool IsPC=false;

    public static void Init()
    {
        WindowWidth=Screen.width;
        WindowHeight=Screen.height;
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