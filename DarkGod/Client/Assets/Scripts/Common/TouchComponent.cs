/****************************************************
    文件：TochTool.cs
	作者：TouchComponent
    邮箱:  839149608@qq.com
    日期：2024/3/18 13:15:42
	功能：摇杆组件
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchComponent : MonoBehaviour
{
    public Transform m_DirBg;
    public Transform m_DirPoint;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos;
    private float oPDis;
    PEListentr m_PEListentr = null;
    public Action<Vector2> UpdateDir;
    [HideInInspector]
    public Vector2 currentDir;
    private void Start()
    {
        oPDis = Screen.height  * 1.0f / Constants.ScreenStandrdHeight  * Constants.ScreenOPDis;
        defaultPos = m_DirBg.position;
        m_PEListentr = gameObject.GetComponent<PEListentr>();
        if (m_PEListentr == null)
            m_PEListentr = gameObject.AddComponent<PEListentr>();
        RegistrationTochEvent();
    }

    private void RegistrationTochEvent()
    {
        m_PEListentr.OnPointerDownEvent = OnPointerDownEvent;
        m_PEListentr.OnPointerUpEvent = OnPointerUpEvent;
        m_PEListentr.OnDragEvent = OnDragEvent;
    }

    public void OnPointerDownEvent(PointerEventData eventData)
    {
        startPos = eventData.position;
        m_DirBg.position = eventData.position;

        if (UpdateDir != null)
        {
            UpdateDir(Vector2.zero);
        }
    }

    public void OnPointerUpEvent(PointerEventData eventData)
    {
        m_DirBg.position = defaultPos;
        m_DirPoint.localPosition = Vector2.zero;
        currentDir = Vector2.zero;
        if (UpdateDir != null)
        {
            UpdateDir(currentDir);
        }
    }

    public void OnDragEvent(PointerEventData eventData)
    {
       Vector2 dir = eventData.position - startPos;
       float len  = dir.magnitude;
        if (len > oPDis) {
            Vector2 clampDir = Vector2.ClampMagnitude(dir, oPDis);
            m_DirPoint.position = startPos + clampDir;
        }
        else
        {
            m_DirPoint.position = eventData.position;
        }
        currentDir = dir.normalized;
        if (UpdateDir!=null)
        {
            UpdateDir(currentDir);
        }
    }

}