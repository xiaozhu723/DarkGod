/****************************************************
    文件：CsvUtils.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/12 17:53:26
	功能：CSV工具类
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// CSV工具类
/// </summary>
public static class CsvUtils
{
    public const string Suffix = ".csv";//CSV文件后缀

    /// <summary>
    /// 解析某一行
    /// </summary>
    /// row：从0开始
    public static List<string> ParseRow(string csvPath, int row, List<int> ignoreColIndex = null)
    {
        if (row < 0)
        {
            Debug.LogError(string.Format("解析的行数不能为负数，CSV文件：{0}，行数：{1}", csvPath, row + 1));
            return null;
        }
        if (string.IsNullOrEmpty(csvPath) || Path.GetExtension(csvPath) != Suffix)
        {
            Debug.LogError(string.Format("CSV文件路径有误：{0}", csvPath));
            return null;
        }
        string[] lineStrArray = File.ReadAllLines(csvPath);
        if (lineStrArray == null)
        {
            return null;
        }
        if (row > lineStrArray.Length - 1)
        {
            Debug.LogError(string.Format("超出表格的最大行数，CSV文件：{0}，表格行数：{1}，要解析的行数：{2}", csvPath, lineStrArray.Length, row + 1));
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
            Debug.LogError(string.Format("解析的列数不能为负数，CSV文件：{0}，列数：{1}", csvPath, col + 1));
            return null;
        }
        if (string.IsNullOrEmpty(csvPath) || Path.GetExtension(csvPath) != Suffix)
        {
            Debug.LogError(string.Format("CSV文件路径有误：{0}", csvPath));
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
            Debug.LogError(string.Format("超出表格的最大列数，CSV文件：{0}，表格列数：{1}，要解析的列数：{2}", csvPath, tempCellStrArray.Length, col + 1));
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
            Debug.LogError(string.Format("CSV文件路径有误：{0}", csvPath));
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


}




public static class CSVHelper
{
    /// <summary>
    /// 判断是否是不带 BOM 的 UTF8 格式  
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static bool IsUTF8Bytes(byte[] data)
    {
        int charByteCounter = 1; //计算当前正分析的字符应还有的字节数  
        byte curByte; //当前分析的字节.  
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前  
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }

                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　  
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1  
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }

                charByteCounter--;
            }
        }

        if (charByteCounter > 1)
        {
            throw new Exception("非预期的byte格式");
        }

        return true;
    }
    /// <summary>
    /// 通过给定的文件流，判断文件的编码类型  
    /// </summary>
    /// <param name="fs"></param>
    /// <returns></returns>
    public static System.Text.Encoding GetType(System.IO.FileStream fs)
    {
        byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM  
        System.Text.Encoding reVal = System.Text.Encoding.Default;

        System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
        int i;
        int.TryParse(fs.Length.ToString(), out i);
        byte[] ss = r.ReadBytes(i);
        if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
        {
            reVal = System.Text.Encoding.UTF8;
        }
        else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
        {
            reVal = System.Text.Encoding.BigEndianUnicode;
        }
        else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
        {
            reVal = System.Text.Encoding.Unicode;
        }

        r.Close();
        return reVal;
    }
    /// <summary>
    /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
    /// </summary>
    /// <param name="FILE_NAME">文件的编码类型</param>
    /// <returns></returns>
    public static System.Text.Encoding GetFileEnCode(string FILE_NAME)
    {
        System.IO.FileStream fs =
            new System.IO.FileStream(FILE_NAME.Replace(@"\", "/"), System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.Text.Encoding r = GetType(fs);
        fs.Close();
        return r;
    }
    public static string ReadCSVFile(string path, string name, string defType = ".csv")
    {
        var p = Path.Combine(path ,name+defType) ;
        //Encoding encoding = GetFileEnCode(p);
        //if (encoding.WebName != "utf-8")
        //{
        //    Debug.LogError(p + "encoding wrong");
        //    return null;
        //}
        var assets = ReadAllText(p);
        return assets;
    }
    /// <summary>
    /// 以UTF8编码读取文件内容
    /// </summary>
    /// <param equimpentName="path"></param>
    /// <returns></returns>
    public static string ReadAllText(string path)
    {
        if (path.StartsWith(Application.streamingAssetsPath) && Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.SendWebRequest();
            while (!www.isDone) { }
            return www.downloadHandler.text;
        }
        return ReadAllText(path, Encoding.UTF8);
    }
    /// <summary>
    /// 读取文件内容
    /// </summary>
    public static string ReadAllText(string path, Encoding encoding)
    {
        string text = "";
        try
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                text = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return text;
    }
    public static string[,] AnalysisCsvData(string csvData)
    {
        string[] lines = csvData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[,] data = ReadCsvFromLines(lines);
        return data;
    }

    public static string[,] ReadCsvData(byte[] bytes)
    {
        string[,] data = new string[,] { };
        try
        {
            string str = Encoding.UTF8.GetString(bytes);
            string[] lines = str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            data = ReadCsvFromLines(lines);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return data;
    }

    static string[,] ReadCsvFromLines(string[] lines)
    {
        int row = lines.Length;
        int col = lines.Max(x => x.Split(',').Length);
        string[,] data = new string[row, col];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] splits = lines[i].Split(',');
            for (int j = 0; j < splits.Length; j++)
            {
                data[i, j] = splits[j];
            }
        }

        return data;
    }
}



class CSVConfig
{

#if UNITY_ANDROID && !UNITY_EDITOR
		public static readonly string csvFolderPath =Application.streamingAssetsPath+ "/ResCfgs/"; //安卓的Application.streamingAssetsPath已默认有"file://"
#elif UNITY_IOS && !UNITY_EDITOR
		public static readonly string csvFolderPath ="file://" + Application.streamingAssetsPath+ "/ResCfgs/";
#elif UNITY_STANDLONE_WIN || UNITY_EDITOR
        public static readonly string csvFolderPath = Application.streamingAssetsPath + "/ResCfgs/";
#else
		string.Empty;
#endif

}
[Serializable]
public class EngineItem
{
    public uint ItemIndex;
    public string ItemName;
    public EngineItem(uint itemIndex, string itemName)
    {
        this.ItemIndex = itemIndex;
        this.ItemName = itemName;
    }
    public EngineItem()
    {

    }
}
[Serializable]
public class Flow
{
    public EngineItem[] EngineItems;
    /// <summary>
    /// 加载csv
    /// </summary>
    /// <param name="fileName">文件名字</param>
    public void LoadCSV(string fileName)
    {
        var temp = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, fileName);
        string[,] dt = CSVHelper.AnalysisCsvData(temp);
        EngineItems = GetFromData(dt);
    }
    /// <summary>
    /// 解析csv
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static EngineItem[] GetFromData(string[,] data)
    {
        List<EngineItem> courseList = new List<EngineItem>();
        int row = data.GetLength(0);
        int col = data.GetLength(1);
        for (int i = 1; i < row; i++)
        {
            string itemIndex = data[i, 0];
            uint.TryParse(itemIndex, out uint uintindex);
            string itemName = data[i, 1];
            EngineItem EngineItem = new EngineItem(uintindex, itemName);
            courseList.Add(EngineItem);
        }
        return courseList.ToArray();
    }
}

