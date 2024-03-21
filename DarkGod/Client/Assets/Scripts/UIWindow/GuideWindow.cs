/****************************************************
    文件：GuideWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/21 16:37:33
	功能：Nothing
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GuideWindow : WindowRoot
{
    struct DilogStruct
    {
        public int key;
        public string TalkTxt;
    };
    public Text txtTalk;
    public Text txtName;
    public Button btnNext;
    public Image imgIcon;
    Sprite defultGuide;
    PlayerData playerData;
    AutoGuideData tGuideData;
    GuideDataInfo NpcInfo;
    int currIndex;
    List<DilogStruct> tDilogArr = new List<DilogStruct>();
    protected override void InitWindow()
    {
        base.InitWindow();
        txtTalk.text = "";
        txtName.text = "";
        defultGuide = imgIcon.sprite;
        RefreshUI();
    }
    private void Start()
    {
        btnNext.onClick.AddListener(OnClickNextBtn);
    }
    public void RefreshUI()
    {
        playerData = GameRoot.Instance.PlayerData;
        if (playerData == null) return;
        tGuideData = resService.GetAutoGuideCfg(playerData.guideID);
        if (tGuideData == null) return;
        NpcInfo = resService.GetGuideDataInfo(tGuideData.npcID);

        var temp = tGuideData.dilogArr.Split('#');
        //txtName.text = playerData.Name;

        for (int i = 0; i < temp.Length; i++)
        {
            if (!string.IsNullOrEmpty(temp[i].Trim()))
            {
                var arr = temp[i].Split('|');
                DilogStruct dilogStruct = new DilogStruct()
                {
                    key = int.Parse(arr[0]),
                    TalkTxt = arr[1].Replace("$name", playerData.Name),
                };

                tDilogArr.Add(dilogStruct);
            }
        }
        currIndex = 0;
        SetTalkText(currIndex);
    }

    private void SetTalkText(int index)
    {
        if (tDilogArr.Count <= index) return;
        DilogStruct dilogStruct = tDilogArr[index];
        string name = "";
        string path = "";
        if (tGuideData.npcID == -1)
        {
            if (dilogStruct.key == 1)
            {
                imgIcon.sprite = defultGuide;
                name = "小芸";
            }
            else
            {
                path = PathDefine.PlayerAssassinImage;
                name = playerData.Name;
            }
        }
        else
        {
            if (dilogStruct.key == 1)
            {
                path = NpcInfo.strNpcGuideSpritePath;
                name = NpcInfo.strNpcName;
            }
            else
            {
                path = PathDefine.PlayerAssassinImage;
                name = playerData.Name;
            }
        }
    
        SetText(txtName, name);
        if (path!="")
        SetSprite(imgIcon, path);
        imgIcon.SetNativeSize();
        txtTalk.text = dilogStruct.TalkTxt;
        currIndex++;
    }

    private void OnClickNextBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        if (currIndex >= tDilogArr.Count)
        {
            GameMsg msg = new GameMsg()
            {
                cmd = (int)EMCMD.RequestGuide,
                reqGuide = new RequestGuide()
                { nGuideID = playerData.guideID,
                }
            };
             netServer.SendMessage(msg);
            SetWinState(false);
            return;
        }
        SetTalkText(currIndex);
    }
}

