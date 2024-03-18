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
                msg.err = (int)ErrorCode.UpdateDBError;
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
        cacheSvc.AcctOffLine(session);
    }
}
