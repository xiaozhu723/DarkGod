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
    public BattleEndWindow battleEndWindow;
    public double StartTime;
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
        CharacterCreatController.Instance.ClearMapNpc(Constants.MainCitySceneID);
    }

    public void EndBattle()
    {
        playerCtrlWindow.SetWinState(false);
        GameRoot.Instance.tipsWindow.RemoveAllItemEntityHp();
    }

    public void StopAI()
    {
        battleManager.StopAI();
    }

    public void RunAI()
    {
        battleManager.RunAI();
    }

    public void DestroyBattle()
    {
        playerCtrlWindow.SetWinState(false);
        GameRoot.Instance.tipsWindow.RemoveAllItemEntityHp();
        Destroy(battleManager.gameObject);
    }

    public void SetPlayerDir(Vector2 dir)
    {
        battleManager.SetPlayerDir(dir);
    }

    public void RequestSkillAtt(int ID)
    {
        battleManager.SkillAttack(ID);
    }

    public void RequestNormalAttack()
    {
        battleManager.NormalAttack();
    }

    public void ResponseSkillAtt(GameMsg msg)
    {
        
    }

    public Vector2 GetDirInput()
    {
       return  playerCtrlWindow.GetCurrDir();
    }
}