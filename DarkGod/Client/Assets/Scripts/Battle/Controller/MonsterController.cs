/****************************************************
	文件：MonsterController.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/04/01 14:53   	
	功能：怪物控制器
*****************************************************/
using UnityEngine;

public class MonsterController : Controller
{
    public override void Init()
    {
        base.Init();
    }
    void OnAnimatorMove()
    {
        //勾选apply root motion 会导致碰撞体失效 先加上OnAnimatorMove
    }
    private void Update()
    {

        if (isMove)
        {
            //设置方向
            SetDir();
            //移动角色
            SetMove();
        }
    }
    void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    void SetMove()
    {
        controller.Move(transform.forward * Time.deltaTime * Constants.MonsterSpeed);
        controller.Move(Vector3.down * Time.deltaTime * Constants.MonsterSpeed);
    }
}
