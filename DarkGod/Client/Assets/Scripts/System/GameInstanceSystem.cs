/****************************************************
	文件：GameInstanceSystem.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/25 17:43   	
	功能：游戏副本管理类
*****************************************************/
using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class GameInstanceSystem : SystemRoot
{
    public GameInstanceWindow gameInstanceWindow;
    public static GameInstanceSystem Instance { get; private set; }
    MapCfg SceneCfg;
    CharacterCreatController characterCreatController;
    PlayerController playerController;
    PlayerData playerData;
    int currSceneID;
    public override void Init()
    {
        base.Init();
        Instance = this;
        characterCreatController = CharacterCreatController.Instance;
        Debug.Log("Init GameInstanceSystem Succeed");
    }

    public void OpenGameInstance()
    {
        
        gameInstanceWindow.SetWinState();
    }
 


    public void RequestFuBenFight(int ID)
    {
        currSceneID = ID;
        SceneCfg = resService.GetMapCfg(currSceneID);
        playerData = GameRoot.Instance.PlayerData;
        if (playerData == null) return;
        if (playerData.Power < SceneCfg.power)
        {
            GameRoot.AddTips("体力不足！");
            return;
        }
        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)EMCMD.RequestFuBenFight,
            reqFuBenFight = new RequestFuBenFight
            {
                nID = ID,
            }
        };

        netServer.SendMessage(gameMsg);
    }

    public void ResponseFuBenFight(GameMsg msg)
    {
        GameRoot.Instance.PlayerData.Power = msg.rsqFuBenFight.nPower;
        if (MainCitySystem.Instance.mainCityWindow.gameObject.activeSelf)
        {
            MainCitySystem.Instance.mainCityWindow.RefreshUI();
            MainCitySystem.Instance.mainCityWindow.SetWinState(false);
        }
        BattleSystem.Instance.StartBattle(currSceneID);
        gameInstanceWindow.SetWinState(false);
    }

    
}