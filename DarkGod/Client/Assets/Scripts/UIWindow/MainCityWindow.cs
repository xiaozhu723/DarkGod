/****************************************************
    文件：MainCityWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/16 14:38:49
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class MainCityWindow : WindowRoot
{
    public Text m_FightingText;//战斗力
    public Text m_PowerText;//体力
    public Text m_LevelText;//等级
    public Text m_VIPLevelText;//
    public Text m_Exp;//经验
    public Text m_PlayerName;//
    public Button m_UpFigthingBtn;//提升战斗力
    public Button m_BuyProwerBtn;//购买体力
    public Button m_AutoTaskBtn;//自动任务按钮
    public Button m_ChargeBtn;//商城
    public Button m_ArenaBtn;//副本
    public Button m_TaskBtn;//任务
    public Button m_StrongBtn;//强化
    public Button m_MkcoinBtn;//铸造
    public Toggle m_MenuToggle;//展开收缩按钮
    public Slider m_PowerSlider;//体力进度条
    public GridLayoutGroup m_Grid;
    public Animation m_MenuAin;
    public TouchComponent touch;
    protected override void InitWindow()
    {
        base.InitWindow();
        m_FightingText.text = "";
        m_PowerText.text = "";
        m_LevelText.text = "";
        m_VIPLevelText.text = "";
        m_MenuToggle.isOn = false;
        m_PowerSlider.value = 0;
        m_MenuToggle.isOn = true;
    }

    private void Start()
    {
        m_UpFigthingBtn.onClick.AddListener(OnClickUpFigthingBtn);
        m_BuyProwerBtn.onClick.AddListener(OnClickBuyProwerBtn);
        m_AutoTaskBtn.onClick.AddListener(OnClickAutoTaskBtn);
        m_ChargeBtn.onClick.AddListener(OnClickChargeBtn);
        m_ArenaBtn.onClick.AddListener(OnClickArenaBtn);
        m_TaskBtn.onClick.AddListener(OnClickTaskBtn);
        m_StrongBtn.onClick.AddListener(OnClickStrongBtn);
        m_MkcoinBtn.onClick.AddListener(OnClickMkcoinBtn);
        m_MenuToggle.onValueChanged.AddListener(OnClickAddToggle);
        touch.UpdateDir = UpdatePlayerDir;
        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        if (playerData == null) return;
        SetPower(playerData.Power, playerData.Level);
        SetText(m_PlayerName, playerData.Name);
        SetText(m_LevelText, playerData.Level);
        SetText(m_FightingText, PECommon.GetFightByProps(playerData));
        SetExpImageSize();
        SetExp(playerData.Exp, playerData.Level);
    }

    //设置体力
    public void SetPower(int power, int lv)
    {
        int powerLimit = PECommon.GetPowerLimit(lv);
        SetText(m_PowerText, string.Format("体力:{0}/{1}", power, powerLimit));
        m_PowerSlider.value = power / powerLimit;
    }

    //设置经验条缩放
    public void SetExpImageSize()
    {
        float globalRate = 1.0f * Constants.ScreenStandrdHeight / Screen.height;
        float with = ((Screen.width * globalRate) - 180) / 10;
        m_Grid.cellSize = new Vector2(with, 10);
    }

    public void UpdatePlayerDir(Vector2 dir)
    {
        MainCitySystem.Instance.SetPlayerDir(dir);
    }

    //设置经验条缩放
    public void SetExp(int exp, int lv)
    {
     
        int maxExp = PECommon.GetExpUpValByLevel(lv);
        int expPrg = (int)((exp * 1.0f / maxExp) * 100);
        int nIndex = expPrg / 10;
        SetText(m_Exp, expPrg + "%");
        for (int i = 0; i < m_Grid.transform.childCount; i++)
        {
            Image temp = m_Grid.transform.GetChild(i).GetComponent<Image>();
            if (temp == null) continue;
            if (i< nIndex)
            {
                temp.fillAmount = 1;
            }
            else if( i == nIndex)
            {
                temp.fillAmount = (expPrg %10)*1.0f/10;
            }
            else
            {
                temp.fillAmount = 0;
            }
        }
    }

    //点击提升战斗力按钮
    public void OnClickUpFigthingBtn()
    {
        PECommon.Log("点击提升战斗力按钮");
    }
    //点击购买体力按钮
    public void OnClickBuyProwerBtn()
    {
        PECommon.Log("点击购买体力按钮");
    }
    //点击自动任务按钮
    public void OnClickAutoTaskBtn()
    {
        PECommon.Log("点击自动任务按钮");
    }
    //点击副本按钮
    public void OnClickChargeBtn()
    {
        PECommon.Log("点击商城按钮");
    }
    //点击商城按钮
    public void OnClickArenaBtn()
    {
        PECommon.Log("点击商城按钮");
    }
    //点击任务按钮
    public void OnClickTaskBtn()
    {
        PECommon.Log("点击任务按钮");
    }
    //点击强化按钮
    public void OnClickStrongBtn()
    {
        PECommon.Log("点击强化按钮");
    }
    //点击铸造按钮
    public void OnClickMkcoinBtn()
    {
        PECommon.Log("点击铸造按钮");
    }
    //点击界面展开动画开关
    public void OnClickAddToggle(bool isOn)
    {
        audioService.PlayUIMusic(Constants.UIExtenBtn);
        AnimationClip clip = null;
        if(isOn)
        {
            clip = m_MenuAin.GetClip("OpenMenuAin");
        }
        else
        {
            clip = m_MenuAin.GetClip("CloseMenuAin");
        }
        PECommon.Log(clip.name);
        m_MenuAin.Play(clip.name);
    }
}