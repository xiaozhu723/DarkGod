/****************************************************
	文件：ServerRoot.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/14 13:32   	
	功能：服务器初始化
*****************************************************/
public class ServerRoot
{
	private static ServerRoot instance = null;
	public static ServerRoot Instance { 
		get {  
			if (instance == null)
				instance = new ServerRoot();
			return instance;
		}
	}

	public void Init()
	{
		//数据层
		DBManager.Instance.Init();

        //网络服务系统
        CacheSvc.Instance.Init();

        NetSvc.Instance.Init();

		//登陆系统
		LoginSys.Instance.Init();
    }

	int nSession = 0;
	public int GetSessionID()
	{
		return nSession += 1;
	}
}
