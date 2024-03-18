/****************************************************
	文件：ServerStart.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 13:31   	
	功能：服务器启动类
*****************************************************/

public class ServerStart
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();

		while (true)
		{
			NetSvc.Instance.Update();
		}
    }
}
