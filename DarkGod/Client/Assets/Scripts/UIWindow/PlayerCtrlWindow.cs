/****************************************************
    文件：PlayerCtrlWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/29 15:8:47
	功能：玩家战斗控制界面
*****************************************************/

using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrlWindow : WindowRoot
{
    private PlayerData playerData;
    public TouchComponent touch;
    public Text m_LevelText;//等级
    public Text m_Exp;//经验
    public Text m_HP;//血量
    public Text m_PlayerName;//名字
    public Image m_HPImage;//血量
    public GridLayoutGroup m_Grid;
    public Transform m_SkillBtnParent;
    public Button m_ExitBtn;
    int maxHP;
    int currHP;
    protected override void InitWindow()
    {
        base.InitWindow();
        RefreshUI();
    }

    private void Start()
    {
        touch.UpdateDir = UpdatePlayerDir;
        for (int i = 0; i < m_SkillBtnParent.childCount; i++)
        {
            SkillItemBtn item = m_SkillBtnParent.GetChild(i).GetComponent<SkillItemBtn>();
            if(item == null) continue;
            SkillData skillData = resService.GetSkillData(item.CurrSkillID);
            if (skillData == null) continue;
            item.Init(skillData.nCDTime);
        }
        m_ExitBtn.onClick.AddListener(OnClickExitBtn);
    }

    private void OnClickExitBtn()
    {
        //battleSystem.battleEndWindow.SetBattleEndType(BattleEndType.Pause);
        //battleSystem.battleEndWindow.SetWinState(true);
        battleSystem.BattleManager.EndBattle(true, 1000);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleSystem.Instance.RequestNormalAttack();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_SkillBtnParent.GetChild(1).GetComponent<SkillItemBtn>().OnClickSkillBtn();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_SkillBtnParent.GetChild(2).GetComponent<SkillItemBtn>().OnClickSkillBtn();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_SkillBtnParent.GetChild(3).GetComponent<SkillItemBtn>().OnClickSkillBtn();
        }
    }
    public void RefreshUI()
    {
        playerData = GameRoot.Instance.PlayerData;
        if (playerData == null) return;
        currHP = maxHP = playerData.hp;
        SetText(m_LevelText, playerData.Level);
        SetText(m_PlayerName, playerData.Name);
        SetExpImageSize();
        SetExp(playerData.Exp, playerData.Level);
        SetHP();
    }

    public void SetHP()
    {
        SetText(m_HP, playerData.hp + "/" + maxHP);
        m_HPImage.fillAmount = playerData.hp * 1.0F / maxHP;
    }

    public void UpdateHP(int newHp)
    {
        currHP = newHp;
        if(currHP < 0)
        {
            currHP = 0;
        }
        SetText(m_HP, currHP + "/" + maxHP);
        m_HPImage.fillAmount = currHP * 1.0F / maxHP;
    }

    //设置经验条缩放
    public void SetExpImageSize()
    {
        float globalRate = 1.0f * Constants.ScreenStandrdHeight / Screen.height;
        float with = ((Screen.width * globalRate) - 180) / 10;
        m_Grid.cellSize = new Vector2(with, 10);
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
            if (i < nIndex)
            {
                temp.fillAmount = 1;
            }
            else if (i == nIndex)
            {
                temp.fillAmount = (expPrg % 10) * 1.0f / 10;
            }
            else
            {
                temp.fillAmount = 0;
            }
        }
    }

    public void UpdatePlayerDir(Vector2 dir)
    {
        battleSystem.SetPlayerDir(dir);
    }
    public Vector2 GetCurrDir()
    {
        return touch.currentDir;
    }

}