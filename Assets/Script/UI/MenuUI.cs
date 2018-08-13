using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 应用层的ui
/// </summary>
public class MenuUI : UIBasePanel {

    private GameObject buttonSet;
    private GameObject buttonExit;
    private GameObject buttonInfo;
    private GameObject buttonNewGame;
    private GameObject buttonLoadGame;
    private GameObject buttonInfinity;

    public override void OnInit()//初始化UI控件
    {
        //给定ID
        this.id = EUiId.ID_MenuPanel;//对UI的ID赋值（重点，一定要赋值）
        this.IsKeepAbove = true;//保持始终不消失

        //用于查找游戏对象
        buttonSet = UtilUI.findChild(this.gameObject, "SettingButton").gameObject;
        buttonExit = UtilUI.findChild(this.gameObject, "ExitButton").gameObject;
        buttonInfo = UtilUI.findChild(this.gameObject, "InfoButton").gameObject;
        buttonNewGame = UtilUI.findChild(this.gameObject, "NewGameButton").gameObject;
        buttonLoadGame = UtilUI.findChild(this.gameObject, "LoadGameButton").gameObject;
        buttonInfinity = UtilUI.findChild(this.gameObject, "InfinityModButton").gameObject;
        UGUIEventListener.Get(buttonSet).onClick = OnBtnSet;//事件和方法的关联
        UGUIEventListener.Get(buttonExit).onClick = OnBtnExit;
        UGUIEventListener.Get(buttonInfo).onClick = OnBtnInfo;
        UGUIEventListener.Get(buttonNewGame).onClick = OnBtnNew;
        UGUIEventListener.Get(buttonLoadGame).onClick = OnBtnLoad;
        UGUIEventListener.Get(buttonInfinity).onClick = OnBtnInfinity;

    }

    #region 通过事件触发的函数
    private void OnBtnSet(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_SettingPanel);
    }
    private void OnBtnExit(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_ExitPanel);
    }
    private void OnBtnInfo(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_InfoPanel);
    }
    private void OnBtnNew(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_LoadingPanel1);
        LoadSceneHelper.Instance.loadScene("MyBoxBuild", delegate
         {
             UIPanelManager.Instance.hideUILater(EUiId.ID_LoadingPanel1, 5.0f);
             UIPanelManager.Instance.showLater(EUiId.ID_GamingPanel, 5.0f);
         });
    }
    private void OnBtnLoad(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_LoadGamePanel);
    }
    private void OnBtnInfinity(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_LoadingPanel2);
        LoadSceneHelper.Instance.loadScene("MyBoxInfinity", delegate
        {
            UIPanelManager.Instance.hideUILater(EUiId.ID_LoadingPanel2, 8.0f);
            UIPanelManager.Instance.showLater(EUiId.ID_GamingPanel, 8.0f);
            //UIPanelManager.Instance.hideUI(EUiId.ID_LoadingPanel2);//隐藏loading界面
            //UIPanelManager.Instance.showUI(EUiId.ID_GamingPanel);//显示主UI
        });
        
    }

    #endregion

}
