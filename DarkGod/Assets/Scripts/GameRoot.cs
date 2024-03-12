/****************************************************
    文件：GameRoot.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 15:45:18
	功能：游戏启动入口模块
*****************************************************/

using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance { get; private set; }

    //加载进度界面
    public LodingWindow lodingWindow;

    //加登录界面
    public LoginWindow loginWindow;

    //动态提示界面
    public TipsWindow tipsWindow;


    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Start....");
        ClearUIRoot();
        Init();
    }

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        if (canvas)
        {
            for (int i = 0; i < canvas.childCount; i++)
            {
               
                Transform temp = canvas.GetChild(i);
                Debug.Log(temp.name);
                if (temp)
                {
                    if(temp.name == "TipsWindow")
                    {
                        continue;
                    }
                    temp.gameObject.SetActive(false);
                }
            }
        }
    }
    private void  Init()
    {
        //服务模块初始化
        ResourceService resSer = GetComponent<ResourceService>();
        if(resSer != null )
        {
            resSer.Init();
        }

        //音频模块初始化
        AudioService audioService = GetComponent<AudioService>();
        if (audioService != null)
        {
            audioService.Init();
        }

        //业务模块初始化
        LoginSystem loginSys = GetComponent<LoginSystem>();
        if (loginSys != null)
        {
            loginSys.Init();
            loginSys.EnterLoding();
        }
    }

    public static void AddTips(string str)
    {
        if (!Instance.tipsWindow.gameObject.activeSelf)
        {
            Instance.tipsWindow.gameObject.SetActive(true);
        }
        Instance.tipsWindow.SetTips(str);
    }
}