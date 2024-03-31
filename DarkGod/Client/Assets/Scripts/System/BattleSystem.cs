/****************************************************
	文件：BattleSystem.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/28 18:28   	
	功能：战斗系统
*****************************************************/
using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class BattleSystem : SystemRoot
{
    public PlayerCtrlWindow playerCtrlWindow;
    public static BattleSystem Instance { get; private set; }
    BattleManager battleManager;
    public BattleManager BattleManager
    {
        get
        {
            return battleManager;
        }
        set
        {
            battleManager = value;
        }
    }
    int currSceneID;
    public override void Init()
    {
        base.Init();
        Instance = this;
        Debug.Log("Init ChatSystem Succeed");
    }

    public void StartBattle(int ID)
    {
        GameRoot.Instance.lodingWindow.SetWinState();
        currSceneID = ID;
        GameObject go = new GameObject { name = "BattleRoot" };
        go.transform.SetParent(GameRoot.Instance.transform);
        battleManager = go.AddComponent<BattleManager>();
        battleManager.Init(currSceneID);
    }

    public void SetPlayerDir(Vector2 dir)
    {
        battleManager.SetPlayerDir(dir);
    }

    public void RequestSkillAtt(int ID)
    {
        battleManager.SkillAttack(ID);
    }

    public void ResponseSkillAtt(GameMsg msg)
    {
        
    }
}