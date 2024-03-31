/****************************************************
	文件：StrongSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/22 17:05   	
	功能：强化管理类
*****************************************************/
using PEProtocol;
using System.Collections.Generic;

public class StrongSys
{
    private static StrongSys instance = null;

    private CacheSvc cacheSvc;
    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
                instance = new StrongSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("StrongSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    //强化处理回包
    public void RequestStrong(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseStrong,
        };
        int maxLevel = CfgSvc.Instance.GetStrongMaxSartLevel(pack.msg.reqStrong.nPratID);
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
        int starLevel = playerData.strongArr[pack.msg.reqStrong.nPratID] + 1;
        if (maxLevel > starLevel)
        {
            StrongData NextCfg = CfgSvc.Instance.GetStrongCfg(pack.msg.reqStrong.nPratID,starLevel);
           
            if (NextCfg!=null)
            {
                bool isStrong = true;
                if (playerData.Coin - NextCfg.nNeedCoin < 0)
                {
                    isStrong = false;
                    sendMsg.err = (int)ErrorCode.CoinShort;
                }
                if (playerData.materials - NextCfg.nNeedCrystal < 0)
                {
                    isStrong = false;
                    sendMsg.err = (int)ErrorCode.MaterialsShort;
                }
                if (playerData.Level< NextCfg.nNeedLevel )
                {
                    isStrong = false;
                    sendMsg.err = (int)ErrorCode.LevelShort;
                }
                if (isStrong)
                {
                    sendMsg.rsqStrong = new ResponseStrong();
                    playerData.Coin = playerData.Coin - NextCfg.nNeedCoin;
                    playerData.strongArr[pack.msg.reqStrong.nPratID] = starLevel;
                    playerData.materials = playerData.materials - NextCfg.nNeedCrystal;
                    playerData.hp = playerData.hp + NextCfg.nAddHP;
                    playerData.ad = playerData.ad + NextCfg.nAddHurt;
                    playerData.addef = playerData.hp + NextCfg.nAddDef;
                    if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
                    {
                        sendMsg.err = (int)ErrorCode.UpdateDBError;
                    }
                    else
                    {
                        sendMsg.rsqStrong.nMaterials = playerData.materials;
                        sendMsg.rsqStrong.nCoin = playerData.Coin;
                        sendMsg.rsqStrong.nStarLevel = starLevel;
                        sendMsg.rsqStrong.nPratID = pack.msg.reqStrong.nPratID;
                        sendMsg.rsqStrong.nHP = playerData.hp;
                        sendMsg.rsqStrong.nAd = playerData.ad;
                        sendMsg.rsqStrong.naddef = playerData.addef;
                        TaskSys.Instance.UpdatePlayerTask(pack.serverSession, 3);
                    }
                }
               
            }
        }
        else
        {
            sendMsg.err = (int)ErrorCode.PratMaxLevel;
        }

        pack.serverSession.SendMsg(sendMsg);
    }
}
