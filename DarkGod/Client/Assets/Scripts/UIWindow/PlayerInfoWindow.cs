/****************************************************
    文件：PlayerInfoWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/19 15:52:46
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using PEProtocol;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerInfoWindow : WindowRoot
{
    public Text m_PlayerName;
    public Text m_Exp;
    public Text m_Power;
    public Text m_Fighting;
    public Text m_Hp;
    public Text m_Hurt;
    public Text m_Def;
    public Image m_ExpProgress;
    public Image m_PowerProgress;
    public Button m_CloseBtn;
    public Button m_AttributeBtn;
    public RawImage m_CharImage;
    private PlayerData playerData;
    private Vector3 startPos;

    public GameObject objDetail;
    public PEGrid itemGrid;
    public GameObject m_DetailItem;
    public Button m_DetailCloseBtn;
    protected override void InitWindow()
    {
        base.InitWindow();
        Show();
        AddDetailItem();
        RegCharImageEvent();
    }

    public void Start()
    {
        m_CloseBtn.onClick.AddListener(OnClickCloseBtn);
        m_AttributeBtn.onClick.AddListener(OnClickAttributeBtn);
        m_DetailCloseBtn.onClick.AddListener(OnClickDetailCloseBtn);
    }

    public void Show()
    {
        playerData = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (playerData == null) return;
        SetPower(playerData.Power, playerData.Level);
        SetText(m_PlayerName, playerData.Name + " LV." + playerData.Level);
        SetText(m_Fighting, "战力     " + PECommon.GetFightByProps(playerData));
        SetExp(playerData.Exp, playerData.Level);
        SetText(m_Hp, "生命     " + playerData.hp);
        SetText(m_Hurt, "伤害     " + (playerData.ad + playerData.ap));
        SetText(m_Def, "防御     " + (playerData.addef + playerData.apdef));
    }

    public void RegCharImageEvent()
    {
        OnPointerDown(m_CharImage.gameObject, (PointerEventData data) =>
        {
            startPos = data.position;
            MainCitySystem.Instance.SetPlayerStartRotation();
        });
        OnDrag(m_CharImage.gameObject, (PointerEventData data) =>
        {
            float y = -(startPos.x + data.position.x) * 0.4f;
            MainCitySystem.Instance.UpdatePlayerRotation(y);
        });
    }
    //设置体力
    public void SetPower(int power, int lv)
    {
        int powerLimit = PECommon.GetPowerLimit(lv);
        SetText(m_Power, string.Format("体力:{0}/{1}", power, powerLimit));
        m_PowerProgress.fillAmount = power * 1.0f / powerLimit;
    }

    //设置经验条缩放
    public void SetExp(int exp, int lv)
    {
        int maxExp = PECommon.GetExpUpValByLevel(lv);
        SetText(m_Exp, exp + "/" + maxExp);
        m_ExpProgress.fillAmount = exp * 1.0f / maxExp;
    }
    //点击详细属性按钮
    public void OnClickAttributeBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        objDetail.SetActive(true);
    }

    public void OnClickCloseBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        MainCitySystem.Instance.ClosePlayerInfoWindow();
    }

    public void OnClickDetailCloseBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        objDetail.SetActive(false);
    }

    public void AddDetailItem()
    {
        List<UIItemData> list = new List<UIItemData>()
        {
             new DetailItemData(){ nIndex = 0, Name = "生命值", Num = playerData.hp.ToString()},
             new DetailItemData(){ nIndex = 1, Name = "物理攻击", Num = playerData.ad.ToString()},
             new DetailItemData(){ nIndex = 2, Name = "魔法攻击", Num = playerData.ap.ToString()},
             new DetailItemData(){ nIndex = 3, Name = "物理防御", Num = playerData.addef.ToString()},
             new DetailItemData(){ nIndex = 4, Name = "魔法防御", Num = playerData.apdef.ToString()},
             new DetailItemData(){ nIndex = 5, Name = "闪避概率", Num = playerData.dodge.ToString()+"%"},
             new DetailItemData(){ nIndex = 6, Name = "穿透比率", Num = playerData.pierce.ToString()+"%"},
             new DetailItemData(){ nIndex = 7, Name = "暴击概率", Num = playerData.critical.ToString()+"%"},
        };

        itemGrid.UpdateItemList(list);
    }
}

public class DetailItemData : UIItemData
{
    public string Name;
    public string Num;

}

