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
        InitAutoGuideCfg();
        PECommon.Log("CfgSvc  Init  Done");
    }

    Dictionary<int, AutoGuideData> autoGuideDataDic = new Dictionary<int, AutoGuideData>();
    public void InitAutoGuideCfg()
    {
        
        autoGuideDataDic.Clear();
        List<List<string>> temp = ParseRowAll(@"D:\UunityProject\DarkGodRoot\DarkGod\Client\Assets\Resources\ResCfgs\guide.csv");
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