/****************************************************
	文件：GuideSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/21 18:46   	
	功能：引导管理类
*****************************************************/
using PEProtocol;
using System.Collections.Generic;

public class GuideSys
{
    private static GuideSys instance = null;

    private CacheSvc cacheSvc;
    public static GuideSys Instance
    {
        get
        {
            if (instance == null)
                instance = new GuideSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("GuideSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    //引导处理回包
    public void RequestGuide(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseGuide,
        };
        
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
        if(playerData.guideID== pack.msg.reqGuide.nGuideID)
        {
            AutoGuideData NextCfg = CfgSvc.Instance.GetAutoGuideCfg(playerData.guideID+1);
            sendMsg.rsqGuide = new ResponseGuide();
            playerData.guideID = NextCfg ==null ? playerData.guideID  : playerData.guideID +1;
            sendMsg.rsqGuide.nGuideID = playerData.guideID;
            //更新玩家数据
            AutoGuideData currCfg =  CfgSvc.Instance.GetAutoGuideCfg(playerData.guideID);
            if(currCfg!=null)
            {
                List<int> list = PECommon.GetExpUpLevelNum(playerData.Level, playerData.Exp + currCfg.exp);
                playerData.Level = playerData.Level + list[0];
                playerData.Exp = list[1];
                playerData.Coin = playerData.Coin+currCfg.coin;
                if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
                {
                    sendMsg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    sendMsg.rsqGuide.nExp = playerData.Exp;
                    sendMsg.rsqGuide.nCoin = playerData.Coin;
                    sendMsg.rsqGuide.nLevel = playerData.Level;
                }
            }
        }
        else
        {
            sendMsg.err = (int)ErrorCode.ServerDataError;
        }

        pack.serverSession.SendMsg(sendMsg);
    }
}