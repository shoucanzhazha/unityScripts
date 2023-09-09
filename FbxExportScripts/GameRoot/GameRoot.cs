using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using LogUtils;
using NierProtocol;

public class GameRoot : PersistentSingleton<GameRoot>
{
    public GameObject Services;
    private NetService netService;
    private AudioService audioService;
    private ResService resService;

    public GameObject Systems;
    private LobbySystem lobbySystem;
    private BattleSystem battleSystem;
    private LoginSystem loginSystem;
    private TipsSystem tipsSystem;

    [SerializeField]
    private UserData userData;
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }






    // Start is called before the first frame update
    void Start()
    {

        //配置日志属性
        LogConfig cfg = new LogConfig()
        {
            saveName = "NierClientLog.txt",
            loggerType = LoggerType.Unity
        };

        LogPrint.InitSettings(cfg);
        this.ColorLog(LogColorEnum.Green, "Init Cfg Done");

        //初始化客户端的服务
        InitGameRoot();
        this.ColorLog(LogColorEnum.Green, "Init GameRoot Done");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void InitGameRoot()
    {
        netService = Services.GetComponent<NetService>();
        netService.InitNetService();

        audioService = Services.GetComponent<AudioService>();
        audioService.InitAudioService();

        resService = Services.GetComponent<ResService>();
        resService.InitResService();

        lobbySystem = Systems.GetComponent<LobbySystem>();
        lobbySystem.InitSystem();

        battleSystem = Systems.GetComponent<BattleSystem>();
        battleSystem.InitSystem();

        loginSystem = Systems.GetComponent<LoginSystem>();
        loginSystem.InitSystem();

        tipsSystem = Systems.GetComponent<TipsSystem>();
        tipsSystem.InitSystem();

        //登录界面
        loginSystem.EnterLogin();


    }




    #region 公共接口
    public void AddTip(TipEnum tipEnum, string tip, params object[] args)
    {
        switch (tipEnum)
        {
            case TipEnum.Little:
                tipsSystem.AddLittleTip(string.Format(tip, args));
                break;
            case TipEnum.Middle:
                tipsSystem.AddMiddleTip(string.Format(tip, args));
                break;
            case TipEnum.Right:
                tipsSystem.AddRight(string.Format(tip, args));
                break;
            case TipEnum.Left:
                tipsSystem.AddLeftTip(string.Format(tip, args));
                break;
        }
    }




    #endregion

}

