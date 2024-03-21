/****************************************************
    文件：WindowRoot.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 20:6:50
	功能：UI界面基类
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using PEProtocol;
using System;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WindowRoot : MonoBehaviour
{
    protected ResourceService resService;
    protected AudioService audioService;
    protected NetServer netServer;
    private UIItem[] mItemArray = null;

    public void SetWinState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            InitWindow();
        }
        else
        {
            Clear();
        }
    }

    protected virtual void InitWindow()
    {
        resService = ResourceService.Instance;
        audioService = AudioService.Instance;
        netServer = NetServer.Instance;
    }


    protected virtual void Clear()
    {
        resService = null;
        audioService = null;
        netServer = null;
    }

    #region Tool Functions

    protected void SetActive(GameObject obj, bool state = true)
    {
        obj.SetActive(state);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }


    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }

    protected void SetSprite(Image img, string path = "")
    {
        img.sprite = resService.LoadSprite(path, true);
    }

    #endregion
    #region Click Event
    public T GetOrAddComponent<T>(GameObject GO) where T : Component
    {
        T t = GO.GetComponent<T>();
        if (t == null)
        {
            t = GO.AddComponent<T>();
        }
        return t ;
    }
    public void OnPointerDown(GameObject GO, Action<PointerEventData> action)
    {
        PEListentr pe = GetOrAddComponent<PEListentr>(GO);
        pe.OnPointerDownEvent += action;
    }
    public void OnPointerUp(GameObject GO, Action<PointerEventData> action)
    {
        PEListentr pe = GetOrAddComponent<PEListentr>(GO);
        pe.OnPointerUpEvent += action;
    }
    public void OnDrag(GameObject GO, Action<PointerEventData> action)
    {
        PEListentr pe = GetOrAddComponent<PEListentr>(GO);
        pe.OnDragEvent += action;
    }
    #endregion
    Dictionary<Transform, UIItem[]> mItemDic = new Dictionary<Transform, UIItem[]>();
    public  void AddItem(List<UIItemData> list, Transform parent,GameObject item)
    {
        if (list == null || list.Count <= 0)
        {
            PECommon.Log("AddItem list==null ", LogType.Error);
            return;
        }
        UIItem[] mItemArray = null;
        if (mItemDic.TryGetValue(parent,out mItemArray)&& mItemArray.Length== list.Count)
        {
            for (int i = 0; i < mItemArray.Length; i++)
            {
                UIItem uiItem = mItemArray[i];
                if (uiItem == null)
                {
                    PECommon.Log("预制体脚本必须继承 UIItem", LogType.Error);
                }
                uiItem.Show();
                uiItem.SetData(list[i]);
                mItemArray[i] = uiItem;
            }
            mItemDic[parent] = mItemArray;
            return;
        }
        mItemArray = new UIItem[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = Instantiate(item);
            UIItem uiItem = go.GetComponent<UIItem>();
            if (uiItem == null)
            {
                PECommon.Log("预制体脚本必须继承 UIItem", LogType.Error);
            }
            go.transform.parent = parent;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            uiItem.Show();
            uiItem.SetData(list[i]);
            mItemArray[i] = uiItem;
        }
        
        mItemDic.Add(parent, mItemArray);
    }

    private void OnDestroy()
    {
        Clear();
    }
}