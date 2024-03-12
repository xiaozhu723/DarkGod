/****************************************************
    文件：WindowRoot.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 20:6:50
	功能：UI界面基类
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour 
{
    protected ResourceService resService;
    protected AudioService audioService;
    public void SetWinState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }

        if(isActive)
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

    }

    protected virtual void Clear()
    {
        resService = null;
        audioService = null;
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


    protected void SetText(Text txt, string  context = "")
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

    #endregion

    private void OnDestroy()
    {
        Clear();
    }
}