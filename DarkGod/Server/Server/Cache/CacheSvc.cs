/****************************************************
	文件：CacheServer.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 17:19   	
	功能：缓存层
*****************************************************/
using PENet;
using PEProtocol;
using System;
using System.Collections.Generic;

public class CacheSvc
{
    private static CacheSvc instance = null;
    private DBManager dBManager = null;
    private Dictionary<string, ServerSession> acctOnlineDic = new  Dictionary <string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onlineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new CacheSvc();
            return instance;
        }
    }

    public void Init()
    {
        dBManager = DBManager.Instance;
        PECommon.Log("CacheSvc  Init  Done");
    }

    //账号是否在线
    public bool IsAcctOnline(string acct)
    {
        return acctOnlineDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码获取玩家数据，不存在则默认创建
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public PlayerData GetPlayerData(string acct,string pass)
    {
        PlayerData pd = dBManager.QueryPlayerData(acct, pass);
        return pd;
    }

    //缓存玩家数据
    public void AcctOnline(string acct,ServerSession session,PlayerData playerData)
    {
        if(!acctOnlineDic.ContainsKey(acct)) {
            acctOnlineDic.Add(acct, session);
        }
        if (!onlineSessionDic.ContainsKey(session))
        {
            onlineSessionDic.Add(session, playerData);
        }
    }

    //获取玩家数据
    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if(onlineSessionDic.TryGetValue(session,out PlayerData playerData))
        {
            return playerData;
        }
        return null;
    }

    //名字是否存在
    public bool IsNameExist(string name)
    {
        return dBManager.QueryNameData(name);
    }

    //更新数据库玩家数据
    public bool UpdatePlayerData(int id , PlayerData playerData)
    {
        return dBManager.UpdatePlayerData(id, playerData);
    }

    //账号下线
    public void AcctOffLine(ServerSession session)
    {
        string key = "";
        foreach (var item in acctOnlineDic)
        {
            if(item.Value == session)
            {
                key = item.Key;
            }
        }
        if(!string.IsNullOrEmpty(key))
        {
            acctOnlineDic.Remove(key);
        }
        onlineSessionDic.Remove(session);
    }
}