/****************************************************
    文件：StrongWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/22 10:56:5
	功能：Nothing
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrongWindow : WindowRoot
{
    public PEGrid m_StorngGrid;
    public Transform m_StarParent;
    public Image m_CurrIcon;
    public Text m_CurrStarLevel;
    public Text m_CurrHP;
    public Text m_StrongHP;
    public Text m_CurrHurt;
    public Text m_StrongHurt;
    public Text m_CurrDef;
    public Text m_StrongDef;
    public Text m_NeedLevel;
    public Text m_NeedCoinNum;
    public Text m_NeedGemNum;
    public Text m_CoinNum;
    public Button m_StrongBtn;
    public Button m_CloseBtn;
    public Sprite star_1;
    public Sprite star_2;
    public GameObject[] maxLevelHide;
    string[] nameArr = new string[]
    {
        "头部","身体","腰部","手臂","腿部","脚部",
    };
    PlayerData playerData;
    List<Image> starList = new List<Image>();
    StrongData currStrongData;
   int CurrPartID = 0;
    int maxLevel = 0;
    protected override void InitWindow()
    {
        base.InitWindow();
        for (int i = 0; i < m_StarParent.childCount; i++)
        {
            starList.Add(m_StarParent.GetChild(i).GetComponent<Image>());
        }
        Show();
    }

    public void Start()
    {
        m_CloseBtn.onClick.AddListener(OnClickCloseBtn);
        m_StrongBtn.onClick.AddListener(OnClickStrongBtn);
        
    }

    public void Show()
    {
        playerData = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if(playerData== null) return;
        SetText(m_CoinNum, playerData.Coin);
        AddStrongItem();
    }

    private void AddStrongItem()
    {
        int[] StrongStarLevel = playerData.strongArr;
        List<UIItemData> tData = new List<UIItemData>();
        for (int i = 0; i < StrongStarLevel.Length; i++)
        {
            StrongData cfg = resService.GetStrongCfg(i, StrongStarLevel[i]+1);
            StrongItemData strongItemData= new StrongItemData();
            strongItemData.partID = i;
            strongItemData.name = nameArr[i];
            strongItemData.iconPath = cfg.iconPath;
            strongItemData.fun = OnClickToggleCallBack;
            tData.Add(strongItemData);
        }
        m_StorngGrid.UpdateItemList(tData);
        StrongItem item = m_StorngGrid.GetIItem(CurrPartID).GetComponent<StrongItem>();
        if(item!=null)
        {
            item.SetToggleState(true);
        }
    }

    private void UpdateStar(int num)
    {
        SetText(m_CurrStarLevel, num + "星级");
        for (int i = 0; i < starList.Count; i++)
        {
            starList[i].sprite = i <= num - 1 ? star_2 : star_1;
            
        }
    }

    public void RefreshRightUI()
    {
        audioService.PlayUIMusic(Constants.Fbitem);
        maxLevel = resService.GetStrongMaxSartLevel(CurrPartID);
        for (int i = 0; i < maxLevelHide.Length; i++)
        {
            maxLevelHide[i].SetActive(playerData.strongArr[CurrPartID] < maxLevel);
        }
      
        currStrongData = resService.GetStrongCfg(CurrPartID, playerData.strongArr[CurrPartID]+1);

        if (currStrongData == null) return;
        SetSprite(m_CurrIcon, currStrongData.iconPath);
        SetText(m_CurrHP, "生命：      +"+ resService.GetPropAddValPreLv(CurrPartID, playerData.strongArr[CurrPartID],1));
        SetText(m_CurrHurt, "伤害：      +" + resService.GetPropAddValPreLv(CurrPartID, playerData.strongArr[CurrPartID], 2));
        SetText(m_CurrDef, "防御：      +" + resService.GetPropAddValPreLv(CurrPartID, playerData.strongArr[CurrPartID], 3));
        SetText(m_StrongHP, "强化后 +" + currStrongData.nAddHP);
        SetText(m_StrongHurt, "+" + currStrongData.nAddHurt);
        SetText(m_StrongDef, "+" + currStrongData.nAddDef);
        SetText(m_NeedLevel, "需要等级：" + currStrongData.nNeedLevel);
        SetText(m_NeedCoinNum, "需要消耗：      " + currStrongData.nNeedCoin);
        SetText(m_NeedGemNum, playerData.materials+ "/" + currStrongData.nNeedCrystal);
    }

    private void OnClickStrongBtn()
    {
        if (currStrongData == null|| playerData == null) return;
        if (playerData.Level < currStrongData.nNeedLevel)
        {
            GameRoot.AddTips("等级不足！");
            return;
        }
        if (playerData.materials < currStrongData.nNeedCrystal)
        {
            GameRoot.AddTips("材料不足！");
            return;
        }
        if (playerData.Coin < currStrongData.nNeedCoin)
        {
            GameRoot.AddTips("金币不足！");
            return;
        }
        audioService.PlayUIMusic(Constants.UIClickBtn);
        MainCitySystem.Instance.RequestStrong(CurrPartID);
    }

    private void OnClickCloseBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        SetWinState(false);
    }

    public void OnClickToggleCallBack(int id)
    {
        CurrPartID = id;
        UpdateStar(playerData.strongArr[CurrPartID]);
        RefreshRightUI();
    }
}