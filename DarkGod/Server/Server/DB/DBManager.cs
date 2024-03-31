/****************************************************
	文件：DBManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/15 14:22   	
	功能：数据库管理类
*****************************************************/
using MySql.Data.MySqlClient;
using PEProtocol;
using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
public class DBManager
{
    private static DBManager instance = null;
    private MySqlConnection conn = null;
    public static DBManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DBManager();
            return instance;
        }
    }

    public void Init()
    {
        conn = new MySqlConnection("server=localhost;User Id = root;passwrod=;Database=darkgod;charset=utf8");
        conn.Open();
        PECommon.Log("DBManager  Init  Done");
    }

    //根据账号查找玩家数据
    public PlayerData QueryPlayerData(string acct, string pass)
    {
        bool isNew = true;
        PlayerData playerData = null;
        MySqlCommand cmd = new MySqlCommand("select * from account where acct =@acct", conn);
        cmd.Parameters.AddWithValue("acct", acct);
        MySqlDataReader reader = cmd.ExecuteReader();

        try
        {
            if (reader.Read())
            {
                isNew = false;
                if (pass.Equals(reader.GetString("pass")))
                {
                  
                    var Arr = reader.GetString("strongArr").Split('#');
                    int[] temp = new int[Arr.Length];
                    for (int i = 0; i < Arr.Length; i++)
                    {
                        temp[i] = Convert.ToInt32(Arr[i]);
                    }
                    var Arr2 = reader.GetString("taskArr").Split('#');
                    int[] temp2 = new int[Arr2.Length];
                    for (int i = 0; i < Arr2.Length; i++)
                    {
                        temp2[i] = Convert.ToInt32(Arr2[i]);
                    }

                    var Arr3 = reader.GetString("taskReceiveArr").Split('#');
                    int[] temp3 = new int[Arr3.Length];
                    for (int i = 0; i < Arr3.Length; i++)
                    {
                        temp3[i] = Convert.ToInt32(Arr3[i]);
                    }
                    playerData = new PlayerData
                    {
                        ID = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Level = reader.GetInt32("level"),
                        Exp = reader.GetInt32("exp"),
                        Power = reader.GetInt32("power"),
                        Coin = reader.GetInt32("coin"),
                        Diamond = reader.GetInt32("diamond"),
                         hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),
                        guideID = reader.GetInt32("guideID"),
                        strongArr = temp,//强化部件星际数组 格式：1#2#3...
                        materials = reader.GetInt32("materials"),
                        offlineTime = reader.GetInt64("offlineTime"),
                        taskArr = temp2,
                        taskReceiveArr = temp3,
                        missionNum = reader.GetInt32("missionNum"),
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            PECommon.Log("Query  PlayerData  By  acct&pass  Error  " + ex, LogType.Error);
        }

        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (isNew)
            {
                var list = CfgSvc.Instance.GetTaskDataList();
                playerData = new PlayerData
                {
                    ID = -1,
                    Name = "",
                    Level = 1,
                    Exp = 0,
                    Power = 150,
                    Coin = 5000,
                    Diamond = 500,
                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,
                    guideID = 1001,
                    strongArr = new int[]
                    {
                        0,0,0,0,0,0
                    },
                    materials =20,
                    offlineTime = TimerSvc.Instance.GetNowTime(),
                    taskArr = new int[list.Count],
                    taskReceiveArr = new int[list.Count],
                    missionNum =10001,
                };
                playerData.ID = InsertNewAcct(acct, pass, playerData);
            }
        }

        return playerData;
    }
    //插入玩家账号数据
    private int InsertNewAcct(string acct, string pass, PlayerData pd)
    {
        int id = -1;
        try
        {
            string strong = "";
            for (int i = 0; i < pd.strongArr.Length; i++)
            {
                strong = strong +(i ==0?"": "#" )+ pd.strongArr[i];
            }

            string task = "";
            for (int i = 0; i < pd.taskArr.Length; i++)
            {
                task = task + (i == 0 ? "" : "#") + pd.taskArr[i];
            }
         
            string taskReceive = "";
            for (int i = 0; i < pd.taskReceiveArr.Length; i++)
            {
                taskReceive = taskReceive + (i == 0 ? "" : "#") + pd.taskReceiveArr[i];
            }
           
            MySqlCommand cmd = new MySqlCommand(
                "insert into account set acct=@acct,pass =@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideID=@guideID,strongArr=@strongArr,materials=@materials,offlineTime=@offlineTime,taskArr=@taskArr,taskReceiveArr=@taskReceiveArr,missionNum=@missionNum", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.Name);
            cmd.Parameters.AddWithValue("level", pd.Level);
            cmd.Parameters.AddWithValue("exp", pd.Exp);
            cmd.Parameters.AddWithValue("power", pd.Power);
            cmd.Parameters.AddWithValue("coin", pd.Coin);
            cmd.Parameters.AddWithValue("diamond", pd.Diamond);
            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideID", pd.guideID);
            cmd.Parameters.AddWithValue("strongArr", strong);
            cmd.Parameters.AddWithValue("materials", pd.materials);
            cmd.Parameters.AddWithValue("offlineTime", pd.offlineTime);
            cmd.Parameters.AddWithValue("taskArr", task);
            cmd.Parameters.AddWithValue("taskReceiveArr", taskReceive);
            cmd.Parameters.AddWithValue("missionNum", pd.missionNum);
            
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception ex)
        {
            PECommon.Log("Insert  NewAcct  Error  " + ex, LogType.Error);
        }
        return id;
    }

    //查询玩家名字是否存在
    public bool QueryNameData(string name)
    {
        bool exist = false;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name= @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                exist = true;
            }
        }
        catch (System.Exception ex)
        {
            PECommon.Log("QueryNameData  Error  " + ex, LogType.Error);
        }

        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        return exist;
    }

    //更新数据库玩家数据
    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        //更新玩家数据
        try
        {
            string strong = "";
            for (int i = 0; i < playerData.strongArr.Length; i++)
            {
                strong = strong + (i == 0 ? "" : "#") + playerData.strongArr[i];
            }

            string task = "";
            for (int i = 0; i < playerData.taskArr.Length; i++)
            {
                task = task + (i == 0 ? "" : "#") + playerData.taskArr[i];
            }
            string taskReceive = "";
            for (int i = 0; i < playerData.taskReceiveArr.Length; i++)
            {
                taskReceive = taskReceive + (i == 0 ? "" : "#") + playerData.taskReceiveArr[i];
            }
            MySqlCommand cmd = new MySqlCommand(
      "update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideID=@guideID,strongArr=@strongArr,materials=@materials,offlineTime=@offlineTime,taskArr=@taskArr,taskReceiveArr=@taskReceiveArr,missionNum=@missionNum where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.Name);
            cmd.Parameters.AddWithValue("level", playerData.Level);
            cmd.Parameters.AddWithValue("exp", playerData.Exp);
            cmd.Parameters.AddWithValue("power", playerData.Power);
            cmd.Parameters.AddWithValue("coin", playerData.Coin);
            cmd.Parameters.AddWithValue("diamond", playerData.Diamond);
            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);
            cmd.Parameters.AddWithValue("guideID", playerData.guideID);
            cmd.Parameters.AddWithValue("strongArr", strong);
            cmd.Parameters.AddWithValue("materials", playerData.materials);
            cmd.Parameters.AddWithValue("offlineTime", playerData.offlineTime);
            cmd.Parameters.AddWithValue("taskArr", task);
            cmd.Parameters.AddWithValue("taskReceiveArr", taskReceive);
            cmd.Parameters.AddWithValue("missionNum", playerData.missionNum);
            //TOADD Others
            cmd.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
            PECommon.Log("UpdatePlayerData  Error  " + ex, LogType.Error);
            return false;
        }
        return true;
    }
}