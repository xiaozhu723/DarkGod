/****************************************************
    文件：PEGrid.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/23 14:45:58
	功能：Grid组件
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEGrid : MonoBehaviour 
{
     GridLayoutGroup grid;
    public GameObject item;
    private List<GameObject> GoPool = new List<GameObject>();
    private List<GameObject> ShowGoList= new List<GameObject>();

    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
    }
    public void UpdateItemList(List<UIItemData> list)
    {
        if (list == null || list.Count <= 0)
        {
            PECommon.Log("UpdateItemList list==null ", LogType.Error);
            return;
        }
        if(ShowGoList.Count >0 && ShowGoList.Count > list.Count)
        {
            for (int i = list.Count-1; i < ShowGoList.Count; i++)
            {
                GoToPool(ShowGoList[i]);
            }
        }
       
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = null;
            if (ShowGoList.Count > i )
            {
                go = ShowGoList[i];
            }
            else
            {
                if (GoPool.Count > 0)
                {
                    go = GoPool[0];
                    GoPool.Remove(go);
                    ShowGoList.Add(go);
                }
            }
            if (go==null)
            {
                go = Instantiate(item);
                ShowGoList.Add(go);
            }
           
            UIItem uiItem = go.GetComponent<UIItem>();
            if (uiItem == null)
            {
                PECommon.Log("预制体脚本必须继承 UIItem", LogType.Error);
            }
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            uiItem.Show();
            uiItem.SetData(list[i]);
        }
        if(grid == null)
        {
            grid = GetComponent<GridLayoutGroup>();
        }
        if(grid.startAxis == 0)
        {
            grid.SetLayoutHorizontal();
        }
        else
        {
            grid.SetLayoutVertical();
        }
    }
    public void AddItem(List<UIItemData> list)
    {
        if (list == null || list.Count <= 0)
        {
            PECommon.Log("UpdateItemList list==null ", LogType.Error);
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = null;
            if (GoPool.Count > 0)
            {
                go = GoPool[0];
                GoPool.Remove(go);
                ShowGoList.Add(go);
            }
            if (go == null)
            {
                go = Instantiate(item);
                ShowGoList.Add(go);
               
            }

            UIItem uiItem = go.GetComponent<UIItem>();
            if (uiItem == null)
            {
                PECommon.Log("预制体脚本必须继承 UIItem", LogType.Error);
            }
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            uiItem.Show();
            uiItem.SetData(list[i]);
        }
        if (grid == null)
        {
            grid = GetComponent<GridLayoutGroup>();
        }
        if (grid.startAxis == 0)
        {
            grid.SetLayoutHorizontal();
        }
        else
        {
            grid.SetLayoutVertical();
        }
    }
    private void GoToPool(GameObject go)
    {
        GoPool.Add(go);
        ShowGoList.Remove(go);
    }

    public List<GameObject> GetIItems()
    {
        return ShowGoList;
    }

    public GameObject GetIItem(int id)
    {
        if(id< ShowGoList.Count)
        {
            return ShowGoList[id];
        }
        return null;
    }
}