/****************************************************
	文件：ServerSession.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 14:31   	
	功能：网络会话链接
*****************************************************/
using PENet;
using PEProtocol;
public class ServerSession:PESession<GameMsg>
{
    int SessionID = 0;
    protected override void OnConnected()
    {
        SessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("SessionID: "+ SessionID+"  Seesion Connected Done.", LogType.Info);
        
    }

    protected override void OnDisConnected()
    {
        PECommon.Log("SessionID: " + SessionID + "  Seesion DisConnected Done.", LogType.Info);
        LoginSys.Instance.ClearOfflineData(this);
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionID: " + SessionID + "  OnReciveMsg.  CMD" + ((EMCMD)msg.cmd).ToString(), LogType.Info);
        NetSvc.Instance.AddPackMsg(new GameMsgPack(msg, this));
    }
}