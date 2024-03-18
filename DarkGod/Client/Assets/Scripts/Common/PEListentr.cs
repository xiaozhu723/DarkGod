/****************************************************
    文件：PEListentr.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/18 13:28:25
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PEListentr : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<PointerEventData> OnPointerDownEvent;
    public Action<PointerEventData> OnPointerUpEvent;
    public Action<PointerEventData> OnDragEvent;

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent?.Invoke(eventData);
    }
}