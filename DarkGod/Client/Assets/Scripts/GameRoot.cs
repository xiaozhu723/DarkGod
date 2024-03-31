/****************************************************
    文件：GameRoot.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 15:45:18
	功能：游戏启动入口模块
*****************************************************/

using PEProtocol;
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

    //玩家数据
    private PlayerData player;
    public PlayerData PlayerData { get { return player; } }

    public Transform CharShowCam { get; private set; }

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Start....");
        ClearUIRoot();
        Init();
        if (CharShowCam == null)
        {
            CharShowCam = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }
        if (CharShowCam != null)
        {
            CharShowCam.gameObject.SetActive(false);
        }
    }

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        if (canvas)
        {
            for (int i = 0; i < canvas.childCount; i++)
            {
               
                Transform temp = canvas.GetChild(i);
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
        //网络模块初始化

        NetServer netSer = GetComponent<NetServer>();
        if (netSer != null)
        {
            netSer.Init();
        }

        //服务模块初始化
        ResourceService resSer = GetComponent<ResourceService>();
        if(resSer != null )
        {
            resSer.Init();
        }

        TimerService timerService = GetComponent<TimerService>();
        if (timerService != null)
        {
            timerService.Init();
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

        CharacterCreatController creatController = GetComponent<CharacterCreatController>();
        if (creatController != null)
        {
            creatController.Init();
        }

        MainCitySystem mainCitySys = GetComponent<MainCitySystem>();
        if (mainCitySys != null)
        {
            mainCitySys.Init();
        }

        ChatSystem chatSystem = GetComponent<ChatSystem>();
        if (chatSystem != null)
        {
            chatSystem.Init();
        }

        GameInstanceSystem gameInstanceSystem = GetComponent<GameInstanceSystem>();
        if (gameInstanceSystem != null)
        {
            gameInstanceSystem.Init();
        }
        RaycastDetection raycastDetection = GetComponent<RaycastDetection>();
        if (raycastDetection != null)
        {
            raycastDetection.Init();
        }
        BattleSystem battleSystem = GetComponent<BattleSystem>();
        if (battleSystem != null)
        {
            battleSystem.Init();
        }
        
    }

    public void SetPlayerData(PlayerData playerData)
    {
        player = playerData;
    }

    public void SetPlayerName(string name)
    {
        player.Name = name;
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