/****************************************************
    文件：LoopDragonAin.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 21:58:25
	功能：Nothing
*****************************************************/

using UnityEngine;

public class LoopDragonAin : MonoBehaviour 
{
    public Animation ain;
    private void Start()
    {
        if (ain == null) return;
        InvokeRepeating("PlayDragonAin", 0, 20);
    }

    private void PlayDragonAin()
    {
        if (ain == null) return;
        ain.Play();
    }
}