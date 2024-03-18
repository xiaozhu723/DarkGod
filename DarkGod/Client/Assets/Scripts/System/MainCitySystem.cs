/****************************************************
    文件：MainCitySystem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/16 15:18:9
	功能：主城系统
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.Networking;

public class MainCitySystem : SystemRoot
{
    public static MainCitySystem Instance { get; private set; }
    public MainCityWindow mainCityWindow;
    private MapCfg mainSceneCfg;
    PlayerController playerController;
    public override void Init()
    {
        base.Init();
        Instance = this;
        Debug.Log("Init MainCitySystem Succeed");
    }

    //进入主城场景
    public void EnterMainCity()
    {
        GameRoot.Instance.lodingWindow.SetWinState();
        mainSceneCfg = resService.GetMapCfg(Constants.MainCitySceneID);
        resService.AsyncLoadScene(mainSceneCfg.sceneName, () =>
        {
            //诞生主角
            CreatPlayer();
            //更换背景音乐
            audioService.PlayBGMusic(Constants.BGMainCity, true);
            //打开主界面
            OpenMainCityWindow();
        });

    }

    //打开登陆界面
    public void OpenMainCityWindow()
    {
        mainCityWindow.SetWinState();
    }

    void CreatPlayer()
    {
        GameObject player = resService.LoadPrefab(PathDefine.AssissnCityPlayerPrefab,true);
        player.transform.position = mainSceneCfg.playerBomPos;
        player.transform.localEulerAngles = mainSceneCfg.playerBomRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        playerController = player.GetComponent<PlayerController>();

        Camera.main.transform.position = mainSceneCfg.mainCamPos;
        Camera.main.transform.localEulerAngles = mainSceneCfg.mainCamRote;

        playerController.Init();
    }

    public void SetPlayerDir(Vector2 dir)
    {
        if(dir != Vector2.zero)
        {
            playerController.SetBlend(Constants.BlendWalk);
        }
        else
        {
            playerController.SetBlend(Constants.BlendIdle);
        }
        playerController.Dir = dir;
    }
}