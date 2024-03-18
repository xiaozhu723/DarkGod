/****************************************************
    文件：SystemRoot.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 21:47:47
	功能：系统模块基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    [HideInInspector]
    public ResourceService resService;
    [HideInInspector]
    public AudioService audioService;
    [HideInInspector]
    public NetServer netServer;
    public virtual void Init()
    {
        resService = ResourceService.Instance;
        audioService = AudioService.Instance;
        netServer = NetServer.Instance;
    }
}