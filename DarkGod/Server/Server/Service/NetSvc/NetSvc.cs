/****************************************************
	文件：NetSvc.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 13:40   	
	功能：网络服务系统
*****************************************************/
using PENet;
using PEProtocol;
using System.Collections.Generic;

public class GameMsgPack
{
    public GameMsg msg;
    public ServerSession serverSession;
    public GameMsgPack(GameMsg msg, ServerSession serverSession)
    {
        this.msg = msg;
        this.serverSession = serverSession;
    }
}
public class NetSvc
{
    private static NetSvc instance = null;

    private static readonly string obj = "lock";

    //消息存储队列
    private Queue<GameMsgPack> msgPackQueue = new Queue<GameMsgPack>();

    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new NetSvc();
            return instance;
        }
    }

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(IPCfg.srvIP, IPCfg.srvPort);
        PECommon.Log("NetSvc  Init  Done");
    }

    //添加消息
    public void AddPackMsg(GameMsgPack msg)
    {
        lock(obj)
        {
            msgPackQueue.Enqueue(msg);
        }
    }

    public void Update()
    {
        if (msgPackQueue.Count > 0)
        {
            lock (obj)
            {
                HandOutMsg(msgPackQueue.Dequeue());
            }
        }
    }

    //分发消息
    public void HandOutMsg(GameMsgPack pack)
    {
        switch((EMCMD)pack.msg.cmd) {
            case EMCMD.RequestLogin: //登陆相关
                LoginSys.Instance.ResponseLogin(pack);
                break;
            case EMCMD.RequestRename: //登陆相关
                LoginSys.Instance.ResponseRename(pack);
                break;
        }
    }
}
