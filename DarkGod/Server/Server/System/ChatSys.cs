/****************************************************
	文件：ChatSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/23 20:25   	
	功能：聊天管理类
*****************************************************/

using PEProtocol;

public class ChatSys
{
    private static ChatSys instance = null;

    private CacheSvc cacheSvc;
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
                instance = new ChatSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("ChatSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
    }

    //强化处理回包
    public void RequestChat(GameMsgPack pack)
    {
        var dic=  cacheSvc.GetPlayerByChatType((ChatType)pack.msg.sendChat.nChatType);
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.PushChat,
        };
        sendMsg.pushChat = new PushChat();
        sendMsg.pushChat.nChatInfo = pack.msg.sendChat.nChatInfo;
        sendMsg.pushChat.nChatType = pack.msg.sendChat.nChatType;
        sendMsg.pushChat.nPlayerID = pack.msg.sendChat.nPlayerID;
        sendMsg.pushChat.nName = pack.msg.sendChat.nName;
        byte[] bytes = PENet.PETool.PackNetMsg(sendMsg);
        TaskSys.Instance.UpdatePlayerTask(pack.serverSession, 6);
        foreach (var item in dic)
        {
            item.Key.SendMsg(bytes);
        }
       
    }
}
