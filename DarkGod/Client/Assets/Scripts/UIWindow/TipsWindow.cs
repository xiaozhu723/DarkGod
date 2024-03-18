/****************************************************
    文件：TipsWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/11 14:6:47
	功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsWindow : WindowRoot 
{
    private enum PlayState
    {
        None = 0,
        Playing = 1,
        EndPlay = 2,
    }

    public Animation ain;
    public Text m_TipsText;
    private PlayState currPlayState = PlayState.None;
    private Queue<string> tipsQueue = new Queue<string>();
    protected override void InitWindow()
    {
        base.InitWindow();
        currPlayState = PlayState.None;
        SetText(m_TipsText,"");
        SetActive(m_TipsText, false);
    }

    public void SetTips(string str)
    {
        if(currPlayState == PlayState.Playing)
        {
            AddTips(str);
            return;
        }
        currPlayState = PlayState.Playing;
        SetActive(m_TipsText, true);
        SetText(m_TipsText, str);
        AnimationClip clip = ain.GetClip("TipsAin");
        ain.Play();
        StartCoroutine(PlayAinCallBack(clip.length, () =>
        {
            SetText(m_TipsText, "");
            SetActive(m_TipsText, false);
            currPlayState = PlayState.EndPlay;
            if(tipsQueue.Count > 0)
            {
                SetTips(GetTips());
            }
        }));
    }

    private IEnumerator PlayAinCallBack(float time,Action callBack )
    {
        yield return new WaitForSeconds(time);
        if (callBack!=null)
        {
            callBack();
        }
    }


    private string GetTips()
    {
        string tips = "";
        if (tipsQueue.Count>0)
        {
            tips = tipsQueue.Dequeue();
        }
        return tips;
    }
    private void AddTips(string str)
    {
        if (tipsQueue == null )
        {
            tipsQueue = new Queue<string>();
        }
        tipsQueue.Enqueue(str);
    }
}