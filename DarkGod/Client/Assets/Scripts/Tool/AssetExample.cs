/****************************************************
    文件：AssetExample.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/21 11:24:54
	功能：读取 AssetDatabase 数据文件
*****************************************************/

using UnityEngine;
using UnityEditor;
using System.Xml.Linq;

public class AssetExample
{
    //// 保存数据到Asset文件
    //[MenuItem("Example/Save Data to Asset")]
    //public static void SaveDataToAsset() 
    //{
    //    // 创建一个新的MyData对象
    //    MyData data = new MyData();
    //    data.myInt = 10;
    //    data.myString = "Hello Asset";

    //    // 获取或创建一个Asset文件
    //    string path = "Assets/MyData.asset";
    //    MyData assetData = AssetDatabase.LoadAssetAtPath<MyData>(path);
    //    if (assetData == null)
    //    {
    //        assetData = ScriptableObject.CreateInstance<MyData>();
    //        AssetDatabase.CreateAsset(assetData, path);
    //    }

    //    // 将数据复制到Asset文件
    //    EditorUtility.CopySerialized(data, assetData);

    //    // 保存更改
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}

  
    public static void LoadDataFromAsset()
    {
        // 获取Asset文件的路径
        string path = "Assets/MyData.asset";
        MyData assetData = AssetDatabase.LoadAssetAtPath<MyData>(path);

        // 如果Asset文件存在，则输出其数据
        if (assetData != null)
        {
            Debug.Log("Loaded Data:");
            Debug.Log("Int: " + assetData.myInt);
            Debug.Log("String: " + assetData.myString);
        }
        else
        {
            Debug.Log("Asset not found at " + path);
        }
    }
}

// 自定义的数据类型，用于序列化
[System.Serializable]
public class MyData : ScriptableObject
{
    public int myInt;
    public string myString;
}