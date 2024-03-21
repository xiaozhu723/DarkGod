/****************************************************
    文件：MainCitySystem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/16 15:18:9
	功能：主城系统
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainCitySystem : SystemRoot
{
    public static MainCitySystem Instance { get; private set; }
    public MainCityWindow mainCityWindow;
    public PlayerInfoWindow playerInfoWindow;
    public GuideWindow guideWindow;
    private MapCfg mainSceneCfg;
    PlayerController playerController;
    private Transform charShowCam;
   
    CharacterCreatController characterCreatController;
    public override void Init()
    {
        base.Init();
        Instance = this;
        characterCreatController = CharacterCreatController.Instance;
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
            characterCreatController.CreatPlayer();
            playerController = characterCreatController.GetPlayer();
            characterCreatController.CreatMainSceneNpc();
            //更换背景音乐
            audioService.PlayBGMusic(Constants.BGMainCity, true);
            //打开主界面
            OpenMainCityWindow();
        });
        charShowCam = GameRoot.Instance.CharShowCam;
        if (charShowCam != null)
        {
            charShowCam.gameObject.SetActive(false);
        }
    }

    //打开登陆界面
    public void OpenMainCityWindow()
    {
        mainCityWindow.SetWinState();
    }
    

    public void SetPlayerDir(Vector2 dir)
    {
        playerController.StopAutoNavigation();
        if (dir != Vector2.zero)
        {
            playerController.SetBlend(Constants.BlendWalk);
        }
        else
        {
            playerController.SetBlend(Constants.BlendIdle);
        }
        playerController.Dir = dir;
    }

    public void OpenPlayerInfoWindow()
    {
        if (charShowCam != null)
        {
            charShowCam.gameObject.SetActive(true);
            charShowCam.localPosition = (playerController.transform.position + playerController.transform.forward * 3.8f) + new Vector3(0, 1.4f, 0);
            charShowCam.localEulerAngles = new Vector3(0, 180 + playerController.transform.localEulerAngles.y, 0);
            charShowCam.localScale = Vector3.one;
        }
        playerInfoWindow.SetWinState();
    }

    public void ClosePlayerInfoWindow()
    {
        if (charShowCam != null)
        {
            charShowCam.gameObject.SetActive(false);
        }
        playerInfoWindow.SetWinState(false);
        ResetPlayerRotation();
    }

    private float startRotation;
    public void SetPlayerStartRotation()
    {
        startRotation = playerController.transform.localRotation.y;
    }

    public void UpdatePlayerRotation(float y)
    {
        playerController.transform.localEulerAngles = new Vector3(0, y, 0);
    }

    public void ResetPlayerRotation()
    {
        playerController.transform.localEulerAngles = new Vector3(0, startRotation, 0);
    }
    AutoGuideData currentTaskData = null;
    public void RanTask(AutoGuideData tGuideData)
    {
       
        if (tGuideData != null)
            currentTaskData = tGuideData;
        if(currentTaskData.npcID!=-1)//-1是引导窗口 大于-1是npcID
        {
           Vector3 endNavPos =  characterCreatController.GetNPCNavPos(currentTaskData.npcID);
            float dis = Vector3.Distance(transform.position, endNavPos);
            if (dis < 0.5f)
            {
                OpenGuideWindow();
                return;
            }
            playerController.StartAutoNavigation(endNavPos, () => { OpenGuideWindow(); });
        }
        else
        {
            OpenGuideWindow();
        }
    }

    public void OpenGuideWindow()
    {
        guideWindow.SetWinState();
    }

    public void ResponseGuide(GameMsg msg)
    {
        bool runTask = GameRoot.Instance.PlayerData.guideID != msg.rsqGuide.nGuideID; ;

        GameRoot.Instance.PlayerData.Exp = msg.rsqGuide.nExp;
        GameRoot.Instance.PlayerData.Coin = msg.rsqGuide.nCoin;
        GameRoot.Instance.PlayerData.Level = msg.rsqGuide.nLevel;
        GameRoot.Instance.PlayerData.guideID = msg.rsqGuide.nGuideID;
        GameRoot.AddTips(string.Format("任务完成！获得{0}金币  增加{1}经验",currentTaskData.coin, currentTaskData.exp));
        mainCityWindow.RefreshUI();

        switch (currentTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                //进入副本
                break;
            case 2:
                //进入强化界面
                break;
            case 3:
                //购买体力
                break;
            case 4:
                //金币铸造
                break;
            case 5:
                //世界聊天
                break;
        }
        //if(runTask)
        //mainCityWindow.OnClickAutoTaskBtn();
        //else
        //    GameRoot.AddTips("引导任务结束！");
    }
}