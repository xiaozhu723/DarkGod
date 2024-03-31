/****************************************************
	文件：RaycastDetection.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/25 17:56   	
	功能：射线检测类
*****************************************************/
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaycastDetection : SystemRoot
{
    private int m_TransformID = 0;
    private Dictionary<int, Action<bool>> m_RaycastDetectionDic = new Dictionary<int, Action<bool>>();
    List<int> m_IdList = new List<int>();
    private Dictionary<Transform, int> m_TransformOrIDDic = new Dictionary<Transform, int>();
    public static RaycastDetection Instance { get; private set; }

    public override void Init()
    {
        base.Init();
        Instance = this;

        Debug.Log("Init RaycastDetection Succeed");
    }

	
    public void Update()
    {
        if(m_RaycastDetectionDic.Count > 0)
        {
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 从摄像机发出射线
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) // 如果射线碰到物体
                {
                    int instanceID = hit.transform.GetInstanceID(); // 获取物体的实例ID
                    for (int i = 0; i < m_IdList.Count; i++)
                    {
                        if (m_RaycastDetectionDic.TryGetValue(m_IdList[i],out Action<bool> fun))
                        {
                            fun(instanceID == m_IdList[i]);
                        }
                    }
                }
            }
        }
    }

    public void RegisterRaycastDetection(Transform trans, Action<bool> fun)
    {
        
        if(m_RaycastDetectionDic.ContainsKey(trans.GetInstanceID()))
        {
            PECommon.Log("物体已注册，无法重复注册！", LogType.Error);
            return ;
        }
        m_RaycastDetectionDic.Add(trans.GetInstanceID(), fun);
        m_IdList.Add(trans.GetInstanceID());
    }

    public void UnRegisterRaycastDetection(Transform trans)
    {
        if (!m_RaycastDetectionDic.ContainsKey(trans.GetInstanceID()))
        {
            return;
        }
        m_IdList.Remove(trans.GetInstanceID());
        m_RaycastDetectionDic.Remove(trans.GetInstanceID());
    }
}