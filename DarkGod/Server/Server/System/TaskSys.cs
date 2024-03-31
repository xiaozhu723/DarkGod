using PEProtocol;
using System;
using System.Collections.Generic;
/****************************************************
	文件：TaskSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/25 11:28   	
	功能：任务管理类
*****************************************************/
public class TaskSys
{
    private static TaskSys instance = null;

    private CacheSvc cacheSvc;
    public static TaskSys Instance
    {
        get
        {
            if (instance == null)
                instance = new TaskSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("TaskSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    public void UpdatePlayerTask(ServerSession session,int id)
    {
        PECommon.Log("UpdatePlayerTask  id =   "+ id);
        int index = id - 1;
        PlayerData playerData = cacheSvc.GetPlayerDataBySession(session);
        if (playerData == null)
        {
            PECommon.Log("UpdatePlayerTask  playerData == null  ");
            return;
        }
        TaskData taskData =CfgSvc.Instance.GetTaskData(id);
        if (taskData == null)
        {
            PECommon.Log("UpdatePlayerTask  taskData == null  ");
            return;
        }
        if (playerData.taskArr.Length > index)
        {
            if (taskData.nMaxCount <= playerData.taskArr[index])
            {
                if (playerData.taskReceiveArr[index] != (int)ReceiveType.Unclaim && playerData.taskReceiveArr[index] != (int)ReceiveType.Already)
                {
                    playerData.taskReceiveArr[index] = (int)ReceiveType.Unclaim;
                }
            }
            else
            {
                playerData.taskArr[index] += 1;
                if (taskData.nMaxCount <= playerData.taskArr[index])
                {
                    playerData.taskReceiveArr[index] = (int)ReceiveType.Unclaim;
                }
            }
        }
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.PushTask,
        };

        sendMsg.pushTask = new PushTask();
        if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
        {
            sendMsg.err = (int)ErrorCode.UpdateDBError;
        }
        else
        {
            sendMsg.pushTask.nTaskId = id;
            sendMsg.pushTask.nNum = playerData.taskArr[index];
            sendMsg.pushTask.nReceiveType = playerData.taskReceiveArr[index];
        }
        session.SendMsg(sendMsg);
    }

    public void RequestTaskReceive(GameMsgPack pack)
    {
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.ResponseTaskReceive,
        };
        sendMsg.rsqTaskReceive = new ResponseTaskReceive();

        PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.serverSession);
        TaskData taskData = CfgSvc.Instance.GetTaskData(pack.msg.reqTaskReceive.nTaskId);
        int index = pack.msg.reqTaskReceive.nTaskId - 1;
        if (taskData == null)
        {
            return;
        }
        if (taskData != null)
        {
            switch ((ReceiveType)playerData.taskReceiveArr[index])
            {
                case ReceiveType.Unfinish:
                    sendMsg.err = (int)ErrorCode.UnfinishTask;
                    break;
                case ReceiveType.Unclaim:
                    playerData.taskReceiveArr[index] = (int)ReceiveType.Already;
                    List<int> list = PECommon.GetExpUpLevelNum(playerData.Level, playerData.Exp + taskData.nExp);
                    playerData.Level = playerData.Level + list[0];
                    playerData.Exp = list[1];
                    playerData.Coin += taskData.nCoin;
                    if (!cacheSvc.UpdatePlayerData(playerData.ID, playerData))
                    {
                        sendMsg.err = (int)ErrorCode.UpdateDBError;
                    }
                    else
                    {
                        sendMsg.rsqTaskReceive.nTaskId = pack.msg.reqTaskReceive.nTaskId;
                        sendMsg.rsqTaskReceive.nReceiveType = (int)ReceiveType.Already;
                        sendMsg.rsqTaskReceive.nCoin = playerData.Coin;
                        sendMsg.rsqTaskReceive.nExp = playerData.Exp;
                    }
                    break;
                case ReceiveType.Already:
                    sendMsg.err = (int)ErrorCode.AlreadyAward;
                    break;
                default:
                    sendMsg.err = (int)ErrorCode.UnfinishTask;
                    break;
            }
            
        }
        else
        {
            sendMsg.err = (int)ErrorCode.ServerDataError;
        }

        pack.serverSession.SendMsg(sendMsg);
    }
}
