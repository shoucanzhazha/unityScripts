using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCPUtils;
using NierProtocol;
using LogUtils;

public class ClientSession : KCPSession<NierMsg>
{
    protected override void OnConnected()
    {
        GameRoot.Instance.AddTip(TipEnum.Little, "连接服务器成功");
    }

    protected override void OnDisConnected()
    {
        GameRoot.Instance.AddTip(TipEnum.Little, "与服务器断开连接");
    }

    protected override void OnReceiveMsg(NierMsg msg)
    {
        NetService.Instance.AddMsgQue(msg);
    }

    protected override void OnUpdate(DateTime now)
    {

    }
}
