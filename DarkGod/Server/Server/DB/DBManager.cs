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
                        critical = reader.GetInt32("critical")
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
            MySqlCommand cmd = new MySqlCommand(
                "insert into account set acct=@acct,pass =@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical", conn);
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
            MySqlCommand cmd = new MySqlCommand(
      "update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.Name);
            cmd.Parameters.AddWithValue("level", playerData.Level);
            cmd.Parameters.AddWithValue("exp", playerData.Exp);
            cmd.Parameters.AddWithValue("power", playerData.Power);
            cmd.Parameters.AddWithValue("coin", playerData.Coin);
            cmd.Parameters.AddWithValue("diamond", playerData.Diamond);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);
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