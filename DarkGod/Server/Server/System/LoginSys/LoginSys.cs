/****************************************************
	文件：LoginSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 13:39   	
	功能：登录系统
*****************************************************/
using PENet;
using PEProtocol;
public class LoginSys
{
    private static LoginSys instance = null;

    private CacheSvc cacheSvc;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
                instance = new LoginSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("LoginSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    //登陆处理回包
    public void ResponseLogin(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseLogin,
        };
        GameMsg msg = pack.msg;

        //判断是否在线
        if (cacheSvc.IsAcctOnline(msg.reqLogin.strAcct))
        {
            sendMsg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            PlayerData pd = cacheSvc.GetPlayerData(msg.reqLogin.strAcct, msg.reqLogin.strPass);
            if (pd == null)
            {
                sendMsg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                int power = pd.Power;
                int maxPower = PECommon.GetPowerLimit(pd.Level);
                if(power< maxPower)
                {
                    long time = pd.offlineTime;
                    long nowTime = TimerSvc.Instance.GetNowTime();
                    int addPower = (int)((nowTime - time)/(1000*60*PECommon.PowerAddSpace)*PECommon.AddPowerNum);
                    if(addPower>0)
                    {
                        power += addPower;
                        if(power> maxPower)
                        {
                            power = maxPower;
                        }
                      
                        if (!cacheSvc.UpdatePlayerData(pd.ID, pd))
                        {
                            PECommon.Log("离线体力增加失败！" + pd.ID, LogType.Error);
                        }
                        else
                        {
                            pd.Power = power;
                        }
                    }
                }
                sendMsg.resLogin = new ResponseLogin { playerData = pd };
                cacheSvc.AcctOnline(msg.reqLogin.strAcct, pack.serverSession, pd);
            }
        }

        pack.serverSession.SendMsg(sendMsg);
    }

    //重命名处理回包
    public void ResponseRename(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseRename,
        };
        GameMsg msg = pack.msg;
        if (cacheSvc.IsNameExist(msg.reqRename.strName))
        {
            sendMsg.err = (int)ErrorCode.NameIsExists;
        }
        else
        {
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
            playerData.Name = msg.reqRename.strName;
            if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
            {
                sendMsg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                sendMsg.rspRename = new ResponseRename
                {
                    strName = playerData.Name
                };
                
            }
        }
        pack.serverSession.SendMsg(sendMsg);
    }

    public void ClearOfflineData(ServerSession session)
    {
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(session);
        if (playerData != null)
        {
            playerData.offlineTime= TimerSvc.Instance.GetNowTime();
            if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
            {
                PECommon.Log("离线时间记录失败！" + playerData.ID, LogType.Error);
            }
        }
        cacheSvc.AcctOffLine(session);
    }
}
