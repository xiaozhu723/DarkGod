/****************************************************
	文件：FuBenSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/27 13:56   	
	功能：副本管理类
*****************************************************/
using PEProtocol;
using System.Collections.Generic;

public class FuBenSys
{
    private static FuBenSys instance = null;

    private CacheSvc cacheSvc;
    public static FuBenSys Instance
    {
        get
        {
            if (instance == null)
                instance = new FuBenSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("FuBenSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    public void RequestFuBenFight(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseFuBenFight,
        };
        sendMsg.rsqFuBenFight = new ResponseFuBenFight();
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
        MapCfg mapCfg = CfgSvc.Instance.GetMapCfg(pack.msg.reqFuBenFight.nID);


        if (mapCfg!=null &&pack.msg.reqFuBenFight.nID <= playerData.missionNum)
        {
           
            if (playerData.Power >= mapCfg.power)
            {
                playerData.Power -= mapCfg.power;
                if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
                {
                    sendMsg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    sendMsg.rsqFuBenFight.nID = pack.msg.reqFuBenFight.nID;
                    sendMsg.rsqFuBenFight.nPower = playerData.Power;
                }
            }
            else
            {
                sendMsg.err = (int)ErrorCode.PowerShort;
            }
        }
        else
        {
            sendMsg.err = (int)ErrorCode.ServerDataError;
        }

        pack.serverSession.SendMsg(sendMsg);
    }

    public void RequestBattleEnd(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseBattleEnd,
        };
        sendMsg.rsqBattleEnd = new ResponseBattleEnd();
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
        MapCfg mapCfg = CfgSvc.Instance.GetMapCfg(pack.msg.reqBattleEnd.nID);

      
        if (mapCfg != null )
        {
            MapCfg nextMapCfg = CfgSvc.Instance.GetMapCfg(pack.msg.reqBattleEnd.nID);
            if (playerData.missionNum == pack.msg.reqBattleEnd.nID && nextMapCfg!=null)
            {
                playerData.missionNum += 1;
            }
            playerData.Coin += mapCfg.coin;
            playerData.materials += mapCfg.crystal;
            List<int> list = PECommon.GetExpUpLevelNum(playerData.Level, playerData.Exp + mapCfg.exp);
            playerData.Level = playerData.Level + list[0];
            playerData.Exp = list[1];
            if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
            {
                sendMsg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                sendMsg.rsqBattleEnd.nID = playerData.missionNum;
                sendMsg.rsqBattleEnd.win = pack.msg.reqBattleEnd.win;
                sendMsg.rsqBattleEnd.hp = pack.msg.reqBattleEnd.hp;
                sendMsg.rsqBattleEnd.costTime = pack.msg.reqBattleEnd.costTime;
                sendMsg.rsqBattleEnd.nCoin = playerData.Coin;
                sendMsg.rsqBattleEnd.nLevel = playerData.Level;
                sendMsg.rsqBattleEnd.nExp = playerData.Exp;
                sendMsg.rsqBattleEnd.materials = playerData.materials;
                TaskSys.Instance.UpdatePlayerTask(pack.serverSession, 2);
            }
        }
        else
        {
            sendMsg.err = (int)ErrorCode.ServerDataError;
        }

        pack.serverSession.SendMsg(sendMsg);
    }
}