using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region 
public enum Language
{
    Chinese,
    English,
    Arabic,
}

//位置类型
public enum UIformType
{
    //常规窗口
    Normal,
    //固定窗口
    Fixed,
    //弹出窗口
    PopUp
}

//显示类型
public enum UIformShowMode
{
    //常规显示
    Normal,
    //退回上层 栈结构UI
    ReverseChange,
    //隐藏其他
    HideOther
}

//透明度类型
public enum UIformLucenyType
{
    Lucency,
    TransLucency,
    ImPenetrable,
    //可穿透
    Pentrate,
}

#endregion


public class SysDefine
{
    public const string SYS_BaseUICanvas = "UI/BaseUICanvas";
}

public class MagDefine
{
    public const string TalentInfoShow = "TalentInfoShow";

    public const string TalentUnlock = "TalentUnlock";

    public const string BackPackItemInfoShow = "BackPackItemInfoShow";

    public const string RoleWarehouseCgShow = "RoleWarehouseCgInfoShow";

    public const string RoleListAndIndex = "RoleListAndIndex";

    public const string RoleSelectData = "RoleSelectData";

    public const string GameToMain = "GameToMain";

    public const string PVPMatchRoomData = "PVPMatchRoomData";

    public const string PromptFormMsgData = "PromptFormMsgData";
}
