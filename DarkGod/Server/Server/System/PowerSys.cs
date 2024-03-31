/****************************************************
	文件：PowerSys.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/24 17:49   	
	功能：体力管理系统
*****************************************************/
using PEProtocol;

public class PowerSys
{

    private static PowerSys instance = null;

    private CacheSvc cacheSvc;
    TimerSvc timerSvc;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
                instance = new PowerSys();
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("PowerSys  Init  Done");
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        TimerSvc.Instance.AddTimerTask(AddPowerFun, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
    }

    public void AddPowerFun(int id)
    {
        var dic = cacheSvc.GetAddPowerPlayer();
        GameMsg sendMsg = new GameMsg
        {
            cmd = (int)EMCMD.PushAddPower,
        };

        sendMsg.pushAddPower = new PushAddPower();
        foreach (var item in dic)
        {
            int powerLimit = PECommon.GetPowerLimit(item.Value.Level);
            item.Value.Power += PECommon.AddPowerNum;
            if (item.Value.Power > powerLimit)
            {
                item.Value.Power = powerLimit;
            }
            item.Value.offlineTime = timerSvc.GetNowTime();
            if (!cacheSvc.UpdatePlayerData(item.Value.ID, item.Value))
            {
                sendMsg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                sendMsg.pushAddPower.nPower = item.Value.Power;
            }
            byte[] bytes = PENet.PETool.PackNetMsg(sendMsg);
            item.Key.SendMsg(bytes);
        }
       
    }
}
