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
    }

    public enum ErrorCode
    {
        None = 0,
        ServerDataError,//数据错误
        UpdateDBError,//更新数据库错误
        AcctIsOnline ,//账号已登录
        WrongPass,//密码错误
        NameIsExists ,//名字已存在
    }
    public class IPCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 4476;
    }
}
