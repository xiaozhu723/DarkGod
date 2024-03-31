/****************************************************
	文件：Class1.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 14:22   	
	功能：网络通信协议（客户端服务器公用）
*****************************************************/
using PENet;
using System;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public RequestLogin reqLogin;
        public ResponseLogin resLogin;
        public RequestRename reqRename;
        public ResponseRename rspRename;
        public RequestGuide reqGuide;
        public ResponseGuide rsqGuide;
        public RequestStrong reqStrong;
        public ResponseStrong rsqStrong;
        public SendChat sendChat;
        public PushChat pushChat;
        public RequestBuy reqBuy;
        public ResponseBuy rsqBuy;
        public PushAddPower pushAddPower;
        public PushTask pushTask;
        public RequestTaskReceive reqTaskReceive;
        public ResponseTaskReceive rsqTaskReceive;
        public RequestFuBenFight reqFuBenFight;
        public ResponseFuBenFight rsqFuBenFight;
    }
    #region 登录相关
    [Serializable]
    public class RequestLogin
    {
        public string strAcct;
        public string strPass;
    }
    [Serializable]
    public class ResponseLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int ID;
        public string Name;//名字
        public int Level;//等级
        public int Exp;//经验
        public int Power;//体力
        public int Coin;//金币
        public int Diamond;//钻石
        public int hp;//血量
        public int ad;//物理攻击
        public int ap;//魔法攻击
        public int addef;//物理防御
        public int apdef;//魔法防御
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率
        public int guideID;//引导Id
        public int[] strongArr;//强化部件星际数组 格式：1#2#3....
        public int materials;//产出材料
        public long offlineTime;//离线时间戳
        public int[] taskArr;//任务完成度数组 格式：1#2#3....
        public int[] taskReceiveArr;//任务完领取状态数组 格式：1#2#3....
        public int missionNum;//副本进度
    }

    //重命名
    [Serializable]
    public class RequestRename
    {
        public string strName;
    }
    [Serializable]
    public class ResponseRename
    {
        public string strName;
    }

    #endregion

    #region 引导相关
    [Serializable]
    public class RequestGuide
    {
        public int nGuideID;
    }
    [Serializable]
    public class ResponseGuide
    {
        public int nGuideID;
        public int nLevel;
        public int nCoin;
        public int nExp;
    }
    #endregion

    #region 强化相关
    [Serializable]
    public class RequestStrong
    {
        public int nPratID;
    }
    [Serializable]
    public class ResponseStrong
    {
        public int nPratID;
        public int nStarLevel;
        public int nCoin;
        public int nMaterials;
        public int nHP;
        public int nAd;
        public int naddef;
    }
    #endregion

    #region 聊天相关
    [Serializable]
    public class SendChat
    {
        public int nPlayerID;
        public int nChatType;
        public string nChatInfo;
        public string nName;
    }
    [Serializable]
    public class PushChat
    {
        public int nPlayerID;
        public int nChatType;
        public string nChatInfo;
        public string nName;
    }
    #endregion

    #region 购买相关
    [Serializable]
    public class RequestBuy
    {
        public int nType;
        public int nCost;
    }
    [Serializable]
    public class ResponseBuy
    {
        public int nType;
        public int nResult;
        public int nCoin;
        public int nPower;
        public int  nDiamond;
    }

    [Serializable]
    public class PushAddPower
    {
        public int nPower;
    }

    #endregion
    #region 任务相关
   
    [Serializable]
    public class PushTask
    {
        public int nTaskId;
        public int nNum;
        public int nReceiveType;
    }

    [Serializable]
    public class RequestTaskReceive
    {
        public int nTaskId;
    }
    [Serializable]
    public class ResponseTaskReceive
    {
        public int nTaskId;
        public int nReceiveType;
        public int nExp;
        public int nCoin;
    }
    #endregion
    #region 副本相关
    [Serializable]
    public class RequestFuBenFight
    {
        public int nID;
    }
    [Serializable]
    public class ResponseFuBenFight
    {
        public int nID;
        public int nPower;
    }

    #endregion
    public enum EMCMD
    {
        None = 0,

        //登陆相关101
        RequestLogin = 101,
        ResponseLogin = 102,

        RequestRename = 103,
        ResponseRename = 104,

        RequestGuide = 105,
        ResponseGuide = 106,

        RequestStrong = 107,
        ResponseStrong = 108,

        SendChat = 109,
        PushChat = 110,

        RequestBuy = 111,
        ResponseBuy = 112,

        PushAddPower = 113,
        PushTask = 114,

        RequestTaskReceive = 115,
        ResponseTaskReceive = 116,

        RequestFuBenFight = 117,
        ResponseFuBenFight = 118,
    }

    public enum ErrorCode
    {
        None = 0,
        ServerDataError,//数据错误
        UpdateDBError,//更新数据库错误
        AcctIsOnline ,//账号已登录
        WrongPass,//密码错误
        NameIsExists ,//名字已存在
        PratMaxLevel,//部件已满级
        CoinShort,//金币不足
        MaterialsShort,//材料不足
        LevelShort,//等级不足
        DiamondShort,//钻石不足
        AlreadyAward,//已领取奖励
        UnfinishTask,//未达到领取奖励条件
        PowerShort,//体力不足
    }

    public enum ChatType
    {
        None = 0,
        WordChat=1,
        UnionChat=2,
        FriendChat =3,
    }

    public enum BuyType
    {
        None = 0,
        BuyPower = 1,
        BuyCoin = 2,
        Max = 3,
    }

    public enum ReceiveType
    {
        None = 0,
        Unclaim = 1,//未领取
        Unfinish = 2,//未达成
        Already = 3,//已领取
        Max = 4,
    }
    
    public class IPCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 4476;
    }
}
