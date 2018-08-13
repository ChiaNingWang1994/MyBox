using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUiId//所有UI的ID定义
{
    Null,
    ID_MenuPanel,//主界面UI的ID
    ID_LoadGamePanel,//加载存档UI的ID
    ID_LoadingPanel1,//加载资源ID1
    ID_LoadingPanel2,//加载资源ID2
    ID_SettingPanel,//设置面板ID
    ID_InfoPanel,//信息面板
    ID_ExitPanel,//退出ID
    ID_GamingPanel,//游戏中ID
    ID_PausePanel,//暂停菜单ID
    ID_SaveGamePanel,//保存存档UI的ID
    ID_WarnPanel//警告UI的ID

}

public enum EUIShowMode
{
    doNothing,//当前UI显示出来的时候其它UI不做任何操作
    hideAllOutAbove,//除了最前面的都隐藏
    hideAll//隐藏所有的UI，包括最前面的
}

public class UIPath
{
    private static Dictionary<EUiId, string> dicPath = new Dictionary<EUiId, string>
    {
        {EUiId.ID_MenuPanel,"PrefabUI/MenuUI"},
        {EUiId.ID_LoadGamePanel,"PrefabUI/LoadGameUI"},
        {EUiId.ID_LoadingPanel1,"PrefabUI/LoadUI_1"},
        {EUiId.ID_LoadingPanel2,"PrefabUI/LoadUI_2"},
        {EUiId.ID_SettingPanel,"PrefabUI/SetUI"},
        {EUiId.ID_InfoPanel,"PrefabUI/AboutUI"},
        {EUiId.ID_ExitPanel,"PrefabUI/ExitUI"},
        {EUiId.ID_GamingPanel,"PrefabUI/GamingUI"},
        {EUiId.ID_PausePanel,"PrefabUI/PauseUI"},
        {EUiId.ID_SaveGamePanel,"PrefabUI/SaveGameUI"},
        {EUiId.ID_WarnPanel,"PrefabUI/WarnUI"},
    };
    public static string getUiIdPath(EUiId id)
    {
        if (dicPath.ContainsKey(id))
        {
            return dicPath[id];
        }
        return null;
    }
}

