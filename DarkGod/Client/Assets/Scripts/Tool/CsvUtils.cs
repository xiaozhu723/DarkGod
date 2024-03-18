/****************************************************
    文件：CsvUtils.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/12 17:53:26
	功能：CSV工具类
*****************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;

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