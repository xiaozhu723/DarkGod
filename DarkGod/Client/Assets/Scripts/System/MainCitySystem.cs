/****************************************************
    文件：MainCitySystem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/16 15:18:9
	功能：主城系统
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainCitySystem : SystemRoot
{
    public static MainCitySystem Instance { get; private set; }
    public MainCityWindow mainCityWindow;
    public PlayerInfoWindow playerInfoWindow;
    public GuideWindow guideWindow;
    public StrongWindow strongWindow;
    public ChatWindow chatWindow;
    public BuyTipsWindow buyTipsWindow;
    public TaskWindow taskWindow;

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
            characterCreatController.CreatPlayer(mainSceneCfg);
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

    //进入副本界面
    public void EnterGameInstance()
    {
        GameInstanceSystem.Instance.OpenGameInstance();
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

    public void StopAutoNavigation()
    {
        playerController.StopAutoNavigation();
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
        if (currentTaskData.npcID != -1)//-1是引导窗口 大于-1是npcID
        {
            Vector3 endNavPos = characterCreatController.GetNPCNavPos(currentTaskData.npcID);
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
        GameRoot.AddTips(string.Format("任务完成！获得{0}金币  增加{1}经验", currentTaskData.coin, currentTaskData.exp));
        mainCityWindow.RefreshUI();

        switch (currentTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                EnterGameInstance();
                //进入副本
                break;
            case 2:
                //进入强化界面
                OpenStrongWindow();
                break;
            case 3:
                //购买体力
                OpenBuyWindow(BuyType.BuyPower);
                break;
            case 4:
                //金币铸造
                OpenBuyWindow(BuyType.BuyCoin);
                break;
            case 5:
                //世界聊天
                OpenChatWindow();
                break;
        }
        //if(runTask)
        //mainCityWindow.OnClickAutoTaskBtn();
        //else
        //    GameRoot.AddTips("引导任务结束！");
    }

    public void OpenStrongWindow()
    {
        strongWindow.SetWinState();
    }

    public void RequestStrong(int nID)
    {
        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)EMCMD.RequestStrong,
            reqStrong = new RequestStrong
            {
                nPratID = nID,
            }
        };

        netServer.SendMessage(gameMsg);
    }
    public void ResponseStrong(GameMsg msg)
    {
        int oldFinth = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);

        GameRoot.Instance.PlayerData.Coin = msg.rsqStrong.nCoin;
        GameRoot.Instance.PlayerData.materials = msg.rsqStrong.nMaterials;
        GameRoot.Instance.PlayerData.strongArr[msg.rsqStrong.nPratID] = msg.rsqStrong.nStarLevel;
        GameRoot.Instance.PlayerData.hp = msg.rsqStrong.nHP;
        GameRoot.Instance.PlayerData.ad = msg.rsqStrong.nAd;
        GameRoot.Instance.PlayerData.addef = msg.rsqStrong.naddef;
        int NewFinth = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(string.Format("强化成功！战力提升" + (NewFinth - oldFinth)));
        mainCityWindow.RefreshUI();
        strongWindow.RefreshUI();

    }


    public void OpenChatWindow()
    {
        chatWindow.SetWinState();
    }

    public void OpenBuyWindow(BuyType type)
    {
        buyTipsWindow.SetBuyType(type);
        buyTipsWindow.SetWinState();
    }

    public void RequestBuy(int type, int cost)
    {
        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)EMCMD.RequestBuy,
            reqBuy = new RequestBuy
            {
                nType = type,
                nCost = cost,
            }
        };

        netServer.SendMessage(gameMsg);
    }
    public void ResponseBuy(GameMsg msg)
    {
        if (msg.rsqBuy.nResult == 1)
        {
            GameRoot.Instance.PlayerData.Coin = msg.rsqBuy.nCoin;
            GameRoot.Instance.PlayerData.Power = msg.rsqBuy.nPower;
            GameRoot.Instance.PlayerData.Diamond = msg.rsqBuy.nDiamond;
            GameRoot.AddTips("购买成功");
        }
        else
        {
            GameRoot.AddTips("购买失败");
        }
        mainCityWindow.RefreshUI();
        buyTipsWindow.SetWinState(false);
    }


    public void PushAddPower(GameMsg msg)
    {
        GameRoot.Instance.PlayerData.Power = msg.pushAddPower.nPower;
        if (!mainCityWindow.gameObject.activeSelf) return;
        mainCityWindow.RefreshUI();
    }

    public void OpenTaskWindow()
    {
        taskWindow.SetWinState();
    }

    public void PushTask(GameMsg msg)
    {
        GameRoot.Instance.PlayerData.taskArr[msg.pushTask.nTaskId - 1] = msg.pushTask.nNum;
        GameRoot.Instance.PlayerData.taskReceiveArr[msg.pushTask.nTaskId - 1] = msg.pushTask.nReceiveType;
        if (!taskWindow.gameObject.activeSelf) return;
        taskWindow.RefreshUI();
    }
    public void RequestTaskReceive(int id)
    {
        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)EMCMD.RequestTaskReceive,
            reqTaskReceive = new RequestTaskReceive
            {
                nTaskId = id,
            }
        };

        netServer.SendMessage(gameMsg);
    }


    public void ResponseTaskReceive(GameMsg msg)
    {
        int coin = msg.rsqTaskReceive.nCoin - GameRoot.Instance.PlayerData.Coin;
        int exp = msg.rsqTaskReceive.nExp - GameRoot.Instance.PlayerData.Exp;
        GameRoot.Instance.PlayerData.taskReceiveArr[msg.rsqTaskReceive.nTaskId - 1] = msg.rsqTaskReceive.nReceiveType;
        GameRoot.Instance.PlayerData.Exp = msg.rsqTaskReceive.nExp;
        GameRoot.Instance.PlayerData.Coin = msg.rsqTaskReceive.nCoin;
        GameRoot.AddTips(string.Format("领取奖励成功！获得{0}金币  增加{1}经验", coin, exp));
        if (mainCityWindow.gameObject.activeSelf)
        {
            mainCityWindow.RefreshUI();
        }
        if (!taskWindow.gameObject.activeSelf) return;
        taskWindow.RefreshUI();
    }
}