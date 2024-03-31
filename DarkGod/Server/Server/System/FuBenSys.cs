/****************************************************
	文件：FuBenSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/27 13:56   	
	功能：副本管理类
*****************************************************/
using PEProtocol;

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
}