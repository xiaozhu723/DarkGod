using System.Collections.Generic;
using System;

public class TimerSvc
{
    class TaskPack
    {
        public int nID;
        public Action<int> action;
        public TaskPack(int nID, Action<int> action) { this.nID = nID; this.action = action; }
    }
    private static TimerSvc instance = null;
    PETimer pETimer;
    Queue<TaskPack> taskQueue = new Queue<TaskPack>();
    string taskQueueLock = "taskQueueLock";
    public static TimerSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new TimerSvc();
            return instance;
        }
    }

    public void Init()
    {
        pETimer = new PETimer(100);
        pETimer.SetLog((string str) =>
        {
            PECommon.Log(str);
        });
        pETimer.SetHandle((Action<int> act, int id) =>
        {
            lock(taskQueueLock)
            {
                taskQueue.Enqueue(new TaskPack(id, act));
            }
        });
        PECommon.Log("TimerSvc  Init  Done");
    }

    public void Update()
    {
        if (taskQueue.Count <= 0) return;
        TaskPack taskPack = null;
        lock(taskQueueLock)
        {
            taskPack = taskQueue.Dequeue();
        }
        if(taskPack!=null)
        {
            taskPack.action(taskPack.nID);
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

    public long GetNowTime()
    {
        return (long)pETimer.GetMillisecondsTime();
    }
}
