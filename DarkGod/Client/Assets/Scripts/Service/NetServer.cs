/****************************************************
    文件：NetServer.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/14 14:49:1
	功能：网络服务类
*****************************************************/

using UnityEngine;
using PEProtocol;
using PENet;
using System.Collections.Generic;

public class NetServer : MonoBehaviour 
{
    public static NetServer Instance { get; private set; }

    private static readonly string obj = "lock";

    //消息存储队列
    private Queue<GameMsg> msgQueue = new Queue<GameMsg>();

    PESocket<ClientSession,GameMsg> mClient = null;

    public void Init()
    {
        Instance = this;
        mClient = new PESocket<ClientSession, GameMsg>();
        mClient.SetLog(true, (string msg, int lv) =>
         {
             switch (lv)
             {
                 case 0:
                     msg = "Log:" + msg;
                     Debug.Log(msg);
                     break;
                 case 1:
                     msg = "Warn:" + msg;
                     Debug.LogWarning(msg);
                     break;
                 case 2:
                     msg = "Error:" + msg;
                     Debug.LogError(msg);
                     break;
                 case 3:
                     msg = "Info:" + msg;
                     Debug.Log(msg);
                     break;
             }
         });
        mClient.StartAsClient(IPCfg.srvIP, IPCfg.srvPort);
        Debug.Log("Init NetServer....");
    }

    public void SendMessage(GameMsg msg)
    {
        if (mClient == null)
        {
            Init();
            GameRoot.AddTips("网络未初始化");
        }

        mClient.session.SendMsg(msg);
    }

    //添加消息
    public void AddPackMsg(GameMsg msg)
    {
        lock (obj)
        {
            msgQueue.Enqueue(msg);
        }
    }

    public void Update()
    {
        if (msgQueue.Count > 0)
        {
            lock (obj)
            {
                HandOutMsg(msgQueue.Dequeue());
            }
        }
    }

    //分发消息
    public void HandOutMsg(GameMsg msg)
    {
        if (msg == null) return;
        if (msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定");
                    break;
                case ErrorCode.AcctIsOnline:
                    GameRoot.AddTips("账号已登录");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;
                case ErrorCode.NameIsExists:
                    GameRoot.AddTips("名字已存在");
                    break;
            }
            return;
        }

        switch ((EMCMD)msg.cmd)
        {
            case EMCMD.ResponseLogin : //登陆回包
                LoginSystem.Instance.ResponseLogin(msg);
                break;
            case EMCMD.ResponseRename: //重命名回包
                LoginSystem.Instance.ResponseRename(msg);
                break;
        }
    }
}