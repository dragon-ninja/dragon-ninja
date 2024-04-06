using System.Collections.Generic;

public class MessageMgr
{
    public delegate void msgDelivery(MsgKV kv);

    //消息缓存   参数为消息分类,委托
    public static Dictionary<string, msgDelivery> msgMap = new Dictionary<string, msgDelivery>();

    public static void init()
    {  msgMap = new Dictionary<string, msgDelivery>();
    }


    //添加消息监听
    public static void AddMsgListener(string msgType, msgDelivery md)
    {
        if (!msgMap.ContainsKey(msgType))
        {
            msgMap.Add(msgType, md);
        }
        else
        {
            msgMap[msgType] += md;
        }
    }

    public static void RemoveMsgListener(string msgType, msgDelivery md)
    {
        if (msgMap.ContainsKey(msgType))
        {
            msgMap[msgType] -= md;
        }
    }

    public static void ClearMsgListener(string msgType, msgDelivery md)
    {
        if (msgMap.ContainsKey(msgType))
        {
            msgMap.Remove(msgType);
        }
    }

    public static void ClearAllMsgListener(string msgType, msgDelivery md)
    {
        if (msgMap != null)
        {
            msgMap.Clear();
        }
    }

    //发送消息
    public static void SendMsg(string msgType, MsgKV kv)
    {
        msgDelivery del;

        if (msgMap.TryGetValue(msgType, out del))
        {
            del?.Invoke(kv);
        }
    }
}


public class MsgKV
{
    public string Key { get; private set; }
    public object Value { get; private set; }

    public MsgKV(string key, object value)
    {
        this.Key = key;
        this.Value = value;
    }

}
