/****************************************************
	文件：BuySys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/24 15:26   	
	功能：购买管理类
*****************************************************/
using PEProtocol;
using System.Collections.Generic;

public class BuySys
{
    private static BuySys instance = null;

    private CacheSvc cacheSvc;
    public static BuySys Instance
    {
        get
        {
            if (instance == null)
                instance = new BuySys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("BuySys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    //购买处理回包
    public void RequestBuy(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseBuy,
        };
        sendMsg.rsqBuy = new ResponseBuy();
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
        BuyType buyType = (BuyType)pack.msg.reqBuy.nType;
        if (pack.msg.reqBuy.nType < (int)BuyType.Max)
        {
            bool isUpdateData = false;
            if(playerData.Diamond>= pack.msg.reqBuy.nCost)
            {
                switch (buyType)
                {
                    case BuyType.BuyPower:
                        isUpdateData = true;
                        playerData.Power = playerData.Power + pack.msg.reqBuy.nCost*10;
                        playerData.Diamond = playerData.Diamond - pack.msg.reqBuy.nCost;
                        break;
                    case BuyType.BuyCoin:
                        isUpdateData = true;
                        playerData.Coin = playerData.Coin + pack.msg.reqBuy.nCost *100;
                        playerData.Diamond = playerData.Diamond - pack.msg.reqBuy.nCost;
                        break;
                }
            }
            else
            {
                sendMsg.err = (int)ErrorCode.DiamondShort;
            }
            if (isUpdateData)
            {
             
                if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
                {
                    sendMsg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    sendMsg.rsqBuy.nType = pack.msg.reqBuy.nType;
                    sendMsg.rsqBuy.nResult = 1;
                    sendMsg.rsqBuy.nCoin = playerData.Coin;
                    sendMsg.rsqBuy.nPower = playerData.Power;
                    sendMsg.rsqBuy.nDiamond = playerData.Diamond;

                    TaskSys.Instance.UpdatePlayerTask(pack.serverSession, buyType == BuyType.BuyPower? 4:5);
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
