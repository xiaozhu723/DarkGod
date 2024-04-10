/****************************************************
	文件：TimerService.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/24 16:41   	
	功能：定时器服务
*****************************************************/
using System;
using UnityEngine;

public class TimerService: SystemRoot
{
    public static TimerService Instance { get; private set; }
    PETimer pETimer;
    public override void Init()
    {
        base.Init();
        Instance = this;
        pETimer = new PETimer();
        pETimer.SetLog((string str) =>
        {
            PECommon.Log(str);
        });
    }

    private void Update()
    {
        if (pETimer != null)
        {
            pETimer.Update();
        }
    }

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="callback">回调</param>
    /// <param name="delay">掩饰时间</param>
    /// <param name="timeUnit">时间单位</param>
    /// <param name="count">循环次数 0无限循环 </param>
    /// <returns>定时器id</returns>
    public int AddTimerTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pETimer.AddTimeTask(callback, delay, timeUnit, count);
    }

    public void DeleteTimeTask(int id)
    {
        pETimer.DeleteTimeTask(id);
    }

    public double GetNowTime()
    {
        return pETimer.GetMillisecondsTime();
    }
}