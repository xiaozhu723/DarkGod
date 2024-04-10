/****************************************************
    文件：CommonUtility.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/12 18:36:41
	功能：公共工具类
*****************************************************/

public class CommonUtility
{
    public static int RadomInt(int min, int max, System.Random rd = null)
    {
         if(rd==null) rd = new System.Random();
         int num = rd.Next(min,max+1);
        return num;
    }

    #region 监视代码运行时间
    static System.Diagnostics.Stopwatch stopwatch;
    public static void StartCountTime()
    {
        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间
    }

    /// <summary>
    /// 监控内容名称
    /// </summary>
    /// <param name="namep"></param>

    public static void StopCountTime(string namep)
    {
        stopwatch.Stop(); //  停止监视
                          //  获取当前实例测量得出的总时间
        System.TimeSpan timespan = stopwatch.Elapsed;
        double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
        if (milliseconds >= 20)
        {
            //打印代码执行时间
            PECommon.Log(namep + "执行逻辑：" + milliseconds);
        }
        else
        {
            //打印代码执行时间
            PECommon.Log(namep + "执行逻辑：" + milliseconds);
        }
    }
    #endregion
}