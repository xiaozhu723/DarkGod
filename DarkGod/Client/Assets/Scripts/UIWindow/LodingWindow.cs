/****************************************************
    文件：LodingWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 16:19:44
	功能：加载进度界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LodingWindow : WindowRoot
{
    public Slider m_Slider;
    public Text m_ProgressText;
    public Text m_TipsText;

    protected override void InitWindow()
    {
        base.InitWindow();
        m_Slider.value = 0;
        SetText(m_ProgressText, "0%");
        SetText(m_TipsText, "");
    }

    public void SetProgress(float progress)
    {
        SetText(m_ProgressText, string.Format("{0}%", (int)(progress * 100)));
        m_Slider.value = progress;
        if(progress>=1)
        {
            SetActive(gameObject,false);
        }
    }
}