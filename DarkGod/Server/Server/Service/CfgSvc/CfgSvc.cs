/****************************************************
	文件：CfgSvc.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/21 18:55   	
	功能：
*****************************************************/
using PENet;
using PEProtocol;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class CfgSvc
{
    private static CfgSvc instance = null;

    public static CfgSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new CfgSvc();
            return instance;
        }
    }

    public void Init()
    {
        InitMapCfg();
        InitAutoGuideCfg();
        InitStrongCfg();
        InitTaskCfg();
        PECommon.Log("CfgSvc  Init  Done");
    }

    public List<List<string>>  RowAllCfg(string name)
    {
       return  ParseRowAll(string.Format(@"D:\UunityProject\DarkGodRoot\DarkGod\Client\Assets\StreamingAssets\ResCfgs\{0}.csv", name));
    }

    Dictionary<int, MapCfg> mapCfgDic = new Dictionary<int, MapCfg>();
    public void InitMapCfg()
    {
        
        mapCfgDic.Clear();
        List<List<string>> temp = RowAllCfg("map");
        for (int i = 0; i < temp.Count; i++)
        {

            if (!int.TryParse(temp[i][0], out int id))
            {
                continue;
            }
            //for (int j = 0; j < temp[i].Count; j++)
            //{
            //    Debug.LogError("temp[i]  " + "i    " + i + "   j   " + j + temp[i][j]);
            //}
            MapCfg cfg = new MapCfg();
            cfg.ID = id;
            cfg.mapName = temp[i][1];
            cfg.sceneName = temp[i][2];
            cfg.power = int.Parse(temp[i][3]);
            cfg.monsterLst = temp[i][8];

            cfg.exp = int.Parse(temp[i][9]);
            cfg.coin = int.Parse(temp[i][10]);
            cfg.crystal = int.Parse(temp[i][11]);
            mapCfgDic.Add(id, cfg);
        }
        
    }

    public MapCfg GetMapCfg(int id)
    {
        MapCfg cfg = null;
        if (mapCfgDic.TryGetValue(id, out cfg))
        {
            return cfg;
        }
        return cfg;
    }

    //引导
    Dictionary<int, AutoGuideData> autoGuideDataDic = new Dictionary<int, AutoGuideData>();
    public void InitAutoGuideCfg()
    {
        
        autoGuideDataDic.Clear();
        List<List<string>> temp = RowAllCfg("guide");
        for (int i = 0; i < temp.Count; i++)
        {
            if (!int.TryParse(temp[i][0], out int id))
            {
                continue;
            }
            AutoGuideData cfg = new AutoGuideData();
            cfg.ID = id;
            cfg.npcID = int.Parse(temp[i][1]);
            cfg.dilogArr = temp[i][2];
            cfg.actID = int.Parse(temp[i][3]);
            cfg.coin = int.Parse(temp[i][4]);
            cfg.exp = int.Parse(temp[i][5]);
            autoGuideDataDic.Add(id, cfg);
        }
        
    }

    public AutoGuideData GetAutoGuideCfg(int id)
    {
        AutoGuideData cfg = null;
        if (autoGuideDataDic.TryGetValue(id, out cfg))
        {

        }
        return cfg;

    }

    //强化配置
    Dictionary<int, List<StrongData>> StrongDataDic = new Dictionary<int, List<StrongData>>();
    public void InitStrongCfg()
    {
        
        StrongDataDic.Clear();
        List<List<string>> temp = RowAllCfg("strong");
        int partID = -1;
        string path = "";
        for (int i = 0; i < temp.Count; i++)
        {
            if (!int.TryParse(temp[i][0], out int id))
            {
                continue;
            }

            StrongData cfg = new StrongData();
            cfg.ID = id;
            int paId = int.Parse(temp[i][1]);
            if (partID != paId)
            {
                partID = paId;
                StrongDataDic.Add(partID, new List<StrongData>());
            }
            cfg.nPartID = paId;
            cfg.nStarLevel = int.Parse(temp[i][2]);
            cfg.nAddHP = int.Parse(temp[i][3]);
            cfg.nAddHurt = int.Parse(temp[i][4]);
            cfg.nAddDef = int.Parse(temp[i][5]);
            cfg.nNeedLevel = int.Parse(temp[i][6]);
            cfg.nNeedCoin = int.Parse(temp[i][7]);

            cfg.nNeedCrystal = int.Parse(temp[i][8]);
            string str = temp[i][9];
            if (str != "")
            {
                path = str;
            }
            cfg.iconPath = path;
            StrongDataDic[partID].Add(cfg);
        }
        
    }

    public StrongData GetStrongCfg(int id, int starLevel)
    {
        List<StrongData> cfg = null;
        StrongData data = null;
        if (StrongDataDic.TryGetValue(id, out cfg))
        {
            for (int i = 0; i < cfg.Count; i++)
            {
                if (starLevel == cfg[i].nStarLevel)
                {
                    data = cfg[i];
                    return data;
                }
            }
        }
        return data;
    }

    public int GetStrongMaxSartLevel(int id)
    {
        int starLevel = 0;
        List<StrongData> cfg = null;
        if (StrongDataDic.TryGetValue(id, out cfg))
        {
            for (int i = 0; i < cfg.Count; i++)
            {
                if (cfg[i].nStarLevel > starLevel)
                {
                    starLevel = cfg[i].nStarLevel;
                }
            }
        }
        return starLevel;
    }

    //任务配置
    List<TaskData> TaskDataList = new List<TaskData>();
    public void InitTaskCfg()
    {
      
        TaskDataList.Clear();
        List<List<string>> temp = RowAllCfg("taskreward");
        int partID = -1;
        string path = "";
        for (int i = 0; i < temp.Count; i++)
        {
            if (!int.TryParse(temp[i][0], out int id))
            {
                continue;
            }

            TaskData cfg = new TaskData();
            cfg.ID = id;
            cfg.strTaskName = temp[i][1];
            cfg.nMaxCount = int.Parse(temp[i][2]);
            cfg.nExp = int.Parse(temp[i][3]);
            cfg.nCoin = int.Parse(temp[i][4]);
            TaskDataList.Add(cfg);
        }
        
    }

    public List<TaskData> GetTaskDataList()
    {
        return TaskDataList;
    }

    public TaskData GetTaskData(int ID)
    {
        TaskData taskData = null;
        for (int i = 0; i < TaskDataList.Count; i++)
        {
            if (TaskDataList[i].ID == ID)
            {
                taskData = TaskDataList[i];
                return taskData;
            }
        }
        return taskData;
    }

    #region csv读取
    public const string Suffix = ".csv";//CSV文件后缀

    /// <summary>
    /// 解析某一行
    /// </summary>
    /// row：从0开始
    public static List<string> ParseRow(string csvPath, int row, List<int> ignoreColIndex = null)
    {
        if (row < 0)
        {
           PECommon.Log(string.Format("解析的行数不能为负数，CSV文件：{0}，行数：{1}", csvPath, row + 1));
            return null;
        }
        if (string.IsNullOrEmpty(csvPath) || Path.GetExtension(csvPath) != Suffix)
        {
           PECommon.Log(string.Format("CSV文件路径有误：{0}", csvPath));
            return null;
        }
        string[] lineStrArray = File.ReadAllLines(csvPath);
        if (lineStrArray == null)
        {
            return null;
        }
        if (row > lineStrArray.Length - 1)
        {
           PECommon.Log(string.Format("超出表格的最大行数，CSV文件：{0}，表格行数：{1}，要解析的行数：{2}", csvPath, lineStrArray.Length, row + 1));
            return null;
        }
        List<string> ret = new List<string>();
        string rowStr = lineStrArray[row].Replace("\r", "");
        string[] cellStrArray = rowStr.Split(',');
        for (int col = 0; col < cellStrArray.Length; col++)
        {
            if (ignoreColIndex != null && ignoreColIndex.Contains(col))
            {
                continue;
            }
            ret.Add(cellStrArray[col]);
        }
        return ret;
    }

    /// <summary>
    /// 解析某一列
    /// </summary>
    /// col：从0开始
    public static List<string> ParseCol(string csvPath, int col, List<int> ignoreRowIndex = null)
    {
        if (col < 0)
        {
           PECommon.Log(string.Format("解析的列数不能为负数，CSV文件：{0}，列数：{1}", csvPath, col + 1));
            return null;
        }
        if (string.IsNullOrEmpty(csvPath) || Path.GetExtension(csvPath) != Suffix)
        {
           PECommon.Log(string.Format("CSV文件路径有误：{0}", csvPath));
            return null;
        }
        string[] lineStrArray = File.ReadAllLines(csvPath);
        if (lineStrArray == null)
        {
            return null;
        }
        string[] tempCellStrArray = lineStrArray[0].Replace("\r", "").Split(',');
        if (col > tempCellStrArray.Length - 1)
        {
           PECommon.Log(string.Format("超出表格的最大列数，CSV文件：{0}，表格列数：{1}，要解析的列数：{2}", csvPath, tempCellStrArray.Length, col + 1));
            return null;
        }
        List<string> ret = new List<string>();
        for (int row = 0; row < lineStrArray.Length; row++)
        {
            if (ignoreRowIndex != null && ignoreRowIndex.Contains(row))
            {
                continue;
            }
            string rowStr = lineStrArray[row];
            string[] cellStrArray = rowStr.Replace("\r", "").Split(',');
            ret.Add(cellStrArray[col]);
        }
        return ret;
    }

    /// <summary>
    /// 解析所有行
    /// </summary>
    public static List<List<string>> ParseRowAll(string csvPath, List<int> ignoreRow = null, List<int> ignoreCol = null)
    {
        if (string.IsNullOrEmpty(csvPath) || Path.GetExtension(csvPath) != Suffix)
        {
            PECommon.Log(string.Format("CSV文件路径有误：{0}", csvPath));
            return null;
        }
        List<List<string>> ret = new List<List<string>>();
        string[] lineStrArray = File.ReadAllLines(csvPath);
        if (lineStrArray == null)
        {
            return null;
        }
        for (int row = 0; row < lineStrArray.Length; row++)
        {
            if (ignoreRow != null && ignoreRow.Contains(row))
            {
                continue;
            }
            List<string> rowStrList = new List<string>();
            string rowStr = lineStrArray[row].Replace("\r", "");
            string[] cellStrArray = rowStr.Split(',');
            for (int col = 0; col < cellStrArray.Length; col++)
            {
                if (ignoreCol != null && ignoreCol.Contains(col))
                {
                    continue;
                }
                rowStrList.Add(cellStrArray[col]);
            }
            ret.Add(rowStrList);
        }
        return ret;
    }

    #endregion
}

public class BaseData<T>
{
    public int ID;
}


//自动任务配置
public class AutoGuideData : BaseData<AutoGuideData>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

//强化配置
public class StrongData : BaseData<StrongData>
{
    public int nPartID;//部件id
    public int nStarLevel;
    public int nAddHP;
    public int nAddHurt;
    public int nAddDef;
    public int nNeedLevel;
    public int nNeedCoin;
    public int nNeedCrystal;
    public string iconPath;
}

//任务配置
public class TaskData : BaseData<TaskData>
{
    public string strTaskName;
    public int nMaxCount;
    public int nExp;
    public int nCoin;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public int power;
    public string monsterLst;
    public int exp;
    public int coin;
    public int crystal;
}