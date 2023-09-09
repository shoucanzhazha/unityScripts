using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using LogUtils;
using KCPUtils;
using NierProtocol;

public class NetService : MonoBehaviour
{
    private static NetService instance;
    public static NetService Instance { get { return instance; } }


    public static readonly string msg_lock = "msg_lock";
    private KCPNet<ClientSession, NierMsg> client = null;
    private Queue<NierMsg> msgQue = null;
    private Task<bool> checkTask = null;
    //检测计数
    private int checkCounter = 0;

    /// <summary>
    /// 初始化网络服务
    /// </summary>
    public void InitNetService()
    {
        instance = this;
        client = new KCPNet<ClientSession, NierMsg>();
        msgQue = new Queue<NierMsg>();

        client.StartAsClient(ServerConfig.LocalDevInnerIP, ServerConfig.UdpPort);

        KCPTools.LogFunction = this.Log;
        KCPTools.WarnFunction = this.Warn;
        KCPTools.ErrorFunction = this.Error;
        KCPTools.ColorLogFunction = (color, msg) =>
        {
            this.ColorLog((LogColorEnum)color, msg);
        };

        string serverIP;
        int serverPort;

        LoginSystem loginSystem = LoginSystem.Instance;
        if (loginSystem != null && loginSystem.LoginPanel.toggle_Login.isOn)
        {
            serverIP = ServerConfig.LocalDevInnerIP;
            serverPort = ServerConfig.UdpPort;
        }
        else
        {
            serverIP = ServerConfig.LocalDevInnerIP;
            serverPort = ServerConfig.UdpPort;
        }

        client.StartAsClient(serverIP, serverPort);
        checkTask = client.ConnectServer(100);

        this.Log("Init NetService Done.");
    }




    public void Update()
    {
        //尝试进行连接
        if (checkTask != null && checkTask.IsCompleted)
        {
            ClientConnect();
        }

        //进行通信
        if (client != null && client.clientSession != null)
        {
            if (msgQue.Count > 0)
            {
                lock (msg_lock)
                {
                    NierMsg msg = msgQue.Dequeue();
                    HandOutMsg(msg);
                }
            }
        }


        


    }

    /// <summary>
    /// 客户端尝试连接服务器
    /// </summary>
    private void ClientConnect()
    {
        if (checkTask.Result)
        {
            //连接成功
            this.ColorLog(LogColorEnum.Green, "ConnectServer Success.");
            checkTask = null;

            //开始发送Ping消息


        }
        else
        {
            checkCounter++;

            if (checkCounter > 4)
            {
                GameRoot.Instance.AddTip(TipEnum.Little, "无法连接服务器，请检查网络状况。");
                this.Error("Connect Failed {0} Times,Check Your Network Connection.", checkCounter);
                checkTask = null;
            }
            else
            {
                this.Warn("Connect Failed {0} Times,Retry...", checkCounter);
                checkTask = client.ConnectServer(100);
            }


        }
    }





    
    /// <summary>
    /// 接收，分发信息
    /// </summary>
    /// <param name="msg"></param>
    public void AddMsgQue(NierMsg msg)
    {
        lock (msg_lock)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void HandOutMsg(NierMsg msg)
    {
        //如果是返回错误码
        if(msg.error != ErrorCode.None)
        {
            switch (msg.error)
            {
                case ErrorCode.AcctIsOnline:
                    GameRoot.Instance.AddTip(TipEnum.Little, "当前账号已经在线。");
                    break;
            }
        }
        else
        {
            switch (msg.cmd)
            {
                case CMD.Respone_Login:
                    LoginSystem.Instance.HandleRepLogin(msg);
                    break;
                case CMD.None:
                    break;
            }
        }







    }



    /// <summary>
    /// 发送信息
    /// </summary>
    public void SendNierMsg(NierMsg msg,Action<bool> callback = null)
    {
        if(client.clientSession != null && client.clientSession.IsConnected())
        {
            client.clientSession.SendMsg(msg);
            callback?.Invoke(true);
        }
        else
        {
            GameRoot.Instance.AddTip(TipEnum.Little, "服务器未连接");
            this.Error("服务器未连接");
            callback?.Invoke(false);
        }
    }

}
