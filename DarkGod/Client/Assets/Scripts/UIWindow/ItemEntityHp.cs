/****************************************************
    文件：ItemEntityHp.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/4/2 20:9:26
	功能：血条组件
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHp : MonoBehaviour 
{
    public Image fgGray;
    public Image fgRed;

    public Text txtCritical;
    public Animation txtCriticalAin;

    public Text txtDodge;
    public Animation txtDodgeAin;

    public Text txtHp;
    public Animation txtHpAin;

    private int HP;
    private Transform mItemParent;
    private RectTransform rect;

    private float currentPrg;
    private float targetPrg;
    bool isUpdateBlend = false;
    float globalRate = 1.0f * Constants.ScreenStandrdHeight / Screen.height;
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(mItemParent.position);
        rect.anchoredPosition = screenPos * globalRate;
        UpdateMixBlend();
    }

    public void InitEntityHp(Transform parent,int hp)
    {
        mItemParent = parent;
        rect = GetComponent<RectTransform>();
        HP = hp;
        
        fgGray.fillAmount = 1;
        fgRed.fillAmount = 1;
    }

    public void SetCritical(int num)
    {
        txtCriticalAin.Stop();
        txtCritical.text = "暴击 " + num;
        txtCriticalAin.Play();
    }

    public void SetDodge()
    {
        txtDodgeAin.Stop();
        txtDodge.text = "闪避";
        txtDodgeAin.Play();
    }

    public void SetHurt(int num)
    {
        txtHpAin.Stop();
        txtHp.text = "-" + num;
       txtHpAin.Play();
    }

 
    public void SetHPVal(int oldVal, int newVal)
    {
        currentPrg = oldVal * 1.0f / HP;
        targetPrg = newVal * 1.0f / HP;
        fgRed.fillAmount = targetPrg;
        isUpdateBlend = true;
    }

    void UpdateMixBlend()
    {
        if (!isUpdateBlend) return;
        //两数之差小于一帧要减去的值 就结束平滑过程
        if (Mathf.Abs(currentPrg - targetPrg) < Time.deltaTime * Constants.HPAccelerSpeed)
        {
            currentPrg = targetPrg;
            isUpdateBlend = false;
        }
        else if (currentPrg > targetPrg)
        {
            currentPrg -= Time.deltaTime * Constants.HPAccelerSpeed;
        }
        else
        {
            currentPrg += Time.deltaTime * Constants.HPAccelerSpeed;
        }
        fgGray.fillAmount = currentPrg;
    }
}