using NierProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSystem : SystemRoot
{
    private static LoginSystem instance;
    public static LoginSystem Instance { get { return instance; } }

    public LoginPanel LoginPanel;




    public override void InitSystem()
    {
        base.InitSystem();
        instance = this;
        this.Log("Init LoginSystem Done.");
    }

    /// <summary>
    /// 进入登录界面
    /// </summary>
    public void EnterLogin()
    {
        //开启界面
        LoginPanel.SetPanelState(true);




    }

    /// <summary>
    /// 处理登录应答
    /// </summary>
    /// <param name="msg"></param>
    public void HandleRepLogin(NierMsg msg)
    {
        gameRoot.AddTip(TipEnum.Little, "登录成功");
        gameRoot.UserData = msg.respLogin.userData;

    }



}
