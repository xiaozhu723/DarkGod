﻿/****************************************************
	文件：PECommon.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 15:37   	
	功能：公用工具类
*****************************************************/
using PENet;
using PEProtocol;


public class PECommon
{
    public static void Log(string message = "", LogType LogLevel = LogType.Info)
    {
        PETool.LogMsg(message, (LogLevel)LogLevel);
    }

    //获取玩家战斗力
    public static int GetFightByProps(PlayerData pd)
    {
        return pd.Level * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    //获取玩家体力上限
    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    //获取玩家升级所需经验
    public static int GetExpUpValByLevel(int lv)
    {
        return 100 * lv * lv;
    }
}

public enum LogType
{
    None,
    Warn,
    Error,
    Info
}